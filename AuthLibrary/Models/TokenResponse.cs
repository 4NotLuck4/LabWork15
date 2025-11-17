namespace AuthLibrary.Models
{
    public class TokenResponse
    {
        public string Token { get; set; }
        string? RefreshToken { get; set; }
    }
}
