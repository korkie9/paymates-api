﻿using System.ComponentModel.DataAnnotations;

namespace paymatesapi.Models
{
    public class User
        //TODO: add data annotations
    {
        [Key]
        public required string Uid { get; set; }

        [EmailAddress(ErrorMessage ="Valid Email is required")]
        public required string Email { get; set; }
        public string? PhotoUrl { get; set; }

        [StringLength(100)]
        public required string Username {  get; set; }

        [StringLength(70)]
        public required string Name { get; set; }

        [StringLength(70)]
        public required string Surname { get; set; }

        public required string Password { get; set; }

    }
}