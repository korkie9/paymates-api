using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using paymatesapi.Entities;
using System.Security.Cryptography;
using paymatesapi.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace paymatesapi.Helpers
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(AuthenticationResponse user);
        public string GenerateRefreshToken();
        public List<Claim> GetClaimsFromHeaderToken();
        // public int? ValidateJwtToken(string? token);

        public string GetUidFromHeaders();
    }

    public class JwtUtils : IJwtUtils
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        // private readonly AppSettings _appSettings;

        public JwtUtils(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public string GenerateJwtToken(AuthenticationResponse user)
        {
            if (user == null) return "error";
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
        public List<Claim> GetClaimsFromHeaderToken()
        {
            List<Claim> claims = new List<Claim>();

            if (_httpContextAccessor?.HttpContext?.Request?.Headers.TryGetValue("Authorization", out var authHeaderValues) == true)
            {
                string authHeaderValue = authHeaderValues.FirstOrDefault()?.ToString() ?? string.Empty;

                if (!string.IsNullOrEmpty(authHeaderValue) && authHeaderValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    string token = authHeaderValue.Substring("Bearer ".Length);

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                    if (securityToken != null)
                    {
                        claims = securityToken.Claims.ToList();
                    }
                }
            }

            return claims;
        }
        public string GetUidFromHeaders()
        {
            List<Claim> claims = GetClaimsFromHeaderToken();

            if (claims.Count == 0)
            {
                return string.Empty;
            }

            var userId = claims.FirstOrDefault(c => c.Type == "Uid")?.Value;
            if (String.IsNullOrEmpty(userId)) return string.Empty;
            return userId;

        }

    }
}