using System;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<ChatUser>()
                .HasOne(p => p.Token)
                .WithOne(i => i.ChatUser)
                .HasForeignKey<RefToken>(b => b.ChatUserId);
        }


        public DbSet<RefToken> RefreshTokens { get; set; }
    }
}
