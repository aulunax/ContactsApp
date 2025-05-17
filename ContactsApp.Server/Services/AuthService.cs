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

    /// <summary>
    /// Interface for the authentication service.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Logs in a user and returns a JWT token.
        /// </summary>
        /// <param name="dto"> Login DTO containing username and password.</param>
        /// <returns> JWT token if login is successful, null otherwise.</returns>
        public Task<string?> LoginAsync(LoginDto dto);

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="dto"> Register DTO containing username, email, and password.</param>
        /// <returns> IdentityResult indicating the result of the registration and possible errors.</returns>
        public Task<IdentityResult> RegisterAsync(RegisterDto dto);
    }


    /// <summary>
    /// Implementation of the authentication service using Identity.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepository, UserManager<User> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);

            // Check if user exists and password is correct
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return null;

            // Create JWT Token claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            // Set up token info
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the token
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            var user = new User
            {
                UserName = dto.Username,
                Email = dto.Email,
            };

            // Let Identity handle creating the User
            return await _userManager.CreateAsync(user, dto.Password);
        }




    }
}
