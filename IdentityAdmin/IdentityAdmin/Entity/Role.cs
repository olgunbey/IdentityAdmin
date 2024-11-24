namespace IdentityAdmin.Entity
{
    public class Role
    {
        public int Id { get; set; }
        public string Rolename { get; set; }

        public ICollection<ClientRole> ClientRoles { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
