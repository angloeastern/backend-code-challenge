using AEChallenge.Application.DTOs.Request;
using AEChallenge.Application.DTOs.Response;
using AEChallenge.Application.Extensions;
using AEChallenge.Application.Interfaces;
using AEChallenge.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using System.Security.Claims;

namespace AEChallenge.Infrastructure.Repositories
{
    public class ShipRepository(AppDbContext context, ICurrentUserService currentUserService, ILogger<ShipRepository> logger) : IShipRepository
    {
        private readonly AppDbContext _context = context;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly ILogger<ShipRepository> _logger = logger;

        public async Task<ApiResponseResult<ShipReadModel>> AddShipsync(ShipCreateDto shipDto)
        {
            string createdBy = _currentUserService.UserName ?? "System";
            var parameters = new[]
            {
                new SqlParameter("@ShipCode", shipDto.ShipCode),
                new SqlParameter("@ShipName", shipDto.ShipName),
                new SqlParameter("@FiscalYear", shipDto.FiscalYear),
                new SqlParameter("@Status", shipDto.Status),
                new SqlParameter("@CreatedBy", createdBy)
            };

            string sql = @"EXEC [dbo].[SP_CreateShip] @ShipCode, @ShipName, @FiscalYear,@Status, @CreatedBy";

            try
            {
                var result = await _context.Set<ShipReadModel>()
                                           .FromSqlRaw(sql, parameters)
                                           .AsNoTracking()
                                           .ToListAsync();

                var response = result.FirstOrDefault();

                if (response == null)
                {
                    _logger.LogWarning("Ship creation failed for {ShipCode}. No data returned.", shipDto.ShipCode);
                    return ApiResponseResult<ShipReadModel>.Failure(
                        message: "Ship creation failed.",
                        status: "CreationFailed"
                    );
                }
                _logger.LogInformation("Ship created successfully: {ShipCode}", shipDto.ShipCode);
                return ApiResponseResult<ShipReadModel>.Success(
                    data: response
                );
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error creating ship {ShipCode}", shipDto.ShipCode);
                return ApiResponseResult<ShipReadModel>.Failure(
                    message: $"Database error during operation: {ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating ship {ShipCode}", shipDto.ShipCode);
                return ApiResponseResult<ShipReadModel>.Failure(
                    message: $"An unexpected system error occurred during operation.{ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
        }

        public async Task<ApiResponseResult<ShipAssignmentReadModel>> AssignShipToUserAsync(ShipAssignmentDto shipDto)
        {
            string createdBy = _currentUserService.UserName ?? "System";
            var parameters = new[]
            {
                new SqlParameter("@UserId", shipDto.UserId),
                new SqlParameter("@ShipCode", shipDto.ShipCode),
                new SqlParameter("@CreatedBy", createdBy)
            };
            string sql = @"EXEC [dbo].[SP_AssignShipToUser] @UserId, @ShipCode, @CreatedBy";
            try
            {
                var result = await _context.Set<ShipAssignmentReadModel>()
                                           .FromSqlRaw(sql, parameters)
                                           .AsNoTracking()
                                           .ToListAsync();

                var response = result.FirstOrDefault();

                if (response == null)
                {
                    _logger.LogWarning("Ship assignment failed for Ship {ShipCode}, User {UserId}.", shipDto.ShipCode, shipDto.UserId);
                    return ApiResponseResult<ShipAssignmentReadModel>.Failure(
                        message: "Ship assignment failed.",
                        status: "CreationFailed"
                    );
                }
                _logger.LogInformation("Ship {ShipCode} assigned to User {UserId} successfully.", shipDto.ShipCode, shipDto.UserId);
                return ApiResponseResult<ShipAssignmentReadModel>.Success(
                    data: response
                );
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error assigning ship {ShipCode} to user {UserId}", shipDto.ShipCode, shipDto.UserId);
                return ApiResponseResult<ShipAssignmentReadModel>.Failure(
                    message: $"Database error during operation: {ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error assigning ship {ShipCode} to user {UserId}", shipDto.ShipCode, shipDto.UserId);
                return ApiResponseResult<ShipAssignmentReadModel>.Failure(
                    message: $"An unexpected system error occurred during operation.{ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
        }

        public async Task<ApiResponseResult<IEnumerable<ShipAssignmentReadModel>>> GetAssignmentShipByUser(int userId)
        {
            var sql = "EXEC [SP_GetShipsAssingnmentByUser] @UserId";

            var sqlParameters = new[]
            {

            new SqlParameter("@UserId",userId)
            };
            try
            {
                var list = await _context.Set<ShipAssignmentReadModel>()
                                     .FromSqlRaw(sql, sqlParameters)
                                     .AsNoTracking()
                                     .ToListAsync();
                return ApiResponseResult<IEnumerable<ShipAssignmentReadModel>>.Success(
                   data: list
               );
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error retrieving assignments for user {UserId}", userId);
                return ApiResponseResult<IEnumerable<ShipAssignmentReadModel>>.Failure(
                    message: $"Database error during operation: {ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving assignments for user {UserId}", userId);
                return ApiResponseResult<IEnumerable<ShipAssignmentReadModel>>.Failure(
                    message: $"An unexpected system error occurred during operation.{ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
        }

        public async Task<ApiResponseResult<IEnumerable<ShipReadModel>>> GetShipAsync(ShipListParameters parameters)
        {
            var sql = "EXEC SP_GetShipList @Status, @PageSize, @PageNumber, @SortColumn, @SortDirection, @SearchTerm";

            var sqlParameters = new[]
            {
            // 1. @Status (NVARCHAR) - Harus di Posisi 1
            new SqlParameter("@Status", parameters.Status ?? (object)DBNull.Value),
        

            new SqlParameter("@PageSize", parameters.PageSize),
        

            new SqlParameter("@PageNumber", parameters.PageNumber),
        

            new SqlParameter("@SortColumn", parameters.SortColumn),
        

            new SqlParameter("@SortDirection", parameters.SortDirection),
        

            new SqlParameter("@SearchTerm", parameters.SearchTerm ?? (object)DBNull.Value)
            };
            try
            {
                var list = await _context.Set<ShipReadModel>()
                                     .FromSqlRaw(sql, sqlParameters)
                                     .AsNoTracking()
                                     .ToListAsync();
                return ApiResponseResult<IEnumerable<ShipReadModel>>.Success(
                   data: list
               );
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error retrieving ship list.");
                return ApiResponseResult<IEnumerable<ShipReadModel>>.Failure(
                    message: $"Database error during operation: {ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving ship list.");
                return ApiResponseResult<IEnumerable<ShipReadModel>>.Failure(
                    message: $"An unexpected system error occurred during operation.{ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
        }

    }
}

