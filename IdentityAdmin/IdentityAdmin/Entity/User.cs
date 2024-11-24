namespace IdentityAdmin.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Password { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<UserPermission> UserPermissions { get; set; }
    }
}
