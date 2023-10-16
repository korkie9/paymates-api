using paymatesapi.Models;

namespace paymatesapi.Services
{
    public interface IUserService
    {
        AuthenticationResponse getUser(string id);
        Task<AuthenticationResponse> registerUser(UserModel user);
        AuthenticationResponse loginUser(UserCreds user);
        bool deleteUser(string id);
        AuthenticationResponse updateUser(User user);



    }
}
