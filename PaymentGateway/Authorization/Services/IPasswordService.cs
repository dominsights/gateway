namespace PaymentGateway.Authorization.Services
{
    public interface IPasswordService
    {
        Password GenerateHashedPassword(string password);
        bool IsPasswordValid(string password, string storedHash, string storedSalt);
    }
}