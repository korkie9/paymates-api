using paymatesapi.DTOs;
using paymatesapi.Contexts;
using paymatesapi.Helpers;
using Microsoft.EntityFrameworkCore;
using paymatesapi.Entities;
using paymatesapi.Models;


namespace paymatesapi.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IJwtUtils _jwtUitls;
        public UserService(DataContext dataContext, IJwtUtils jwtUtils)
        {
            _dataContext = dataContext;
            _jwtUitls = jwtUtils;
        }
        private static User dummyUser = new User
        {
            Uid = "ewrferf",
            Name = "randow",
            Surname = "korkie",
            Username = "korkews",
            Email = "werfrw@sewrf.com",
            Password = "erfwefwf"

        };
        public AuthenticationResponse getUser(string id)
        {
            return new AuthenticationResponse(dummyUser, "token");
        }
        public async Task<AuthenticationResponse> registerUser(UserDTO user)
        {
            var dbUser = _dataContext.Users.Any(u => u.Username == user.Username || u.Email == user.Email);
            //TODO: Test this
            if (dbUser == true) return new AuthenticationResponse(null, null);
            Guid guid = Guid.NewGuid();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);

            User newUser = new User
            {
                Uid = guid.ToString(),
                Name = user.Name,
                Surname = user.Surname,
                Username = user.Username,
                Email = user.Email,
                PhotoUrl = user.PhotoUrl ?? null,
                Password = passwordHash,
            };
            _dataContext.Add(newUser);
            await _dataContext.SaveChangesAsync();
            var token = _jwtUitls.GenerateJwtToken(newUser);

            return new AuthenticationResponse(newUser, token);
        }
        public AuthenticationResponse loginUser(UserCreds creds)
        {
            var dbUser = _dataContext.Users.Where(u => u.Username == creds.Username || u.Email == creds.Username).FirstOrDefault();
            if (dbUser == null) return new AuthenticationResponse(null, null);
            if (!BCrypt.Net.BCrypt.Verify(creds.Password, dbUser.Password)) return new AuthenticationResponse(null, null);

            var token = _jwtUitls.GenerateJwtToken(dbUser);
            return new AuthenticationResponse(dbUser, token);
        }
        public bool deleteUser(string id)
        {
            return true;
        }
        public AuthenticationResponse updateUser(UserDTO user)
        {
            return new AuthenticationResponse(dummyUser, "token");
        }

    }
}
