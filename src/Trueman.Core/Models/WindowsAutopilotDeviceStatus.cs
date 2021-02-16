using Microsoft.Graph;
using System;

namespace Trueman.Core.Models
{
    public class WindowsAutopilotDeviceStatus
    {
        public WindowsAutopilotDeviceIdentity WindowsAutopilotDeviceIdentity { get; private set; }
        public bool Found => WindowsAutopilotDeviceIdentity != null;
        public bool AssignedToOtherUser => WindowsAutopilotDeviceIdentity != null && !string.IsNullOrEmpty(WindowsAutopilotDeviceIdentity.UserPrincipalName);
        public bool AssignedToThisUser => WindowsAutopilotDeviceIdentity != null && !string.IsNullOrEmpty(WindowsAutopilotDeviceIdentity.UserPrincipalName) && WindowsAutopilotDeviceIdentity.UserPrincipalName.Equals(UserPrincipalName, StringComparison.InvariantCultureIgnoreCase);
        public string UserPrincipalName { get; private set; }
        public bool Enrolled => WindowsAutopilotDeviceIdentity != null && (WindowsAutopilotDeviceIdentity.EnrollmentState == EnrollmentState.Enrolled || WindowsAutopilotDeviceIdentity.EnrollmentState == EnrollmentState.Blocked || WindowsAutopilotDeviceIdentity.EnrollmentState == EnrollmentState.PendingReset);

        public WindowsAutopilotDeviceStatus(string userPrincipalName, WindowsAutopilotDeviceIdentity windowsAutopilotDeviceIdentity)
        {
            WindowsAutopilotDeviceIdentity = windowsAutopilotDeviceIdentity;
            UserPrincipalName = userPrincipalName;
        }

    }
}
