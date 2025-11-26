using AutoMapper;
using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.GpsDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Services
{
    public class GpsService : IGpsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GpsService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<GpsDTO> GetGpsLocationAsync(string userId)
        {
            if (userId is null)
            {
                throw new ArgumentException("Invalid user ID.");
            }
            var gpsLocations = await _unitOfWork.GetRepository<GpsLocation, int>().GetAllAsync();
            var gpsLocation = gpsLocations.FirstOrDefault(g => g.UserId == userId);
            if (gpsLocation == null)
            {
                throw new KeyNotFoundException("GPS location not found for the specified user ID.");
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
                    Longitude = gpsUpdateDTO.Longitude
                };
                await _unitOfWork.GetRepository<GpsLocation, int>().AddAsync(gpsLocation);
            }
            else
            {
                gpsLocation.Latitude = gpsUpdateDTO.Latitude;
                gpsLocation.Longitude = gpsUpdateDTO.Longitude;
                _unitOfWork.GetRepository<GpsLocation, int>().Update(gpsLocation);
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
