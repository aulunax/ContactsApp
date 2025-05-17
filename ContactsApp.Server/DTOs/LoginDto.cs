using System.ComponentModel.DataAnnotations;

namespace ContactsApp.Server.DTOs
{
    /// <summary>
    /// DTO for user login.
    /// </summary>
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]

        public string Password { get; set; }
    }
}
