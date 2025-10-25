using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.ViewModels.DeviceViewModel;
using SmartCareDAL.Models;
using SmartCareDAL.Repositories.Interface;

namespace SmartCareBLL.Services.Classes
{
    public class DeviceCommandService : IDeviceCommandService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeviceCommandService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DeviceCommandViewModel> SendBeepCommandAsync(int userId)
        {
            var command = new DeviceCommand
            {
                UserId = userId,
                CommandType = "Beep",
                CommandData = "Duration:2s",
                CreatedAt = DateTime.UtcNow,
                IsExecuted = false
            };

            await _unitOfWork.DeviceCommands.AddAsync(command);
            await _unitOfWork.SaveChangesAsync();

            return new DeviceCommandViewModel
            {
                Id = command.Id,
                UserId = userId,
                CommandType = command.CommandType,
                CommandData = command.CommandData,
                IsExecuted = false
            };
        }

        public async Task<IEnumerable<DeviceCommandViewModel>> GetPendingCommandsAsync(int userId)
        {
            var commands = await _unitOfWork.DeviceCommands.FindAsync(c => c.UserId == userId && !c.IsExecuted);
            return commands.Select(c => new DeviceCommandViewModel
            {
                Id = c.Id,
                UserId = c.UserId,
                CommandType = c.CommandType,
                CommandData = c.CommandData,
                IsExecuted = c.IsExecuted
            });
        }

        public async Task<DeviceCommandViewModel?> GetTopPendingCommandAsync(int userId)
        {
            var command = (await _unitOfWork.DeviceCommands.FindAsync(
                c => c.UserId == userId && !c.IsExecuted))
                .OrderBy(c => c.Id) // or by CreatedAt if you have it
                .FirstOrDefault();

            if (command == null)
                return null;

            return new DeviceCommandViewModel
            {
                Id = command.Id,
                UserId = command.UserId,
                CommandType = command.CommandType,
                CommandData = command.CommandData,
                IsExecuted = command.IsExecuted
            };
        }


        public async Task<bool> MarkCommandAsExecutedAsync(int commandId)
        {
            var command = await _unitOfWork.DeviceCommands.GetByIdAsync(commandId);
            if (command == null) return false;

            command.IsExecuted = true;
            command.ExecutedAt = DateTime.UtcNow;

            _unitOfWork.DeviceCommands.Update(command);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

    }
}
