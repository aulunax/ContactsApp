using Microsoft.AspNetCore.Identity;

namespace ContactsApp.Server.Models
{
    /// <summary>
    /// Represents a user in the application. Inherits from IdentityUser.
    /// </summary>
    public class User : IdentityUser<int>
    {
        /// <summary>
        /// List of contacts associated with the user.
        /// </summary>
        public List<Contact> Contacts { get; set; }
    }
}
