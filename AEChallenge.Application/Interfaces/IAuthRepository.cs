using AEChallenge.Application.DTOs.Response;

namespace AEChallenge.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<ApiResponseResult<UserLoginReadModel>> AuthenticateAsync(string username, string password);
    }
}