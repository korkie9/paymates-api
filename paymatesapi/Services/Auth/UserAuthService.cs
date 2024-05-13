using System.Security.Claims;
using paymatesapi.Contexts;
using paymatesapi.DTOs;
using paymatesapi.Entities;
using paymatesapi.Helpers;
using paymatesapi.Models;

namespace paymatesapi.Services
{
    public class UserAuthService(
        DataContext dataContext,
        IJwtUtils jwtUtils,
        IEmailService emailService,
        IConfiguration configuration
    ) : IUserAuthService
    {
        private readonly DataContext _dataContext = dataContext;

        private readonly IEmailService _emailService = emailService;

        private readonly IJwtUtils _jwtUitls = jwtUtils;
        private readonly IConfiguration _configuration = configuration;

        public BaseResponse<User> GetUser(string id)
        {
            var dbUser = _dataContext.Users.Find(id);
            if (dbUser == null)
            {
                return new BaseResponse<User>
                {
                    Error = new Error { Message = "This user does not exist" }
                };
            }
            return new BaseResponse<User> { Data = dbUser };
        }

        public BaseResponse<User> RegisterUser(UserDTO user)
        {
            var dbUser = _dataContext.Users.Any(u =>
                u.Username == user.Username || u.Email == user.Email
            );
            if (dbUser == true)
                return new BaseResponse<User>
                {
                    Error = new Error { Message = "User already exists" }
                };
            Guid guid = Guid.NewGuid();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            string newRefreshToken = _jwtUitls.GenerateRefreshToken();
            User newUser =
                new()
                {
                    Uid = guid.ToString(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    Email = user.Email,
                    PhotoUrl = user.PhotoUrl ?? null,
                    Password = passwordHash,
                    RefreshToken = newRefreshToken,
                    RefreshTokenExpiry = DateTime.Now.AddDays(1).ToFileTimeUtc()
                };
            var token = _jwtUitls.GenerateJwtToken(newUser, 60);
            var frontendUrl =
                _configuration.GetSection("Urls:FrontendUrl").Value! + "/confirm-account/";
            var email = new EmailBody
            {
                Body =
                    "Hi there, thank you for signing up with Pyamates. Please click the url in this email to verify your account. If you did not create an account with us, please ignore this email. "
                    + frontendUrl
                    + token,
                From = _configuration.GetSection("Email:FromEmail").Value!,
                Subject = "Verify Account",
                To = user.Email
            };
            _emailService.SendEmail(email);

            return new BaseResponse<User> { Data = newUser };
        }

        public async Task<BaseResponse<User>> LoginUser(UserCreds creds)
        {
            var dbUser = _dataContext
                .Users.Where(u => u.Username == creds.Username || u.Email == creds.Username)
                .FirstOrDefault();
            if (dbUser == null)
                return new BaseResponse<User>
                {
                    Error = new Error { Message = "User does not exist" }
                };
            if (!BCrypt.Net.BCrypt.Verify(creds.Password, dbUser.Password))
                return new BaseResponse<User>
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

        public async Task<BaseResponse<bool>> UpdateUser(UserUpdateRequest user)
        {
            var dbUser = _dataContext.Users.Find(user.Uid);
            if (dbUser != null)
            {
                User tempUser = dbUser;
                tempUser.FirstName = user.FirstName;
                tempUser.LastName = user.LastName;
                tempUser.PhotoUrl = user.PhotoUrl;
                var updatedUser = _dataContext.Update(tempUser);
                if (updatedUser != null)
                {
                    await _dataContext.SaveChangesAsync();
                    return new BaseResponse<bool> { Data = true };
                }
            }
            return new BaseResponse<bool> { Error = new Error { Message = "User not found" } };
        }

        private static BaseResponse<string> ErrorMessage(string ErrorMessage)
        {
            return new BaseResponse<string> { Error = new Error { Message = ErrorMessage } };
        }

        public async Task<BaseResponse<string>> UpdateRefreshToken(RefreshTokenRequest requestBody)
        {
            var dbUser = await _dataContext.Users.FindAsync(requestBody.Uid);
            if (dbUser == null)
                return ErrorMessage("User not found");
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

        public async Task<BaseResponse<User>> CreateUser(string token)
        {
            List<Claim> userClaim = _jwtUitls.GetClaimsFromToken(token);
            if (userClaim.ToList().Count == 0)
                return new BaseResponse<User>
                {
                    Error = new Error { Message = "Token is invalid" }
                };

            string uid = userClaim?.FirstOrDefault(c => c.Type == "Uid")?.Value!;
            string email = userClaim?.FirstOrDefault(c => c.Type == "Email")?.Value!;
            string? photoUrl = userClaim?.FirstOrDefault(c => c.Type == "PhotoUrl")?.Value;
            string username = userClaim?.FirstOrDefault(c => c.Type == "Username")?.Value!;
            string firstName = userClaim?.FirstOrDefault(c => c.Type == "FirstName")?.Value!;
            string lastName = userClaim?.FirstOrDefault(c => c.Type == "LastName")?.Value!;
            string password = userClaim?.FirstOrDefault(c => c.Type == "Password")?.Value!;
            string refreshToken = userClaim?.FirstOrDefault(c => c.Type == "RefreshToken")?.Value!;
            long refreshTokenExpiry = Convert.ToInt64(
                userClaim?.FirstOrDefault(c => c.Type == "RefreshTokenExpiry")?.Value
            );

            var user = new User
            {
                Uid = uid,
                Email = email,
                PhotoUrl = photoUrl,
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                Password = password,
                RefreshToken = refreshToken,
                RefreshTokenExpiry = refreshTokenExpiry,
                Verified = true
            };
            if (_dataContext.Users.Find(user.Uid) != null)
                return new BaseResponse<User>
                {
                    Error = new Error { Message = "User has already been created" }
                };
            _dataContext.Add(user);
            await _dataContext.SaveChangesAsync();
            return new BaseResponse<User> { Data = user };
        }
    }
}
