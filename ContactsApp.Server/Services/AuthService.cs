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
        public Task<string?> LoginAsync(LoginDto dto);
        public Task<IdentityResult> RegisterAsync(RegisterDto dto);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;


        public AuthService(IUserRepository userRepository, UserManager<User> userManager, IConfiguration config)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _config = config;
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
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

            return await _userManager.CreateAsync(user, dto.Password);
        }




    }
}
