namespace IdentityAdmin.Entity
{
    public class ClientRole
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public Client Client { get; set; }
    }
}
