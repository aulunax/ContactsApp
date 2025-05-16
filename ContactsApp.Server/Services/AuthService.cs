using ContactsApp.Server.Data;
using ContactsApp.Server.DTOs;
using ContactsApp.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactsApp.Server.Services
{

    public interface IAuthService
    {
        public Task<LoginResponseDto?> LoginAsync(LoginDto dto);
        public Task<LoginResponseDto?> RegisterAsync(RegisterDto dto);
    }

    public class AuthService : IAuthService
    {
        private readonly PasswordHasher<User> _passwordHasher = new();

        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.UserByEmailAsync(dto.Email);

            if (user == null) 
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            result = PasswordVerificationResult.Success;
            if (result != PasswordVerificationResult.Success) 
                return null;

            return new LoginResponseDto
            {
                Email = user.Email,
                Username = user.Username,
                Token = "Placeholder"
            };
        }

        public async Task<LoginResponseDto?> RegisterAsync(RegisterDto dto)
        {
            var user = await _userRepository.UserByEmailAsync(dto.Email);
            if (user != null)
                return null;

            var newUser = new User
            {
                Email = dto.Email,
                Username = dto.Username,
                PasswordHash = _passwordHasher.HashPassword(null, dto.Password)
            };
            // Save the new user to the database
            // await _userRepository.AddUserAsync(newUser);
            return new LoginResponseDto
            {
                Email = newUser.Email,
                Username = newUser.Username,
                Token = "Placeholder"
            };
        }




    }
}
