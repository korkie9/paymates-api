using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using paymatesapi.Entities;
using System.Security.Cryptography;
using paymatesapi.Models;

namespace paymatesapi.Helpers
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(AuthenticationResponse user);
        public string GenerateRefreshToken();
        // public int? ValidateJwtToken(string? token);
    }

    public class JwtUtils : IJwtUtils
    {
        private readonly IConfiguration _configuration;
        // private readonly AppSettings _appSettings;

        public JwtUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(AuthenticationResponse user)
        {
            if(user == null) return "error";
            if (user?.Uid == null) return "error";
            if (user?.FirstName == null) return "error";
            if (user?.LastName == null) return "error";
            if (user?.Username == null) return "error";
            if (user?.Email == null) return "error";
            if (user?.Uid == null) return "error";

            // generate token that is valid for 5 minutes
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt:Token").Value!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Uid", user.Uid),
                new Claim("Name", user.FirstName),
                new Claim("Surname", user.LastName),
                new Claim("Username", user.Username),
                new Claim("Email", user.Email),
                new Claim("PhotoUrl", user.PhotoUrl ?? ""),
            }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        // public int? ValidateJwtToken(string? token)
        // {
        //     if (token == null)
        //         return null;

        //     var tokenHandler = new JwtSecurityTokenHandler();
        //     var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt:Token").Value!);
        //     try
        //     {
        //         tokenHandler.ValidateToken(token, new TokenValidationParameters
        //         {
        //             ValidateIssuerSigningKey = true,
        //             IssuerSigningKey = new SymmetricSecurityKey(key),
        //             ValidateIssuer = false,
        //             ValidateAudience = false,
        //             // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
        //             ClockSkew = TimeSpan.Zero
        //         }, out SecurityToken validatedToken);

        //         var jwtToken = (JwtSecurityToken)validatedToken;
        //         var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

        //         // return user id from JWT token if validation successful
        //         return userId;
        //     }
        //     catch
        //     {
        //         // return null if validation fails
        //         return null;
        //     }
        // }
    }
}