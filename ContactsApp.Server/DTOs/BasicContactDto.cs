namespace ContactsApp.Server.DTOs
{
    /// <summary>
    /// Simplified DTO for contact information.
    /// </summary>
    public class BasicContactDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
