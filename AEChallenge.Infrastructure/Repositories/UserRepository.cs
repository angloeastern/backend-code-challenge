using AEChallenge.Application.DTOs.Request;
using AEChallenge.Application.DTOs.Response;
using AEChallenge.Application.Extensions;
using AEChallenge.Application.Interfaces;
using AEChallenge.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Security.Claims;

namespace AEChallenge.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context, ICurrentUserService currentUserService, ILogger<UserRepository> logger) : IUserRepository
    {
        private readonly AppDbContext _context = context;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly ILogger<UserRepository> _logger = logger;

        public async Task<ApiResponseResult<UserCreationReadModel>> AddUserAsync(UserCreateDto userDto)
        {
            string createdBy = _currentUserService.UserName ?? "System";


            var parameters = new[]
            {
        new SqlParameter("@UserName", userDto.UserName),
        new SqlParameter("@Password", userDto.Password),
        new SqlParameter("@UserRole", userDto.UserRole),
        new SqlParameter("@CreatedBy", createdBy)
    };

            string sql = @"EXEC [dbo].[SP_CreateSystemUser] @UserName, @Password, @UserRole, @CreatedBy";

            try
            {
                var result = await _context.Set<UserCreationReadModel>()
                                           .FromSqlRaw(sql, parameters)
                                           .AsNoTracking()
                                           .ToListAsync();

                var response = result.FirstOrDefault();

                if (response == null || response.UserId == 0)
                {
                    _logger.LogWarning("User creation failed for {UserName}. No data returned.", userDto.UserName);
                    return ApiResponseResult<UserCreationReadModel>.Failure(
                        message: "User creation failed. Database procedure did not return valid user data.",
                        status: "CreationFailed"
                    );
                }

                _logger.LogInformation("User created successfully: {UserName}", userDto.UserName);
                return ApiResponseResult<UserCreationReadModel>.Success(
                    data: response
                );
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error creating user {UserName}", userDto.UserName);
                return ApiResponseResult<UserCreationReadModel>.Failure(
                    message: $"Database error during operation: {ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating user {UserName}", userDto.UserName);
                return ApiResponseResult<UserCreationReadModel>.Failure(
                    message: $"An unexpected system error occurred during operation.{ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
        }

        public async Task<ApiResponseResult<IEnumerable<UserCreationReadModel>>> GetUserAsync(BaseQueryParameters parameters)
        {
            var sql = "EXEC SP_GetUsersList @PageSize, @PageNumber, @SortColumn, @SortDirection, @SearchTerm";

            var sqlParameters = new[]
            {
                new SqlParameter("@PageSize", parameters.PageSize),
                new SqlParameter("@PageNumber", parameters.PageNumber),
                new SqlParameter("@SortColumn", parameters.SortColumn),
                new SqlParameter("@SortDirection", parameters.SortDirection),
                new SqlParameter("@SearchTerm", parameters.SearchTerm ?? (object)DBNull.Value)
            };
            try
            {
                var list = await _context.Set<UserCreationReadModel>()
                                     .FromSqlRaw(sql, sqlParameters)
                                     .AsNoTracking()
                                     .ToListAsync();
                return ApiResponseResult<IEnumerable<UserCreationReadModel>>.Success(
                   data: list
               );
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error retrieving user list.");
                return ApiResponseResult<IEnumerable<UserCreationReadModel>>.Failure(
                    message: $"Database error during operation: {ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving user list.");
                return ApiResponseResult<IEnumerable<UserCreationReadModel>>.Failure(
                    message: $"An unexpected system error occurred during operation.{ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
        }
    }
}

