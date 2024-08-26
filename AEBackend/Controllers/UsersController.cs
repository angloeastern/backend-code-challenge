using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using AEBackend.Repositories;
using Microsoft.AspNetCore.RateLimiting;
using AEBackend.DomainModels;
using AEBackend.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace AEBackend.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController : ControllerBase
{

  private IUserRepository _userRepository;
  private UserManager<User> _userManager;

  public UsersController(IUserRepository userRepository, UserManager<User> userManager)
  {
    _userRepository = userRepository;
    _userManager = userManager;
  }

  [EnableRateLimiting("fixed")]
  // [Authorize(Roles = AppRoles.Administrator)]
  [HttpGet(Name = "Users")]
  public async Task<ApiResult<List<User>>> Get()
  {
    var users = await _userRepository.GetAllUsers();
    return ApiResult.Success(users);
  }


  [EnableRateLimiting("fixed")]
  // [Authorize(Roles = AppRoles.Administrator)]
  [HttpPost(Name = "Register")]
  public async Task<ApiResult<User>> Create([FromBody] CreateUserRequest createUserRequest)
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
        SecurityStamp = Guid.NewGuid().ToString()
      };

      var createUserResult = await _userManager.CreateAsync(user, createUserRequest.Password);

      if (!createUserResult.Succeeded)
      {
        return ApiResult.Failure<User>(string.Join(", ", createUserResult.Errors.Select(x => x.Description)));
      }

      await _userManager.AddToRoleAsync(user, createUserRequest.Role);

      return ApiResult.Success(user);
    }
    else
    {
      var er = ModelState.Values.Select(x => x.Errors.ToList()).ToList();

      List<string> errorMessages = [];
      er.ForEach(x => x.ForEach(y =>
      {
        errorMessages.Add(y.ErrorMessage);
      }));
      return ApiResult.Failure<User>(string.Join(", ", errorMessages));
    }
  }


}
