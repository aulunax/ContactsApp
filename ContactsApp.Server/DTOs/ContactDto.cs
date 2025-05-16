using System.ComponentModel.DataAnnotations;

namespace ContactsApp.Server.DTOs
{
    public class ContactDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; } = string.Empty;

        [StringLength(40)]
        public string? Surname { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? Category { get; set; }
        public string? Subcategory { get; set; }
        public DateTime? BirthDate { get; set; }
        public int UserId { get; set; }
    }
}
