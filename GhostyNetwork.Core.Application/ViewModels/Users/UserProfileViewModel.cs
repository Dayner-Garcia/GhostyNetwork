using System.ComponentModel.DataAnnotations;

namespace GhostyNetwork.Core.Application.ViewModels.Users
{
    public class UserProfileViewModel
    {
        public int Id { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
