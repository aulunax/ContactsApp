namespace ContactsApp.Server.Models
{
    /// <summary>
    /// Represents a category in the application.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Primary key of the category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of subcategories associated with this category.
        /// </summary>
        public List<Subcategory> Subcategories { get; set; }
    }
}
