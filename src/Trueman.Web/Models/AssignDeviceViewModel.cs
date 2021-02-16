using System.ComponentModel.DataAnnotations;

namespace Trueman.Web.Models
{
    public class AssignDeviceViewModel
    {
        [Required(ErrorMessage = "Device serial number is required")]
        public string SerialNumber { get; set; }
    }
}
