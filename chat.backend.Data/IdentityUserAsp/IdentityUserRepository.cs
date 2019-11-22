using chat.backend.Data.Interfaces;
using System.Threading.Tasks;

namespace chat.backend.Data.IdentityUserAsp
{
    public class IdentityUserRepository : IRepository<IdentUser>
    {
        private ApplicationDbContex _applicationDbContex;

        public IdentityUserRepository(ApplicationDbContex applicationDbContex)
        {
            _applicationDbContex = applicationDbContex;
        }

        public async Task<IdentUser> FindByEmailAsync(string Id)
        {
            await 
        }
    }
}
