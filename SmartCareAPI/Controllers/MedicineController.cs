using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCareBLL.DTOS.MedicineReminderDTOS;
using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCareAPI.Controllers
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


        [HttpGet]
        public async Task<IActionResult> GetAllReminders()
        {
            try
            {
                var reminders = await _medicineService.GetAllReminderAsync();
                return Ok(ApiResponse<IEnumerable<MedicineReminderDTO>>.SuccessResponse(reminders));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("An unexpected error occurred."));
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRemindersByUserId(int userId)
        {
            try
            {
                var reminders = await _medicineService.GetReminderByUserIdAsync(userId);
                return Ok(ApiResponse<IEnumerable<MedicineReminderDTO>>.SuccessResponse(reminders));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("An unexpected error occurred."));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReminderById(int id)
        {
            try
            {
                var reminder = await _medicineService.GetReminderByIdAsync(id);
                if (reminder == null)
                    return NotFound(ApiResponse<MedicineReminderDTO>.FailResponse($"Reminder with ID {id} not found."));
                
                return Ok(ApiResponse<MedicineReminderDTO>.SuccessResponse(reminder));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("An unexpected error occurred."));
            }
        }

        [HttpGet("esp/{deviceIdentifier}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MedicineReminderDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MedicineReminderDTO>>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRemindersByDevice(string deviceIdentifier)
        {
            try
            {
                var reminders = await _medicineService.GetRemindersByDeviceIdentifierAsync(deviceIdentifier);

                if (reminders == null)
                {
                    string errorMessage = $"Device with identifier '{deviceIdentifier}' not found.";
                    return NotFound(ApiResponse<IEnumerable<MedicineReminderDTO>>.FailResponse(errorMessage));
                }

                return Ok(ApiResponse<IEnumerable<MedicineReminderDTO>>.SuccessResponse(reminders));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("An unexpected error occurred."));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateReminder([FromBody] MedicineReminderCreateDTO model)
        {
            try
            {
                var createdReminder = await _medicineService.CreateReminderAsync(model);
                if (createdReminder == null)
                {
                    return BadRequest(ApiResponse<string>.FailResponse("Could not create reminder."));
                }
                return CreatedAtAction(nameof(GetReminderById), new { id = createdReminder.Id }, ApiResponse<MedicineReminderDTO>.SuccessResponse(createdReminder));
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ApiResponse<string>.FailResponse(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("An unexpected error occurred while creating the reminder."));
            }
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
