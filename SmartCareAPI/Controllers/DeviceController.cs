using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCareBLL.DTOS.DeviceDTOS;
using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.DTOS.MedicineReminderDTOS;
using SmartCareBLL.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {

        private readonly IDeviceService _deviceService;
        private readonly IMedicineService _medicineService;

        public DeviceController(IDeviceService deviceService, IMedicineService medicineService)
        {
            _deviceService = deviceService;
            _medicineService = medicineService;
        }


        [HttpGet("{userId}")]
        public async Task<IActionResult> GetDeviceInfoByUserId(int userId)
        {
            var deviceInfo = await _deviceService.GetDeviceInfoByUserId(userId);
            if (deviceInfo == null)
            {
                return NotFound();
            }
            return Ok(ApiResponse<DeviceDTO>.SuccessResponse(deviceInfo));
        }

        [HttpPost("register/{userId}")]
        public async Task<IActionResult> RegisterDeviceForUser(int userId, [FromBody] CreateDeviceDTO createDeviceDTO)
        {
            var registeredDevice = await _deviceService.RegisterDeviceForUser(userId, createDeviceDTO);
            return Ok(ApiResponse<DeviceDTO>.SuccessResponse(registeredDevice));
        }

    }
}
