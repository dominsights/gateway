using System.ComponentModel.DataAnnotations;

namespace BackendTraining.Services
{
    public class Login
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}