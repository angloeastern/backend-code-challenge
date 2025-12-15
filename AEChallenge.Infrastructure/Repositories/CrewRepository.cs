using AEChallenge.Application.DTOs.Request;
using AEChallenge.Application.DTOs.Response;
using AEChallenge.Application.Extensions;
using AEChallenge.Application.Interfaces;
using AEChallenge.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AEChallenge.Infrastructure.Repositories
{
    public class CrewRepository(AppDbContext context, ILogger<CrewRepository> logger) : ICrewRepository
    {
        private readonly AppDbContext _context = context;
        private readonly ILogger<CrewRepository> _logger = logger;

        public async Task<ApiResponseResult<IEnumerable<CrewListReadModel>>> GetCrewListAsync(
            CrewListParameters parameters)
        {
            var sql = "EXEC SP_GetCrewList @ShipCode, @PageSize, @PageNumber, @SortColumn, @SortDirection, @SearchTerm";

            var sqlParameters = new[]
            {
                new SqlParameter("@ShipCode", parameters.ShipCode),
                new SqlParameter("@PageSize", parameters.PageSize),
                new SqlParameter("@PageNumber", parameters.PageNumber),
                new SqlParameter("@SortColumn", parameters.SortColumn),
                new SqlParameter("@SortDirection", parameters.SortDirection),
                new SqlParameter("@SearchTerm", parameters.SearchTerm ?? (object)DBNull.Value)
            };
            try
            {
                var crewList = await _context.Set<CrewListReadModel>()
                                     .FromSqlRaw(sql, sqlParameters)
                                     .AsNoTracking()
                                     .ToListAsync();
                return ApiResponseResult<IEnumerable<CrewListReadModel>>.Success(
                   data: crewList
               );
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error retrieving crew list for ship {ShipCode}", parameters.ShipCode);
                return ApiResponseResult<IEnumerable<CrewListReadModel>>.Failure(
                    message: $"Database error during operation: {ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving crew list for ship {ShipCode}", parameters.ShipCode);
                return ApiResponseResult<IEnumerable<CrewListReadModel>>.Failure(
                    message: $"An unexpected system error occurred during operation.{ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
        }
    }
}