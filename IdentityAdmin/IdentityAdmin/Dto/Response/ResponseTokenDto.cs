namespace IdentityAdmin.Dto.Response
{
    public class ResponseTokenDto
    {
        public int UserId { get; set; }
        public string Access_Token { get; set; }
        public DateTime AccessTokenExpire { get; set; }
        public string Refresh_Token { get; set; }
        public DateTime RefreshTokenExpire { get; set; }
    }
}
