using Microsoft.AspNetCore.Mvc;
using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.ViewModels.Common;
using SmartCareBLL.ViewModels.DeviceViewModel;
using SmartCareDAL.Repositories.Interface;

namespace SmartCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceCommandController : ControllerBase
    {
        private readonly IDeviceCommandService _deviceCommandService;

        public DeviceCommandController(IDeviceCommandService deviceCommandService) 
        {
            _deviceCommandService = deviceCommandService;
        }


        [HttpPost("beep/{userId}")]
        public async Task<IActionResult> SendBeepCommand(int userId)
        {
            var result = await _deviceCommandService.SendBeepCommandAsync(userId);
            return Ok(ApiResponse<DeviceCommandViewModel>.SuccessResponse(result, "Beep command created."));
        }

        [HttpGet("pending/{userId}")]
        public async Task<IActionResult> GetPendingCommands(int userId)
        {
            var result = await _deviceCommandService.GetPendingCommandsAsync(userId);
            return Ok(ApiResponse<IEnumerable<DeviceCommandViewModel>>.SuccessResponse(result, "Pending commands retrieved."));
        }

        [HttpPost("execute/{commandId}")]
        public async Task<IActionResult> MarkCommandAsExecuted(int commandId)
        {
            var success = await _deviceCommandService.MarkCommandAsExecutedAsync(commandId);
            if (!success)
                return NotFound(ApiResponse<string>.FailResponse("Command not found."));

            return Ok(ApiResponse<string>.SuccessResponse("Command marked as executed."));
        }
    }
}
