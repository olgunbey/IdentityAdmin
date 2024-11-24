namespace IdentityAdmin.Entity
{
    public class UserPermission
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
        public User User { get; set; }
    }
}
