using ContactBook.Data.DTO;
using System.Threading.Tasks;

namespace ContactBook.Core.Interfaces
{
    public interface IAuthentication
    {
        Task<UserResponseDTO> Login(UserLoginDTO userLogin);
        Task<UserResponseDTO> Register(RegistrationRequestDTO registrationRequest);
    }
}
