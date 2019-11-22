﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace chat.backend.Data.IdentityUserAsp
{
    public class ApplicationDbContex : IdentityDbContext<IdentUser>
    {
        public ApplicationDbContex(DbContextOptions options)
            : base(options)
        {
        }
    }
}
