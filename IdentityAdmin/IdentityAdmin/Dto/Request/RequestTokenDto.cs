namespace IdentityAdmin.Dto.Request
{
    public class RequestTokenDto
    {
        public string Client_Id { get; set; }
        public string Secret { get; set; }
        public string Grant_Type { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? RefreshToken { get; set; }
    }
}
