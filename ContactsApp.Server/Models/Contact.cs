using System.ComponentModel.DataAnnotations;

namespace ContactsApp.Server.Models
{
    /// <summary>
    /// Represents a contact in the application.
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Primary key of the contact.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Name in the contact.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Surname in the contact. Can be null.
        /// </summary>
        public string? Surname { get; set; }

        /// <summary>
        /// Email address of the contact. Can be null.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Navigation property to the category this contact belongs to.
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// Foreign key to the category this contact belongs to. Can be null.
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Navigation property to the subcategory this contact belongs to.
        /// </summary>
        public Subcategory Subcategory { get; set; }

        /// <summary>
        /// Foreign key to the subcategory this contact belongs to. Can be null.
        /// </summary>
        public int? SubcategoryId { get; set; }

        /// <summary>
        /// Custom subcatregory name used when category is selected as "Other"
        /// Empty by default
        /// </summary>
        public string? CustomSubcategory { get; set; }


        /// <summary>
        /// Phone number of the contact. Can be null.
        /// </summary>
        [Phone]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Birth date of the contact. Can be null.
        /// </summary>
        public DateTime? BirthDate { get; set; }


        /// <summary>
        /// Navigation property to the user this contact belongs to.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Foreign key to the user this contact belongs to.
        /// </summary>
        public int UserId { get; set; }
    }
}
