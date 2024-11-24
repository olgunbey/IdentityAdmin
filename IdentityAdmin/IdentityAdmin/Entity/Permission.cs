namespace IdentityAdmin.Entity
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<UserPermission> UserPermissions { get; set; }
    }
}
