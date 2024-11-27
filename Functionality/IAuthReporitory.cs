using TokenAuthentication.Models;

namespace TokenAuthentication.Functionality
{
    public interface IAuthReporitory
    {
        TblUser Register(TblUser user, string password);
        TblUser Login(string email, string password); 
        bool UserExists(string email); 
    }
}