using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCareBLL.DTOS.GpsDTOS;
using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GpsController : ControllerBase
    {
        private readonly IGpsService _gpsService;

        public GpsController(IGpsService gpsService)
        {
            _gpsService = gpsService;
        }

        [HttpPut("update/{deviceIdentifier}")]
        public async Task<IActionResult> UpdateGpsLocation(string deviceIdentifier, [FromBody] GpsUpdateDTO gpsUpdateDTO)
        {
            try
            {
                await _gpsService.UpdateGpsLocationAsync(deviceIdentifier, gpsUpdateDTO);
                return Ok(ApiResponse<string>.SuccessResponse("GPS location updated successfully."));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<string>.FailResponse(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<string>.FailResponse(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("An unexpected error occurred."));
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetGpsLocation(int userId)
        {
            try
            {
                var gpsLocation = await _gpsService.GetGpsLocationAsync(userId);
                return Ok(ApiResponse<GpsDTO>.SuccessResponse(gpsLocation));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<GpsDTO>.FailResponse(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<GpsDTO>.FailResponse(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<GpsDTO>.FailResponse("An unexpected error occurred."));
            }
        }
    }
}
