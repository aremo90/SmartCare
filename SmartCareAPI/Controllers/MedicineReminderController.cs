using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.ViewModels.Common;
using SmartCareBLL.ViewModels.MedicineViewModel;

namespace SmartCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class MedicineReminderController : ControllerBase
    {
        private readonly IMedicineReminderService _reminderService;

        public MedicineReminderController(IMedicineReminderService reminderService)
        {
            _reminderService = reminderService;
        }
        #region Get All Medicine

        // ✅ GET: api/medicinereminder
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reminders = await _reminderService.GetAllAsync();
            return Ok(ApiResponse<object>.SuccessResponse(reminders, "Reminders retrieved successfully"));
        }
        #endregion

        #region Get Medicine info for a user

        // ✅ GET: api/medicinereminder/user/5
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var reminders = await _reminderService.GetByUserIdAsync(userId);
            return Ok(ApiResponse<object>.SuccessResponse(reminders, $"Reminders for user {userId} retrieved successfully"));
        }
        #endregion

        #region Get Medicine info by Id


        // ✅ GET: api/medicinereminder/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reminder = await _reminderService.GetByIdAsync(id);
            if (reminder == null)
                return NotFound(ApiResponse<string>.FailResponse("Reminder not found"));

            return Ok(ApiResponse<object>.SuccessResponse(reminder, "Reminder retrieved successfully"));
        }
        #endregion

        #region Add New Medicine

        // ✅ POST: api/medicinereminder
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MedicineReminderCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.FailResponse("Invalid reminder data"));

            try
            {
                var reminder = await _reminderService.CreateAsync(model);
                return Ok(ApiResponse<object>.SuccessResponse(reminder, "Reminder created successfully"));
            }
            catch (ApplicationException ex)
            {
                return Conflict(ApiResponse<string>.FailResponse(ex.Message));
            }
        }
        #endregion

        #region Update Medicine

        // ✅ PUT: api/medicinereminder/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MedicineReminderUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.FailResponse("Invalid data"));

            try
            {
                var updated = await _reminderService.UpdateAsync(id, model);
                if (!updated)
                    return NotFound(ApiResponse<string>.FailResponse("Reminder not found"));

                return Ok(ApiResponse<string>.SuccessResponse("Reminder updated successfully"));
            }
            catch (ApplicationException ex)
            {
                return Conflict(ApiResponse<string>.FailResponse(ex.Message));
            }
        }
        #endregion

        #region Delete Medicine

        // ✅ DELETE: api/medicinereminder/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _reminderService.DeleteAsync(id);
            if (!deleted)
                return NotFound(ApiResponse<string>.FailResponse("Reminder not found"));

            return Ok(ApiResponse<string>.SuccessResponse("Reminder deleted successfully"));
        }
        #endregion
    }


}
