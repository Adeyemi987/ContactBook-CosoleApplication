using ContactBook.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.Data
{
    public class ContactBookContext : IdentityDbContext<AppUser>
    {
        public ContactBookContext(DbContextOptions<ContactBookContext> options) : base (options)
        {

        }

        public DbSet<AppUser> AppUsers { get; internal set; }
    }
}
