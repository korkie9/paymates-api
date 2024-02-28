﻿using paymatesapi.DTOs;
using paymatesapi.Contexts;
using paymatesapi.Helpers;
using paymatesapi.Entities;
using paymatesapi.Models;
using Microsoft.EntityFrameworkCore;

namespace paymatesapi.Services
{
    public class UserAuthService(DataContext dataContext, IJwtUtils jwtUtils) : IUserAuthService
    {
        private readonly DataContext _dataContext = dataContext;

        private readonly IJwtUtils _jwtUitls = jwtUtils;

        public BaseResponse<User> GetUser(string id)
        {
            var dbUser = _dataContext.Users.Find(id);
            if (dbUser == null)
            {
                return new BaseResponse<User> { Error = new Error { Message = "This user does not exist" } };
            }
            return new BaseResponse<User> { Data = dbUser };
        }
        public async Task<BaseResponse<User>> RegisterUser(UserDTO user)
        {
            var dbUser = _dataContext.Users.Any(u => u.Username == user.Username || u.Email == user.Email);
            if (dbUser == true) return new BaseResponse<User> { Error = new Error { Message = "User already exists" } };
            Guid guid = Guid.NewGuid();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            string newRefreshToken = _jwtUitls.GenerateRefreshToken();
            User newUser = new()
            {
                Uid = guid.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                PhotoUrl = user.PhotoUrl ?? null,
                Password = passwordHash,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiry = DateTime.Now.AddMinutes(1).ToFileTimeUtc() // TODO: Change back to day
            };
            _dataContext.Add(newUser);
            await _dataContext.SaveChangesAsync();


            return new BaseResponse<User> { Data = newUser };
        }
        public async Task<BaseResponse<User>> LoginUser(UserCreds creds)
        {
            var dbUser = _dataContext.Users.Where(u => u.Username == creds.Username || u.Email == creds.Username).FirstOrDefault();
            if (dbUser == null) return new BaseResponse<User>
            {
                Error = new Error { Message = "User does not exist" }
            };
            if (!BCrypt.Net.BCrypt.Verify(creds.Password, dbUser.Password)) return new BaseResponse<User>
            {
                Error = new Error { Message = "Credentials are incorrect" }
            };

            var refreshToken = _jwtUitls.GenerateRefreshToken();
            dbUser.RefreshToken = refreshToken;
            dbUser.RefreshTokenExpiry = DateTime.Now.AddDays(1).ToFileTimeUtc();
            await _dataContext.SaveChangesAsync();
            return new BaseResponse<User> { Data = dbUser };
        }
        public async Task<BaseResponse<bool>> DeleteUser(string uid)
        {
            var dbUser = _dataContext.Users.Find(uid);
            if (dbUser != null)
            {
                _dataContext.Users.Remove(dbUser);
                await _dataContext.SaveChangesAsync();
                return new BaseResponse<bool> { Data = true };
            }
            return new BaseResponse<bool> { Data = false };
        }
        public async Task<BaseResponse<bool>> UpdateUser(UserDTO user)
        {
            var dbUser = _dataContext.Update(user);
            await _dataContext.SaveChangesAsync();
            return new BaseResponse<bool> { Data = true };
        }


        private static BaseResponse<string> ErrorMessage(string ErrorMessage)
        {
            return new BaseResponse<string>
            {
                Error = new Error { Message = ErrorMessage }
            };
        }
        public async Task<BaseResponse<string>> UpdateRefreshToken(RefreshTokenRequest requestBody)
        {
            var dbUser = await _dataContext.Users.FindAsync(requestBody.Uid);
            if (dbUser == null) return ErrorMessage("User not found");
            if (dbUser.RefreshToken != requestBody.RefreshToken)
            {
                return ErrorMessage("User is not authenticated");
            }

            string newRefreshToken = _jwtUitls.GenerateRefreshToken();
            dbUser.RefreshToken = newRefreshToken;
            dbUser.RefreshTokenExpiry = DateTime.Now.AddDays(1).ToFileTimeUtc();
            _dataContext.Users.Update(dbUser);
            await _dataContext.SaveChangesAsync();
            return new BaseResponse<string> { Data = newRefreshToken };
        }

    }
}
