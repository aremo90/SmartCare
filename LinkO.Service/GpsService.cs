using AutoMapper;
using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.Domin.Models.IdentityModule;
using LinkO.Service.Exceptions;
using LinkO.ServiceAbstraction;
using Linko.Service.Specification;
using LinkO.Shared.CommonResult;
using LinkO.Shared.DTOS.GpsDTOS;
using Microsoft.AspNetCore.Identity;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Services
{
    public class GpsService : IGpsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public GpsService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }


        public async Task<Result<GpsDTO>> GetGpsLocationAsync(string Email)
        {
            var User = await _userManager.FindByEmailAsync(Email);
            if (User is null)
                return Error.NotFound();

            var gpsRepo = _unitOfWork.GetRepository<GpsLocation, int>();
            var gpsSpec = new BaseSpecification<GpsLocation, int>(g => g.UserId == User.Id);
            var gpsLocation = await gpsRepo.GetByIdAsync(gpsSpec);
            if (gpsLocation == null)
            {
                gpsLocation = new GpsLocation
                {
                    UserId = User.Id,
                    Latitude = 29.965157,
                    Longitude = 31.015914,
                    CreatedAt = DateTime.Now
                };
                await _unitOfWork.GetRepository<GpsLocation, int>().AddAsync(gpsLocation);
            }
            return _mapper.Map<GpsDTO>(gpsLocation);
        }

        public async Task UpdateGpsLocationAsync(string deviceIdentifier, GpsUpdateDTO gpsUpdateDTO)
        {
            if (string.IsNullOrEmpty(deviceIdentifier))
            {
                throw new ArgumentException("Device identifier cannot be null or empty.", nameof(deviceIdentifier));
            }
            if (gpsUpdateDTO == null)
            {
                throw new ArgumentNullException(nameof(gpsUpdateDTO));
            }
            if (gpsUpdateDTO.Latitude < -90 || gpsUpdateDTO.Latitude > 90 || gpsUpdateDTO.Longitude < -180 || gpsUpdateDTO.Longitude > 180)
            {
                throw new ArgumentException("Invalid GPS coordinates.", nameof(gpsUpdateDTO));
            }

            var deviceRepo = _unitOfWork.GetRepository<Device, int>();
            var deviceSpec = new BaseSpecification<Device, int>(d => d.DeviceIdentifier == deviceIdentifier);
            var device = await deviceRepo.GetByIdAsync(deviceSpec);

            if (device == null)
            {
                throw new KeyNotFoundException($"Device not found for the specified device identifier: {deviceIdentifier}");
            }

            var gpsRepo = _unitOfWork.GetRepository<GpsLocation, int>();
            var gpsSpec = new BaseSpecification<GpsLocation, int>(g => g.UserId == device.UserId);
            var gpsLocation = await gpsRepo.GetByIdAsync(gpsSpec);

            if (gpsLocation == null)
            {
                gpsLocation = new GpsLocation
                {
                    UserId = device.UserId,
                    Latitude = gpsUpdateDTO.Latitude,
                    Longitude = gpsUpdateDTO.Longitude,
                    CreatedAt = DateTime.Now
                };
                await _unitOfWork.GetRepository<GpsLocation, int>().AddAsync(gpsLocation);
            }
            else
            {
                gpsLocation.Latitude = gpsUpdateDTO.Latitude;
                gpsLocation.Longitude = gpsUpdateDTO.Longitude;
                gpsLocation.UpdatedAt = DateTime.Now;
                _unitOfWork.GetRepository<GpsLocation, int>().Update(gpsLocation);
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
