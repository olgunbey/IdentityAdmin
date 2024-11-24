namespace IdentityAdmin.Entity
{
    public class Client
    {
        public int Id { get; set; }
        public string Client_Id { get; set; }
        public string Secret { get; set; }
        public int TokenExpireType { get; set; } //absolute, sliding
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public ICollection<ClientUserInfo> ClientUserInfos { get; set; }
        //public int AbsoluteExpirationTimeInMinutes { get; set; } //bugün bunlar duzeltilcek
        //public int SlidingExpirationTimeInMinutes { get; set; } //bugün bunlar düzeltilcek
        public ICollection<ClientRole> ClientRoles { get; set; }
    }
}
