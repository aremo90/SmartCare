using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.DeviceDTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace LinkO.Presentation.Controllers
{
    public class DeviceController : ApiBaseController
    {

        private readonly IDeviceService _deviceService;
        private readonly IMedicineService _medicineService;

        public DeviceController(IDeviceService deviceService, IMedicineService medicineService)
        {
            _deviceService = deviceService;
            _medicineService = medicineService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<DeviceDTO>> GetDeviceInfoByUserId()
        {
            var Result = await _deviceService.GetDeviceInfoByUserId(GetUserEmail());
            return HandleResult<DeviceDTO>(Result);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<DeviceDTO>> RegisterDeviceForUser([FromBody] CreateDeviceDTO createDeviceDTO)
        {
            var Result = await _deviceService.RegisterDeviceForUser(GetUserEmail(), createDeviceDTO);
            return HandleResult<DeviceDTO>(Result);
        }

    }
}
