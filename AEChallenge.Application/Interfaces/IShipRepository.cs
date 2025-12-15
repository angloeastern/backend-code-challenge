using AEChallenge.Application.DTOs.Request;
using AEChallenge.Application.DTOs.Response;

namespace AEChallenge.Application.Interfaces
{
    public interface IShipRepository
    {
        Task<ApiResponseResult<ShipReadModel>> AddShipsync(ShipCreateDto shipDto);
        Task<ApiResponseResult<IEnumerable<ShipReadModel>>> GetShipAsync(ShipListParameters parameters);
        Task<ApiResponseResult<ShipAssignmentReadModel>> AssignShipToUserAsync(ShipAssignmentDto shipDto);
        Task<ApiResponseResult<IEnumerable<ShipAssignmentReadModel>>> GetAssignmentShipByUser(int userId);
    }
}