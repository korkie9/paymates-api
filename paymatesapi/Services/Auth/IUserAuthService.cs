using paymatesapi.DTOs;
using paymatesapi.Models;
using paymatesapi.Entities;

namespace paymatesapi.Services
{
    public interface IUserAuthService
    {
        BaseResponse<User> GetUser(string id);
        Task<BaseResponse<User>> RegisterUser(UserDTO user);
        Task<BaseResponse<User>> LoginUser(UserCreds user);
        Task<BaseResponse<bool>> DeleteUser(string id);
        Task<BaseResponse<bool>> UpdateUser(UserDTO user);


    }
}
