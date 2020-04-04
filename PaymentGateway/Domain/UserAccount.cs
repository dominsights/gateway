using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway.Domain
{
    [Table("UserAccount")]
    public class UserAccount
    {
        public Guid Id  { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
