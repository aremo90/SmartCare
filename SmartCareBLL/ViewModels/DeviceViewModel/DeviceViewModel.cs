using System;

namespace SmartCareBLL.ViewModels.DeviceViewModel
{
    public class DeviceViewModel
    {
        public int Id { get; set; }
        public string DeviceIdentifier { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string? Model { get; set; }
        public bool IsActive { get; set; }
        public double? SignalStrength { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
