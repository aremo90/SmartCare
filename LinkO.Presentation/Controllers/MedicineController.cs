using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.AddressDTOS;
using LinkO.Shared.DTOS.MedicineReminderDTOS;
using LinkO.Shared.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkO.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _medicineService;

        public MedicineController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }


        [HttpGet("{userID}")]
        public async Task<IActionResult> GetAllAddressesByUserId(string userId)
        {
            try
            {
                var Medicines = await _medicineService.GetReminderByUserId(userId);
                return Ok(ApiResponse<IEnumerable<MedicineReminderDTO>>.SuccessResponse(Medicines));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("An unexpected error occurred."));
            }
        }

        [HttpGet("esp/{deviceIdentifier}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceReminderDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceReminderDTO>>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRemindersByDevice(string deviceIdentifier)
        {
            try
            {
                var reminders = await _medicineService.GetRemindersByDeviceIdentifierAsync(deviceIdentifier);

                if (reminders == null)
                {
                    string errorMessage = $"Device with identifier '{deviceIdentifier}' not found.";
                    return NotFound(ApiResponse<IEnumerable<DeviceReminderDTO>>.FailResponse(errorMessage));
                }

                return Ok(ApiResponse<IEnumerable<DeviceReminderDTO>>.SuccessResponse(reminders, "Success"));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("An unexpected error occurred."));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateReminder([FromBody] CreateMedicineReminderDTO model)
        {
            var createdReminderGroup = await _medicineService.CreateReminderAsync(model);
            if (createdReminderGroup == null)
            {
                return BadRequest(ApiResponse<string>.FailResponse("Could not create reminder."));
            }
            return Ok(createdReminderGroup);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReminder(int id)
        {
            try
            {
                var result = await _medicineService.DeleteReminderAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.FailResponse($"Reminder with ID {id} not found."));
                
                return Ok(ApiResponse<string>.SuccessResponse("Reminder deleted successfully."));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("An unexpected error occurred while deleting the reminder."));
            }
        }
    }
}