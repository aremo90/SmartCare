using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Presentation.Controllers
{
    public class NotificationController : ApiBaseController
    {
        private readonly IFcmService _fcmService;

        public NotificationController(IFcmService fcmService)
        {
            _fcmService = fcmService;
        }

        [HttpPost("send/{DeviceIdentifier}")]
        public async Task<ActionResult<string>> SendNotification(string DeviceIdentifier)
        {
            var result = await _fcmService.SendNotificationAsync(DeviceIdentifier);
            return HandleResult(result);
        }
    }
}

