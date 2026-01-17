using FirebaseAdmin.Messaging;
using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.Domin.Models.IdentityModule;
using LinkO.ServiceAbstraction;
using Linko.Service.Specification;
using LinkO.Shared.CommonResult;
using Microsoft.AspNetCore.Identity;


namespace LinkO.Service
{
    public class FcmService : IFcmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public FcmService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<Result<string>> SendNotificationAsync(string DeviceIdentifier)
        {
            if (string.IsNullOrEmpty(DeviceIdentifier))
            {
                throw new ArgumentException("Device identifier cannot be null or empty.", nameof(DeviceIdentifier));
            }
            Device? device = await getDevice(DeviceIdentifier);
            if (device == null)
                return Error.NotFound("Device Not Found", $"No device found with identifier: {DeviceIdentifier}");


            // STEP 1: LOGIC - Lookup the Token based on the Identifier
            var user = await _userManager.FindByIdAsync(device.UserId);
            if (user == null)
                return Error.NotFound("User Not Found", $"No user found with ID: {device.UserId}");

            var userFcmToken = user.DeviceFcmToken;
            if (userFcmToken is null)
                return Error.Conflict();

            var message = new Message()
            {
                Token = userFcmToken,
                Notification = new Notification()
                {
                    Title = "⚠️ Emergency Alert",
                    Body = $"Fall detected by sensor: {device.DeviceName}"
                },
                Android = new AndroidConfig()
                {
                    Priority = Priority.High,
                    Notification = new AndroidNotification()
                    {
                        Sound = "default",
                        ChannelId = "emergency_channel"
                    }
                }
            };

            try
            {
                // Attempt to send the REAL message
                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                return response;
            }
            catch (FirebaseMessagingException ex)
            {
                // 1. Check if the error means the token is dead
                if (ex.MessagingErrorCode == MessagingErrorCode.Unregistered || ex.MessagingErrorCode == MessagingErrorCode.InvalidArgument)
                {
                    // 2. AUTO-CLEANUP: Remove the bad token from your database
                    // This prevents you from trying to send to a dead phone again
                    user.DeviceFcmToken = null;
                    await _userManager.UpdateAsync(user);

                    return Error.Failure("Token Invalid", "The user's device token is no longer valid. Token removed from DB.");
                }

                // Handle other errors (Network, Quota, etc.)
                return Error.Failure("Notification Failed", ex.Message);
            }
        }
        private async Task<Device?> getDevice(string DeviceIdentifier)
        {
            var deviceRepository = _unitOfWork.GetRepository<Device, int>();
            var spec = new BaseSpecification<Device, int>(d => d.DeviceIdentifier == DeviceIdentifier);
            var device = await deviceRepository.GetByIdAsync(spec);
            return device;
        }
    }
}