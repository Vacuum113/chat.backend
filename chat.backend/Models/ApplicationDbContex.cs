using chat.backend.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace chat.backend.Models
{
    public class ApplicationDbContex : IdentityDbContext<ChatUser>
    {
        public ApplicationDbContex(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<RefToken> RefreshTokens { get; set; }
    }
}
