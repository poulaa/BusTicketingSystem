using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using bus_system4.Areas.Identity.Data;
using bus_system4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bus_system3.Areas.Identity.Data
{
    public class bus_context : IdentityDbContext<bus_user>
    {
        public bus_context(DbContextOptions<bus_context> options)
            : base(options)
        {
        }
        public DbSet<reservation> reservation { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<bus_user>
        {
            public void Configure(EntityTypeBuilder<bus_user> builder)
            {
                builder.Property(u => u.first_name).HasMaxLength(255);
                builder.Property(u => u.last_name).HasMaxLength(255);

            }
        }
    }
}

