using paymatesapi.DTOs;
using paymatesapi.Models;
using paymatesapi.Entities;

namespace paymatesapi.Services
{
    public interface IUserService
    {
        AuthenticationResponse getUser(string id, string email, string username);
        Task<AuthenticationResponse> registerUser(UserDTO user);
        Task<AuthenticationResponse> loginUser(UserCreds user);
        bool deleteUser(string id);
        AuthenticationResponse updateUser(UserDTO user);


    }
}
