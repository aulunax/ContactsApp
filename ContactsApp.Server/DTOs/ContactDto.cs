namespace ContactsApp.Server.DTOs
{
    public class ContactDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Category { get; set; }
        public string? Subcategory { get; set; }
        public string? BirthDate { get; set; }
    }
}
