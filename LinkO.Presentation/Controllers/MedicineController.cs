using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.AddressDTOS;
using LinkO.Shared.DTOS.MedicineReminderDTOS;
using LinkO.Shared.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkO.Presentation.Controllers
{
    public class MedicineController : ApiBaseController
    {
        private readonly IMedicineService _medicineService;

        public MedicineController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineReminderDTO>>> GetAllRemindersByUserId()
        {
            var Medicines = await _medicineService.GetReminderByUserId(GetUserEmail());
            return HandleResult<IEnumerable<MedicineReminderDTO>>(Medicines);
        }

        [HttpGet("esp/{deviceIdentifier}")]
        //[ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceReminderDTO>>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceReminderDTO>>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<DeviceReminderDTO>>> GetRemindersByDevice(string deviceIdentifier)
        {
            var reminders = await _medicineService.GetRemindersByDeviceIdentifierAsync(deviceIdentifier);
            return HandleResult<IEnumerable<DeviceReminderDTO>>(reminders);
            //return Ok(ApiResponse<IEnumerable<DeviceReminderDTO>>.SuccessResponse(reminders, "Success"));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<MedicineReminderDTO>> CreateReminder([FromBody] CreateMedicineReminderDTO model)
        {
            var createdReminder = await _medicineService.CreateReminderAsync(GetUserEmail() , model);
            return HandleResult<MedicineReminderDTO>(createdReminder);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReminder(int id)
        {
            var result = await _medicineService.DeleteReminderAsync(id);
            if (!result)
                return NotFound(ApiResponse<string>.FailResponse($"Reminder with ID {id} not found."));
            
            return Ok(ApiResponse<string>.SuccessResponse("Reminder deleted successfully."));
        }
    }
}