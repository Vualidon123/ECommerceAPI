using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }

        public async Task AddUserAsync(User user)
        {
            // Add business logic validation
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(user.Username))
            {
                throw new ArgumentException("Username is required.");
            }

            // Check if email already exists
            var existingUserByEmail = await _userRepository.GetUserByEmail(user.Email);
            if (existingUserByEmail != null)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            // Check if username already exists
            var existingUserByUsername = await _userRepository.GetUserByEmail(user.Username);
            if (existingUserByUsername != null)
            {
                throw new InvalidOperationException("A user with this username already exists.");
            }

            await _userRepository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(user.Id);
            if (existingUser == null)
            {
                throw new InvalidOperationException($"User with ID {user.Id} not found.");
            }

            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                throw new InvalidOperationException($"User with ID {id} not found.");
            }

            await _userRepository.DeleteUser(id);
        }
    }
}
