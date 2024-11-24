namespace IdentityAdmin.Dto.Request
{
    public class RequestRenewAccessTokenDto
    {
        public string Client_Id { get; set; }
        public string Secret { get; set; }
        public string Grant_Type { get; set; }
        public string Refresh_Token { get; set; }
    }
}
