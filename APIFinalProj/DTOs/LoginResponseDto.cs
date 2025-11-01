namespace APIFinalProj.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public UserInfoDto User { get; set; }
    }
}
