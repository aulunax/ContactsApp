namespace ContactsApp.Server.Models
{
    /// <summary>
    /// Represents a subcategory in the application.
    /// </summary>
    public class Subcategory
    {
        /// <summary>
        /// Primary key of the subcategory.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the subcategory.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Foreign key to the category this subcategory belongs to.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Navigation property to the category this subcategory belongs to.
        /// </summary>
        public Category Category { get; set; }
    }
}
