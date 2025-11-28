using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.GpsDTOS;
using LinkO.Shared.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkO.Presentation.Controllers
{
    public class GpsController : ApiBaseController
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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<GpsDTO>> GetGpsLocation()
        {
            var gpsLocation = await _gpsService.GetGpsLocationAsync(GetUserEmail());
            return HandleResult<GpsDTO>(gpsLocation);
        }
    }
}
