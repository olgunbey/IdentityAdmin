using IdentityAdmin.Entity;
using Microsoft.EntityFrameworkCore;

namespace IdentityAdmin.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Client> Client { get; set; }
        public DbSet<ClientUserInfo> ClientUserInfo { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<ClientRole> ClientRole { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<UserPermission> UserPermission { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientUserInfo>()
                .HasOne(y => y.Client)
                .WithMany(y => y.ClientUserInfos)
                .HasForeignKey(y => y.ClientId);

            modelBuilder.Entity<ClientUserInfo>()
                .HasOne(y => y.UserInfo)
                .WithMany(y => y.ClientUserInfos)
                .HasForeignKey(y => y.UserInfoId);

            modelBuilder.Entity<ClientRole>()
                .HasOne(y => y.Role)
                .WithMany(Y => Y.ClientRoles)
                .HasForeignKey(Y => Y.RoleId);

            modelBuilder.Entity<ClientRole>()
                .HasOne(y => y.Client)
                .WithMany(y => y.ClientRoles)
                .HasForeignKey(y => y.ClientId);

            modelBuilder.Entity<UserRole>()
                .HasOne(y => y.User)
                .WithMany(y => y.UserRoles)
                .HasForeignKey(y => y.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(y => y.Role)
                .WithMany(y => y.UserRoles)
                .HasForeignKey(y => y.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(y => y.Role)
                .WithMany(y => y.RolePermissions)
                .HasForeignKey(y => y.RoleId);




            modelBuilder.Entity<UserPermission>()
                .HasOne(y => y.Permission)
                .WithMany(y => y.UserPermissions)
                .HasForeignKey(y => y.PermissionId);

            modelBuilder.Entity<UserPermission>()
                .HasOne(y => y.User)
                .WithMany(y=>y.UserPermissions)
                .HasForeignKey(y => y.UserId);
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=OLGUNBEY\\SQLEXPRESS;Database=Token;Trusted_Connection=True; TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }

    }
}
