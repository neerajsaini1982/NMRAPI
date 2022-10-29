// <copyright file="NMRAPIsContext.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.Infrastructure.Context
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using NMRAPIs.Core.Common;
    using NMRAPIs.Infrastructure.Data.Errors;
    using NMRAPIs.Infrastructure.EntityClass;

    /// <summary>
    /// App DB Context.
    /// </summary>
    public class NMRAPIsContext : DbContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="NMRAPIsContext"/> class.
        /// </summary>
        /// <param name="options">options.</param>
        public NMRAPIsContext(DbContextOptions<NMRAPIsContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NMRAPIsContext"/> class.
        /// </summary>
        /// <param name="options">options.</param>
        /// <param name="httpContextAccessor">httpContextAccessor.</param>
        public NMRAPIsContext(DbContextOptions<NMRAPIsContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets or sets code error logs.
        /// </summary>
        public virtual DbSet<CodeErrorLogs> CodeErrorLogs { get; set; }

        /// <summary>
        /// Gets or sets Profiles.
        /// </summary>
        public virtual DbSet<Profile> Profiles { get; set; }

        /// <summary>
        /// Gets or sets Addresses.
        /// </summary>
        public virtual DbSet<Address> Addresses { get; set; }

        /// <summary>
        /// OnModelCreating Override.
        /// </summary>
        /// <param name="modelBuilder">Model Builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profile>().HasQueryFilter(x => !x.IsDeleted);

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Overridden SaveChanges Method to have the Audio Objects Auto-Filled.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">acceptAllChangesOnSuccess.</param>
        /// <returns>Int.</returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// Overridden SaveChanges Method to have the Audio Objects Auto-Filled.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">acceptAllChangesOnSuccess.</param>
        /// <param name="cancellationToken">cancellationToken.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// Fill the Audit Properties.
        /// </summary>
        private void OnBeforeSaving()
        {
            var currentUsername = !string.IsNullOrEmpty(this.httpContextAccessor?.HttpContext?.User?.FindFirst("sub")?.Value)
                ? this.httpContextAccessor.HttpContext.User.FindFirst("sub").Value
                : null;

            int.TryParse(currentUsername, out int currentLoginId);

            // This is to prevent overriding the value with 0 when no login was present.
            if (currentLoginId != 0)
            {
                var entries = this.ChangeTracker.Entries();
                foreach (var entry in entries)
                {
                    if (entry.Entity is ITrackable trackable)
                    {
                        var now = DateTime.UtcNow;
                        var user = currentUsername;
                        switch (entry.State)
                        {
                            case EntityState.Modified:
                                trackable.LastUpdatedAt = now;
                                trackable.LastUpdatedBy = currentLoginId;
                                break;

                            case EntityState.Added:
                                trackable.CreatedAt = now;
                                trackable.CreatedBy = currentLoginId;
                                trackable.LastUpdatedAt = now;
                                trackable.LastUpdatedBy = currentLoginId;
                                break;
                        }
                    }
                }
            }
        }
    }
}
