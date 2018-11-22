using System.ComponentModel.DataAnnotations;

namespace CRMManager.Models
{
    public class Authentication
    {
        [Required(ErrorMessage = "Please provide your email id", AllowEmptyStrings = false)]
        public string LoginId { get; set; }

        [Required(ErrorMessage = "Please provide your password", AllowEmptyStrings = false)]
        public string Password { get; set; }

        public string UserName { get; set; }

        public string UserType { get; set; }

        public string Mobile { get; set; }

    }
}