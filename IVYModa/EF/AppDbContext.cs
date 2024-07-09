using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IVYModa.EF
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public override DbSet<AppUser> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName!.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.Property(u => u.Id).HasMaxLength(50);
                entity.Property(u => u.FirstName).HasMaxLength(50);
                entity.Property(u => u.LastName).HasMaxLength(50);
                entity.Property(u => u.PasswordHash).HasMaxLength(250);
                entity.Property(u => u.SecurityStamp).HasMaxLength(250);
                entity.Property(u => u.ConcurrencyStamp).HasMaxLength(250);
                entity.Property(u => u.PhoneNumber).HasMaxLength(250);
                entity.Property(u => u.BirthDay).HasMaxLength(50);
                entity.Property(u => u.Gender).HasMaxLength(10);
                entity.Property(u => u.UserName).IsRequired(false);
                entity.Property(u => u.PhoneNumber).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.HasIndex(u => new { u.Email, u.PhoneNumber })
                    .HasDatabaseName("Index_Email_PhoneNumber")
                    .IsUnique();
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasOne(l => l.IdUserNav).WithMany(u => u.Locations)
                    .HasForeignKey(l => l.IdUser)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LoginLog>(entity =>
            {
                entity.HasOne(ll => ll.IdUserNav).WithMany(u => u.LoginLogs)
                    .HasForeignKey(ll => ll.IdUser)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
