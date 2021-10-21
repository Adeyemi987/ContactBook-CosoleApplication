using Microsoft.AspNetCore.Http;

namespace ContactBook.Data.DTO
{
    public class AddImageDTO
    {
        public IFormFile Image { get; set; }
    }
}
