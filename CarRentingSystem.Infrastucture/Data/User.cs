using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static CarRentingSystem.Infrastucture.Constants.ModelsConstants;

namespace CarRentingSystem.Infrastucture.Data
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(UserFirstNameMaxLength)]
        public string FirstName { get; init; } = null!;

        [Required]
        [MaxLength(UserLastNameMaxLength)]
        public string LastName { get; init; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
