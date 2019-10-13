using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace chat.backend.Models
{
    public class ApplicationDbContex : ApiAuthorizationDbContext<ChatUser>
    {
        public ApplicationDbContex(DbContextOptions options, 
            IOptions<OperationalStoreOptions> operationalStoreOptions)
                : base(options, operationalStoreOptions)
        {
        }

        DbSet<ChatUser> ChatUsers { get; set; }
    }
}
