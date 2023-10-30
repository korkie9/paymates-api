using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace paymatesapi.Models
{
    public class UserResponse
    {
        public string Uid { get; set; }
        public string PhotoUrl { get; set; }

        public required string Username { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }
    }
}