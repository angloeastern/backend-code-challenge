namespace AEChallenge.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }      // Mendapatkan ID pengguna dari JWT Claim
        string? UserName { get; }    // Mendapatkan Username dari JWT Claim
        string? UserRole { get; }
    }
}
