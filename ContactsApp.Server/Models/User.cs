using Microsoft.AspNetCore.Identity;

namespace ContactsApp.Server.Models
{
    public class User : IdentityUser<int>
    {
        public List<Contact> Contacts { get; set; }
    }
}
