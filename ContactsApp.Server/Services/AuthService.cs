using ContactsApp.Server.DTOs;
using ContactsApp.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace ContactsApp.Server.Services
{

    public interface IAuthService
    {
        
    }

    public class AuthService : IAuthService
    {
        private readonly PasswordHasher<User> _passwordHasher = new();


    }
}
