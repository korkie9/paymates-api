using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using paymatesapi.Entities;
using System.Security.Cryptography;
using System.Reflection.Metadata;

namespace paymatesapi.Helpers
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(User user, int expiry);

        public string GenerateRefreshToken();

        public List<Claim> GetClaimsFromHeaderToken();

        public string GetUidFromHeaders();

        public List<Claim> GetClaimsFromToken(string token);
    }

    public class JwtUtils(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IJwtUtils
    {
        private readonly IConfiguration _configuration = configuration;

        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string GenerateJwtToken(User user, int expiry)
        {
            // generate token that is valid for 5 minutes
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt:Token").Value!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                new Claim("Uid", user.Uid),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("Username", user.Username),
                new Claim("Email", user.Email),
                new Claim("PhotoUrl", user.PhotoUrl ?? ""),
                new Claim("RefreshToken", user.RefreshToken ?? ""),
                new Claim("Password", user.Password ?? ""),
                new Claim("RefreshTokenExpiry", user.RefreshTokenExpiry.ToString()), /// TODO: Might be an issue
            ]),
                Expires = DateTime.UtcNow.AddMinutes(expiry),
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
            List<Claim> claims = [];

            if (_httpContextAccessor?.HttpContext?.Request?.Headers.TryGetValue("Authorization", out var authHeaderValues) == true)
            {
                string authHeaderValue = authHeaderValues.FirstOrDefault()?.ToString() ?? string.Empty;

                if (!string.IsNullOrEmpty(authHeaderValue) && authHeaderValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    string token = authHeaderValue["Bearer ".Length..];

                    var tokenHandler = new JwtSecurityTokenHandler();

                    if (tokenHandler.ReadToken(token) is JwtSecurityToken securityToken)
                    {
                        claims = securityToken.Claims.ToList();
                    }
                }
            }

            return claims;
        }

        public List<Claim> GetClaimsFromToken(string token)
        {
            List<Claim> claims = [];
            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.ReadToken(token) is JwtSecurityToken securityToken)
            {
                claims = securityToken.Claims.ToList();
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