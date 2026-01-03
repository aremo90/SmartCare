using AutoMapper;
using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.Domin.Models.IdentityModule;
using LinkO.Service.Exceptions;
using LinkO.ServiceAbstraction;
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

        public GpsService(IUnitOfWork unitOfWork , IMapper mapper , UserManager<ApplicationUser> userManager)
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

            var gpsLocations = await _unitOfWork.GetRepository<GpsLocation, int>().GetAllAsync();
            var gpsLocation = gpsLocations.FirstOrDefault(g => g.UserId == User.Id);
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

            var devices = await _unitOfWork.GetRepository<Device, int>().GetAllAsync();
            var device = devices.FirstOrDefault(d => d.DeviceIdentifier == deviceIdentifier);

            if (device == null)
            {
                throw new KeyNotFoundException($"Device not found for the specified device identifier: {deviceIdentifier}");
            }

            var gpsLocations = await _unitOfWork.GetRepository<GpsLocation, int>().GetAllAsync();
            var gpsLocation = gpsLocations.FirstOrDefault(g => g.UserId == device.UserId);

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
