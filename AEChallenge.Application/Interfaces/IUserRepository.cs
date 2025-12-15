using AEChallenge.Application.DTOs.Request;
using AEChallenge.Application.DTOs.Response;
using System.Threading.Tasks;

namespace AEChallenge.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<ApiResponseResult<UserCreationReadModel>> AddUserAsync(UserCreateDto userDto);
        Task<ApiResponseResult<IEnumerable<UserCreationReadModel>>> GetUserAsync(BaseQueryParameters parameters);
    }
}