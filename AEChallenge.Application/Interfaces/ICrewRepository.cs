using AEChallenge.Application.DTOs.Request;
using AEChallenge.Application.DTOs.Response;

namespace AEChallenge.Application.Interfaces
{
    public interface ICrewRepository
    {
        Task<ApiResponseResult<IEnumerable<CrewListReadModel>>> GetCrewListAsync(
            CrewListParameters parameters);
    }
}