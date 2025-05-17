namespace ContactsApp.Server.DTOs
{
    /// <summary>
    /// DTO for subcategory information.
    /// </summary>
    public class SubcategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
    }
}
