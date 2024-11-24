namespace IdentityAdmin.Entity
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string UserInfoName { get; set; }
        public ICollection<ClientUserInfo> ClientUserInfos { get; set; }
    }
}
