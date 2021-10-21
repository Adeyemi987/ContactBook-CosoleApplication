using ContactBook.Data.DTO;
using ContactBook.Model;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactBook.Core.Interfaces
{
    public interface IUserService
    {
        Task<bool> DeleteUser(string userId);
        Task<IEnumerable<UserResponseDTO>> GetAllUsers(Pagination pagingParameter);
        Task<UserResponseDTO> GetUser(string userId);
        Task<IEnumerable<UserResponseDTO>> Search(Pagination pagingParameter, string searchWord = "");
        Task<UserResponseDTO> GetUserByEmail(string email);
        Task<bool> Update(string userId, UpdateUserRequestDTO updateUser);
        Task<bool> UploadImage(string userId, string url);
        Task <IdentityResult> CreateAsync(string firstName, string lastName, string email, string userName, string phoneNumber);
    }
}
