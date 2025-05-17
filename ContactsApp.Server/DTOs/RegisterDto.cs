using System.ComponentModel.DataAnnotations;

namespace ContactsApp.Server.DTOs
{

    /// <summary>
    /// DTO for user registration.
    /// </summary>
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
