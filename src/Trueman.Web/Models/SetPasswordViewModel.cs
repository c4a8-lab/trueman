using System.ComponentModel.DataAnnotations;

namespace Trueman.Web.Models
{
    public class SetPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; }
    }
}
