using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Authorization
{
    public class Login
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}