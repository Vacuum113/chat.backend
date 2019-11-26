using Microsoft.AspNetCore.Identity;

namespace chat.backend.Data.IdentityUserAsp
{
    public static class IdentityPasswordOptions
    {
        public static IdentityOptions AddIdentityOptions(this IdentityOptions o)
        { 
            o.Password.RequireDigit = true;
            o.Password.RequireLowercase = true;
            o.Password.RequireUppercase = true;
            o.Password.RequiredLength = 8;
            o.Password.RequireNonAlphanumeric = false;
            o.User.RequireUniqueEmail = true;
            return o;
        }
    }
}

