using Microsoft.Graph;
using System.Collections.Generic;

namespace Trueman.Core.Models
{
    public class WindowsAutopilotDevicesAssignedToUserDto
    {
        public List<WindowsAutopilotDeviceIdentity> Items { get; set; }
        public bool HasAlreadyAssignedDevices => Items.Count > 0;

    }
}
