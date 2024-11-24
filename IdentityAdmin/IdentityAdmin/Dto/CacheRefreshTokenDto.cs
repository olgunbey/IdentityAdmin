namespace IdentityAdmin.Dto
{
    public class CacheRefreshTokenDto
    {
        public DateTime RefreshTokenExpire { get; set; }
        public string Refresh_Token { get; set; }
        public int UserID { get; set; }
    }
}
