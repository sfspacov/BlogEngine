using System;
using BlogEngine.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BlogEngine.Core.Services.Abstractions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using BlogEngine.Core.Data.Entities.Common;

namespace BlogEngine.Core.Data.DatabaseContexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        private readonly ICurrentUserProvider _currentUserProvider;

        public DbSet<Post> Posts { get; set; }
        public DbSet<PostStatus> PostStatus { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostRating> PostRatings { get; set; }
        public DbSet<CustomerReview> CustomerReviews { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            ICurrentUserProvider currentUserProvider) : base(options)
        {
            _currentUserProvider = currentUserProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            FillBaseEntityFields().GetAwaiter().GetResult();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await FillBaseEntityFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private async Task FillBaseEntityFields()
        {
            var user = await _currentUserProvider.GetCurrentUserAsync();
            string userFullName = user is null ? "Anonymous" : user.FullName;

            var now = DateTime.Now;

            var entityEntries = ChangeTracker
                .Entries<BaseEntity>()
                .ToList();

            entityEntries.ForEach(e =>
            {
                var entity = e.Entity;

                entity.LastUpdateDate = now;
                entity.LastUpdateBy = userFullName;

                switch (e.State)
                {
                    case EntityState.Added:
                        entity.DateCreated = now;
                        entity.CreatedBy = userFullName;
                        break;
                    case EntityState.Modified:
                        #region DateCreated and CreatedBy value should not be changed
                        entity.DateCreated = e.GetDatabaseValues().GetValue<DateTime>(BaseEntityFields.DateCreated);
                        entity.CreatedBy = e.GetDatabaseValues().GetValue<string>(BaseEntityFields.CreatedBy);
                        #endregion
                        break;
                }
            });
        }
    }
}