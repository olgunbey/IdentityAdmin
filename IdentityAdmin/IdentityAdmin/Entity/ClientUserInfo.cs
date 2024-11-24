namespace IdentityAdmin.Entity
{
    public class ClientUserInfo
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public int UserInfoId { get; set; }

        public Client Client { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
