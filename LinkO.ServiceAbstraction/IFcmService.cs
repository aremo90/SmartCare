using LinkO.Shared.CommonResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.ServiceAbstraction
{
    public interface IFcmService
    {
        Task<Result<string>> SendNotificationAsync(string DeviceIdentifier);
    }
}
