using System.Data;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using TravelOrganization.Data.Models.Enumerators;

namespace TravelOrganization.Data.Models.Account
{

    public class User
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }

        public DateTime BirthDate { get; set; }
        public string ProfilePictureLink { get; set; }


        public DateTime RegistrationDate { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public bool EmailConfirmed { get; set; }
        public string ConfirmationToken { get; set; }
        public DateTime? TokenExpiration { get; set; }

    }
}
