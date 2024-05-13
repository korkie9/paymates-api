using paymatesapi.DTOs;
using paymatesapi.Entities;
using paymatesapi.Models;

namespace paymatesapi.Services
{
    public interface IUserAuthService
    {
        BaseResponse<User> GetUser(string id);

        BaseResponse<User> RegisterUser(UserDTO user);

        Task<BaseResponse<User>> CreateUser(string token);

        Task<BaseResponse<User>> LoginUser(UserCreds user);

        Task<BaseResponse<bool>> DeleteUser(string id);

        Task<BaseResponse<bool>> UpdateUser(UserUpdateRequest user);

        Task<BaseResponse<string>> UpdateRefreshToken(RefreshTokenRequest requestBody);
    }
}
