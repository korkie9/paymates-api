﻿using paymatesapi.DTOs;
using paymatesapi.Contexts;
using paymatesapi.Helpers;
using Microsoft.EntityFrameworkCore;
using paymatesapi.Entities;
using paymatesapi.Models;
using System.Security.Cryptography;
using System;

namespace paymatesapi.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly DataContext _dataContext;
        private readonly IJwtUtils _jwtUitls;
        public UserAuthService(DataContext dataContext, IJwtUtils jwtUtils)
        {
            _dataContext = dataContext;
            _jwtUitls = jwtUtils;
        }
        private static User dummyUser = new User
        {
            Uid = "ewrferf",
            FirstName = "randow",
            LastName = "korkie",
            Username = "korkews",
            Email = "werfrw@sewrf.com",
            Password = "erfwefwf",
            RefreshToken = "dfbedrtrdfgb",
            RefreshTokenExpiry = DateTime.Now.AddHours(1)
        };
        public AuthenticationResponse getUser(string id)
        {
            var dbUser = _dataContext.Users.Find(id);
            return new AuthenticationResponse(dbUser);
        }
        public async Task<AuthenticationResponse> registerUser(UserDTO user)
        {
            var dbUser = _dataContext.Users.Any(u => u.Username == user.Username || u.Email == user.Email);
            if (dbUser == true) return new AuthenticationResponse(null);
            Guid guid = Guid.NewGuid();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            string newRefreshToken = _jwtUitls.GenerateRefreshToken();
            User newUser = new User
            {
                Uid = guid.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                PhotoUrl = user.PhotoUrl ?? null,
                Password = passwordHash,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiry = DateTime.Now.AddHours(1)
            };
            _dataContext.Add(newUser);
            await _dataContext.SaveChangesAsync();

            return new AuthenticationResponse(newUser);
        }
        public async Task<AuthenticationResponse> loginUser(UserCreds creds)
        {
            var dbUser = _dataContext.Users.Where(u => u.Username == creds.Username || u.Email == creds.Username).FirstOrDefault();
            if (dbUser == null) return new AuthenticationResponse(null);
            if (!BCrypt.Net.BCrypt.Verify(creds.Password, dbUser.Password)) return new AuthenticationResponse(null);

            var refreshToken = _jwtUitls.GenerateRefreshToken();
            dbUser.RefreshToken = refreshToken;
            dbUser.RefreshTokenExpiry = DateTime.Now.AddHours(1);
            await _dataContext.SaveChangesAsync();
            return new AuthenticationResponse(dbUser);
        }
        public bool deleteUser(string id)
        {
            return true;
        }
        public AuthenticationResponse updateUser(UserDTO user)
        {
            return new AuthenticationResponse(dummyUser);
        }

    }
}
