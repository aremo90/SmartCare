using Microsoft.AspNetCore.Mvc;
using SmartCareBLL.Services.Interfaces;
using SmartCareBLL.ViewModels.Common;
using SmartCareBLL.ViewModels.DeviceViewModel;
using SmartCareDAL.Models;

[ApiController]
[Route("api/[controller]")]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DeviceController(IDeviceService deviceService)
    {
            _deviceService = deviceService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] DeviceRegisterDto dto)
    {
        var device = await _deviceService.RegisterDeviceAsync(dto.UserId, dto.DeviceIdentifier, dto.Model);
    
        if (device == null)
            return BadRequest(ApiResponse<string>.FailResponse("Failed to register device"));
    
        return Ok(ApiResponse<object>.SuccessResponse(device , "Device registered successfully"));
    }
    
    [HttpPost("pair")]
    public async Task<IActionResult> Pair([FromBody] DevicePairDto dto)
    {
        var success = await _deviceService.PairDeviceAsync(dto.UserId, dto.DeviceIdentifier);
    
        return success
            ? Ok(ApiResponse<string>.SuccessResponse("Device paired successfully"))
            : NotFound(ApiResponse<string>.FailResponse("Device not found"));
    }
    
    [HttpPost("status")]
    public async Task<IActionResult> UpdateStatus([FromBody] DeviceStatusDto dto)
    {
        var success = await _deviceService.UpdateStatusAsync(dto.DeviceIdentifier, dto.IsActive, dto.SignalStrength);
    
        return success
            ? Ok(ApiResponse<string>.SuccessResponse("Device status updated"))
            : NotFound(ApiResponse<string>.FailResponse("Device not found"));
    }
    
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var device = await _deviceService.GetDeviceByUserIdAsync(userId);
    
        return device == null
            ? NotFound(ApiResponse<string>.FailResponse("No device found for this user"))
            : Ok(ApiResponse<DeviceViewModel>.SuccessResponse(device, "Device retrieved successfully"));
    }
}
