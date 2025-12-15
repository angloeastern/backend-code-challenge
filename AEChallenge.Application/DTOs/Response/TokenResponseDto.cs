namespace AEChallenge.Application.DTOs.Response
{
    public class TokenResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserLoginReadModel User { get; set; } = new UserLoginReadModel();
    }
}
