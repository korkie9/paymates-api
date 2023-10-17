using paymatesapi.Models;
using paymatesapi.Contexts;
using paymatesapi.Helpers;
using Microsoft.EntityFrameworkCore;


namespace paymatesapi.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _userContext;
        private readonly IJwtUtils _jwtUitls;
        public UserService(UserContext userContext, IJwtUtils jwtUtils)
        {
            _userContext = userContext;
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
        public async Task<AuthenticationResponse> registerUser(UserModel user)
        {
            var dbUser = _userContext.Users.Any(u => u.Username == user.Username || u.Email == user.Email);
            if (dbUser == true) return null;
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
            _userContext.Add(newUser);
            await _userContext.SaveChangesAsync();
            var token = _jwtUitls.GenerateJwtToken(newUser);

            return new AuthenticationResponse(newUser, token);
        }
        public AuthenticationResponse loginUser(UserCreds creds)
        {
            // var dbUser = _userContext.Users.Any(u => u.Username == creds.Username || u.Email == creds.Username);
            // if (dbUser == null) return null;
            // if (!BCrypt.Net.BCrypt.Verify(creds.Password, dbUser.Password)) return null;

            // var token = _jwtUitls.GenerateJwtToken(dbUser);
            // return new AuthenticationResponse(dbUser, token);
            return null;
        }
        public bool deleteUser(string id)
        {
            return true;
        }
        public AuthenticationResponse updateUser(User user)
        {
            return new AuthenticationResponse(dummyUser, "token");
        }

    }
}
