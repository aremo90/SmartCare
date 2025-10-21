using SmartCareBLL.ViewModels.DeviceViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Interfaces
{
    public interface IDeviceCommandService
    {
        Task<DeviceCommandViewModel> SendBeepCommandAsync(int userId);
        Task<IEnumerable<DeviceCommandViewModel>> GetPendingCommandsAsync(int userId);
        Task<bool> MarkCommandAsExecutedAsync(int commandId);
    }
}
