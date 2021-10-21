using ContactBook.Core.Interfaces;
using ContactBook.Data.DTO;
using ContactBook.Data.DTO.Mappings;
using ContactBook.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.Core.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IdentityResult> CreateAsync(string firstName, string lastName, string email, string userName, string phoneNumber)
        {
            AppUser user = new AppUser();
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;
            user.UserName = userName;
            user.PhoneNumber = phoneNumber;

            var result = await _userManager.CreateAsync(user, "Dorcasdo@14");

            return result;
        }

        public async Task<bool> DeleteUser(string userId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return true;
                }

                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description + Environment.NewLine;
                }

                throw new MissingMemberException(errors);
            }
            throw new ArgumentException("Resource not found");
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAllUsers(Pagination pagingParameter)
        {
            var users = await _userManager.Users.ToListAsync();
            var allUsers = new List<UserResponseDTO>();
            if (users != null)
            {
                foreach (var item in users)
                {
                    allUsers.Add(UserMappings.GetUserResponse(item));
                }
                return allUsers.Skip((pagingParameter.PageNumber - 1) * pagingParameter.PageSize)
                    .Take(pagingParameter.PageSize)
                    .ToList();
            }

            throw new ArgumentNullException("No user exists");
        }

        public async Task<UserResponseDTO> GetUser(string userId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return UserMappings.GetUserResponse(user);
            }

            throw new ArgumentException("Resource not found");
        }

        public async Task<UserResponseDTO> GetUserByEmail(string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return UserMappings.GetUserResponse(user);
            }

            throw new ArgumentException("User not found");
        }

        public async Task<IEnumerable<UserResponseDTO>> Search(Pagination pagingParameter, string searchWord = "")
        {
            

            if (!string.IsNullOrEmpty(searchWord))
            {
                searchWord = searchWord.ToLower();
                var result = await _userManager.Users.Where(e => 
                                       e.FirstName.ToLower().Contains(searchWord) 
                                    || e.LastName.ToLower().Contains(searchWord) 
                                    || e.Email.ToLower().Contains(searchWord)).ToListAsync();


                if (result != null)
                {
                    var users = new List<UserResponseDTO>();

                    foreach (var item in result)
                    {
                        users.Add(UserMappings.GetUserResponse(item));
                    }

                    return users.Skip((pagingParameter.PageNumber - 1) * pagingParameter.PageSize)
                    .Take(pagingParameter.PageSize);
                }
                
            }

            
            return default;
        }

        public async Task<bool> Update(string userId, UpdateUserRequestDTO updateUser)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                user.FirstName = string.IsNullOrWhiteSpace(updateUser.FirstName) ? user.FirstName : updateUser.FirstName;
                user.LastName = string.IsNullOrWhiteSpace(updateUser.LastName) ? user.LastName : updateUser.LastName;
                user.PhoneNumber = string.IsNullOrWhiteSpace(updateUser.PhoneNumber) ? user.PhoneNumber : updateUser.PhoneNumber;
                user.ImageUrl = string.IsNullOrWhiteSpace(updateUser.ImageUrl) ? user.ImageUrl : updateUser.ImageUrl;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return true;
                }

                string errors = string.Empty;

                foreach (var error in result.Errors)
                {
                    errors += error.Description + Environment.NewLine;
                }

                throw new MissingMemberException(errors);
            }
            throw new ArgumentException("Resource not found");
        }

        public async Task<bool> UploadImage(string userId, string url)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                user.ImageUrl = url;


                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return true;
                }

                string errors = string.Empty;

                foreach (var error in result.Errors)
                {
                    errors += error.Description + Environment.NewLine;
                }

                throw new MissingMemberException(errors);
            }

            throw new ArgumentException("Resource not found");
        }
    }
}
