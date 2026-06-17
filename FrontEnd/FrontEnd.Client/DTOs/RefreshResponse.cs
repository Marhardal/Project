namespace FrontEnd.Client.DTOs
{
    public class RefreshResponse
    {
        public string UserID { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
