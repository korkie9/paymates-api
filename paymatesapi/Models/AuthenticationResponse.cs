
namespace paymatesapi.Models;

using paymatesapi.Entities;
using paymatesapi.Models;

public class AuthenticationResponse
{
    public string? Uid { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? PhotoUrl { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }


    public AuthenticationResponse(User? user)
    {
        Uid = user?.Uid;
        Name = user?.Name;
        Surname = user?.Surname;
        Username = user?.Username;
        Email = user?.Email;
        PhotoUrl = user?.PhotoUrl;
        RefreshToken = user?.RefreshToken;
        RefreshTokenExpiry =  user?.RefreshTokenExpiry;
    }
}