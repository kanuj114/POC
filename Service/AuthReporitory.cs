using TokenAuthentication.Database;
using TokenAuthentication.Functionality;
using TokenAuthentication.Models;

namespace TokenAuthentication.Service
{
    public class AuthReporitory : IAuthReporitory
    {
        TokenDbContext _tokenDbContext; //DI for TokenDbContext

        public AuthReporitory(TokenDbContext tokenDbContext)
        {
            _tokenDbContext = tokenDbContext;
        }
        public TblUser Login(string email, string password) // Login function will be called from Controller(username : Anuj, Password : 6583)
        {
            // We need to call certain function to validate the password and convert into Hashing + SALT key, then compared to table column.
            var user = _tokenDbContext.TblUsers.FirstOrDefault(x => x.Email == email);
            if (user == null)
                return null;
            if (!VerifyPasswordHash(password, user.Password, user.Salt))
                return null;
            return user; // auth successful


        }

        public TblUser Register(TblUser user, string password) //
        {

            // Create password hash and salt
            byte[] passwordHash, salt;
            CreatePasswordHash(password, out passwordHash, out salt);
            user.Password = passwordHash;
            user.Salt = salt;

            // Save the user to the database using EF Core
            _tokenDbContext.TblUsers.Add(user);
            _tokenDbContext.SaveChanges();

            // Return the saved user
            return user;

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] salt) // One input, Two output
        {
            // Here we will write the code for Hashing + SALT
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                salt = hmac.Key; // Will get a Random number, Key is a property of hmac(use any custom key ex. flat no., car no.,DOB etc)
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // Hashing
            }
        }
        public bool UserExists(string email)
        {
            if (_tokenDbContext.TblUsers.Any(x => x.Email == email))
                return true;
            return false;

        }


        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
            }
            return true;
        }
    }
}