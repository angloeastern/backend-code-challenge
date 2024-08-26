using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using AEBackend.Repositories;
using Microsoft.AspNetCore.RateLimiting;
using AEBackend.DomainModels;
using AEBackend.DTOs;
using Microsoft.AspNetCore.Identity;

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
  [HttpGet(Name = "Users")]
  public Task<List<User>> Get()
  {
    return _userRepository.GetAllUsers();
  }

  [EnableRateLimiting("fixed")]
  [HttpPost(Name = "Register")]
  public async Task<IActionResult> Create([FromBody] CreateUserRequest createUserRequest)
  {
    if (ModelState.IsValid)
    {
      var existingUser = await _userManager.FindByEmailAsync(createUserRequest.Email);
      if (existingUser != null)
      {
        ModelState.AddModelError("Email", "Email already taken");
        return BadRequest(ModelState);
      }

      if (!AppRoles.IsRoleValid(createUserRequest.Role))
      {
        ModelState.AddModelError("Role", "Role is not valid");
        return BadRequest(ModelState);
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
        ModelState.AddModelError("", String.Join(", ", createUserResult.Errors.Select(x => x.Description)));
        return BadRequest(ModelState);
      }

      await _userManager.AddToRoleAsync(user, createUserRequest.Role);

      return Ok();
    }
    else
    {
      return BadRequest(ModelState);
    }
  }


}
