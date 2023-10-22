
namespace paymatesapi.Models;

using paymatesapi.Entities;
using paymatesapi.Models;

public class AuthenticationResponse
{
    public string? Uid { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? PhotoUrl { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }


    public AuthenticationResponse(User? user)
    {
        Uid = user?.Uid;
        FirstName = user?.FirstName;
        LastName = user?.LastName;
        Username = user?.Username;
        Email = user?.Email;
        PhotoUrl = user?.PhotoUrl;
        RefreshToken = user?.RefreshToken;
        RefreshTokenExpiry =  user?.RefreshTokenExpiry;
    }
}