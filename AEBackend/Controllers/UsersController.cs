using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using AEBackend.Repositories;
using Microsoft.AspNetCore.RateLimiting;
using AEBackend.DomainModels;
using AEBackend.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace AEBackend.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController : ApplicationController
{

  private IUserRepository _userRepository;
  private UserManager<User> _userManager;
  private IConfiguration _configuration;

  public UsersController(IUserRepository userRepository, UserManager<User> userManager, IConfiguration configuration)
  {
    _userRepository = userRepository;
    _userManager = userManager;
    _configuration = configuration;
  }


  [SwaggerOperation("See all users in the system")]
  [EnableRateLimiting("fixed")]
  [Authorize(AppRoles.AdministratorRole)]
  [HttpGet]
  [Produces("application/json")]
  [ProducesResponseType(typeof(ApiResult<User>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ApiResult), StatusCodes.Status500InternalServerError)]

  public async Task<ApiResult<List<User>>> Get()
  {
    try
    {
      var users = await _userRepository.GetAllUsers();

      return ApiResult.Success(users);
    }
    catch (System.Exception ex)
    {
      return ApiResult.Failure<List<User>>(new ApiError(ex.ToString()));
    }
  }

  [HttpGet("{id}/Ships")]
  [SwaggerOperation("See ships assigned to a specific user")]
  public async Task<ApiResult<List<User>>> GetShips()
  {
    try
    {
      var users = await _userRepository.GetAllUsers();

      return ApiResult.Success(users);
    }
    catch (System.Exception ex)
    {
      return ApiResult.Failure<List<User>>(new ApiError(ex.ToString()));
    }
  }

  [HttpPut("{id}/Ships")]
  [SwaggerOperation("Update ships assigned to a user")]
  public async Task<ApiResult<List<User>>> UpdateShips()
  {
    try
    {
      var users = await _userRepository.GetAllUsers();

      return ApiResult.Success(users);
    }
    catch (System.Exception ex)
    {
      return ApiResult.Failure<List<User>>(new ApiError(ex.ToString()));
    }
  }


  private async Task<ApiResult<User>> CreateUser([FromBody] CreateUserRequest createUserRequest)
  {
    if (ModelState.IsValid)
    {
      var existingUser = await _userManager.FindByEmailAsync(createUserRequest.Email);
      if (existingUser != null)
      {
        return ApiResult.Failure<User>("Email already taken");
      }

      if (!AppRoles.IsRoleValid(createUserRequest.Role))
      {
        return ApiResult.Failure<User>("Role is not valid");
      }

      var user = new User
      {
        FirstName = createUserRequest.FirstName,
        LastName = createUserRequest.LastName,
        Email = createUserRequest.Email,
        UserName = createUserRequest.Email,
        SecurityStamp = Guid.NewGuid().ToString(),
        NormalizedUserName = createUserRequest.Email.ToUpper(),
        NormalizedEmail = createUserRequest.Email.ToUpper(),
        UserRoles = [new ApplicationUserRole() { Role = new ApplicationRole { Name = createUserRequest.Role } }]
      };

      PasswordHasher<User> ph = new();
      user.PasswordHash = ph.HashPassword(user, createUserRequest.Password);

      await _userRepository.CreateUser(user);

      return ApiResult.Success(user);
    }
    else
    {
      return ApiResult.Failure<User>(string.Join(", ", GetModelStateErrors()));
    }
  }


  [SwaggerOperation("Add a user to the system")]
  [EnableRateLimiting("fixed")]
  [Authorize(AppRoles.AdministratorRole)]
  [HttpPost]
  [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ApiResult), StatusCodes.Status500InternalServerError)]
  public async Task<ApiResult<User>> Create([FromBody] CreateUserRequest createUserRequest)
  {
    try
    {
      return await CreateUser(createUserRequest);
    }
    catch (System.Exception ex)
    {
      return ApiResult.Failure<User>(new ApiError(ex.ToString()));
    }

  }




}
