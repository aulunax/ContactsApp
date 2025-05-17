using ContactsApp.Server.Data;
using ContactsApp.Server.DTOs;

namespace ContactsApp.Server.Services
{
    /// <summary>
    /// Interface for the user service.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>
        /// List of UserDto containing user information.
        /// </returns>
        Task<List<UserDto>> GetAllUsersAsync();

        /// <summary>
        /// Checks if a user with given ID exists.
        /// </summary>
        /// <param name="id"> User ID to check.</param>
        /// <returns> True if user exists, false otherwise.</returns>
        Task<bool> UserExistsAsync(int id);
    }


    /// <summary>
    /// Implementation of the user service.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(user => new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
            }).ToList();
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            return await _userRepository.UserExistsByIdAsync(id);
        }
    }
}
