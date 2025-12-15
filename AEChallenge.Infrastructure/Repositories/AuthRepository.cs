using AEChallenge.Application.DTOs.Response;
using AEChallenge.Application.Extensions;
using AEChallenge.Application.Interfaces;
using AEChallenge.Infrastructure.Data;
using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AEChallenge.Infrastructure.Repositories
{
    public class AuthRepository(AppDbContext context, ILogger<AuthRepository> logger) : IAuthRepository
    {
        private readonly AppDbContext _context = context;
        private readonly ILogger<AuthRepository> _logger = logger;

        public async Task<ApiResponseResult<UserLoginReadModel>> AuthenticateAsync(string username, string password)
        {
            string sql = "EXEC SP_AuthenticateUser @Username, @Password";

            var parameters = new[]
            {
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password)
            };
            try
            {
                var resultsList = await _context.Set<UserLoginReadModel>()
                                    .FromSqlRaw(sql, parameters)
                                    .ToListAsync();
                var userResult = resultsList.FirstOrDefault();
                if (userResult == null || userResult.UserId == 0)
                {
                    _logger.LogWarning("Authentication failed for user: {Username}", username);
                    return ApiResponseResult<UserLoginReadModel>.Invalid(
                        message: $"Invalid username or password."
                    );
                }

                _logger.LogInformation("User authenticated successfully: {Username}", username);
                return ApiResponseResult<UserLoginReadModel>.Success(
                    data: userResult,
                    message: $"Authentication success."
                );

            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error during authentication for user: {Username}", username);
                return ApiResponseResult<UserLoginReadModel>.Failure(
                    message: $"Database error during operation: {ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during authentication for user: {Username}", username);
                return ApiResponseResult<UserLoginReadModel>.Failure(
                    message: $"An unexpected system error occurred during operation.{ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
        }
    }
}