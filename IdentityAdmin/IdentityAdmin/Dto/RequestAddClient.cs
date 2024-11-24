namespace IdentityAdmin.Dto
{
    public class RequestAddClient
    {
        public string Client_Id { get; set; }
        public string Secret { get; set; }
        public int Expire { get; set; }
        public int TokenExpireType { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public ICollection<int> UserInfo { get; set; }

    }
}
