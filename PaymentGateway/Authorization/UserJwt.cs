using System;

namespace PaymentGateway.Authorization
{
    public class UserJwt
    {
        public UserJwt()
        {
        }

        public string Token { get; set; }
        public long ExpiresIn { get; set; }
        public Guid Id { get; internal set; }
    }
}