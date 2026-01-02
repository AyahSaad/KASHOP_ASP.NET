using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<Product> Products { get; set; }
        // Note: new table for translation due to normalization in database cuz its multyvalue
        public DbSet<ProductTranslation> ProductTranslations { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor=httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles"); 
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins"); 
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            builder.Entity<Category>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var auditEntries = ChangeTracker.Entries<BaseModel>();
            if (_httpContextAccessor.HttpContext != null)
            {
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                foreach (var auditEntry in auditEntries)
                {
                    if (auditEntry.State == EntityState.Added)
                    {
                        auditEntry.Property(x => x.CreatedBy).CurrentValue= currentUserId;
                        auditEntry.Property(x => x.CreatedAt).CurrentValue= DateTime.UtcNow;
                    }
                    if (auditEntry.State == EntityState.Modified)
                    {
                        auditEntry.Property(x => x.UpdatedBy).CurrentValue= currentUserId;
                        auditEntry.Property(x => x.UpdatedAt).CurrentValue= DateTime.UtcNow;
                    }

                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }


        public override int SaveChanges()
        {
            var auditEntries = ChangeTracker.Entries<BaseModel>();
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            foreach (var auditEntry in auditEntries)
            {
                if (auditEntry.State == EntityState.Added) {
                    auditEntry.Property(x => x.CreatedBy).CurrentValue= currentUserId;
                    auditEntry.Property(x => x.CreatedAt).CurrentValue= DateTime.UtcNow;
                }
                if (auditEntry.State == EntityState.Modified) {
                    auditEntry.Property(x => x.UpdatedBy).CurrentValue= currentUserId;
                    auditEntry.Property(x => x.UpdatedAt).CurrentValue= DateTime.UtcNow;
                }

            }

            return base.SaveChanges();
        }
    }
}
