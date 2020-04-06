using System;
using System.IO;
using System.Security.Authentication;
using System.Threading.Tasks;
using PaymentGateway.Authorization.Data;

namespace PaymentGateway.Authorization.Services
{
    public class AuthService : IAuthService
    {
        private IJwtHandler _jwtHandler;

        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IPasswordService _passwordService;

        public async Task<UserAccount> SaveAsync(string username, string password)
        {
            var hashedPassword = _passwordService.GenerateHashedPassword(password);

            var userAccount = new UserAccount
            {
                Password = hashedPassword.Hash,
                Salt = hashedPassword.Salt,
                Username = username
            };

            await _userAccountRepository.SaveAsync(userAccount);

            return userAccount;
        }

        public async Task<UserJwt> LoginAsync(string userName, string password)
        {
            try
            {
                var user = await _userAccountRepository.GetByUsernameAsync(userName);

                if (!_passwordService.IsPasswordValid(password, user.Password, user.Salt))
                {
                    throw new InvalidCredentialException("Username or password doesn't match!");
                }

                var jwt = _jwtHandler.Create(user.Id.ToString());
                var payload = new UserJwt() { Id = user.Id, Token = jwt.Token, ExpiresIn = jwt.Expires };

                return payload;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public AuthService(IJwtHandler jwtHandler, IUserAccountRepository userAccountRepository, IPasswordService passwordService)
        {
            _jwtHandler = jwtHandler;
            _userAccountRepository = userAccountRepository;
            _passwordService = passwordService;
        }
    }
}
