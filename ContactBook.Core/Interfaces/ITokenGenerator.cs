using ContactBook.Model;
using System.Threading.Tasks;

namespace ContactBook.Core.Interfaces
{
    public interface ITokenGenerator
    {
        Task<string> GenerateToken(AppUser user);
    }
}
