namespace ContactsApp.Server.Models
{
    public class Contact
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string? Surname { get; set; }
        public string Email { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }

        public Subcategory Subcategory { get; set; }
        public int SubcategoryId { get; set; }

        /// <summary>
        /// Custom subcatregory name used when category is selected as "Other"
        /// Empty byu default
        /// </summary>
        public string? CustomSubcategory { get; set; } 

        public string? PhoneNumber { get; set; }
        public string? BirthDate { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
    }
}
