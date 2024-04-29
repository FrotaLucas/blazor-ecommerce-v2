using System.Security.Cryptography;

namespace BlazorEcommerce_V2.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        public readonly DataContext _context;
        public AuthService(DataContext context)
        {

            _context = context;

        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            if( await UserExists(user.Email) )
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "User already exists.",
                };
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new ServiceResponse<int> {  
                Data = user.Id,
                Success = true,
                Message = "Registration sucessfull!"
            };
        }

        private void CreatePasswordHash(string password, out byte[] passwordSalt, out byte[] passordHash)
        {
            using( var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string email)
        {
            if(await _context.Users.AnyAsync( user=> user.Email.ToLower().
                Equals(email.ToLower())))  
            {
                return true;

            }

            return false;
        }
    }
}
