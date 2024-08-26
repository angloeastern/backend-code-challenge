using Microsoft.AspNetCore.Mvc;
using AEBackend.DTOs;
using Asp.Versioning;
using AEBackend.Repositories;
using Microsoft.AspNetCore.RateLimiting;

namespace AEBackend.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController : ControllerBase
{

  private IUserRepository _userRepository;

  public UsersController(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  [EnableRateLimiting("fixed")]
  [HttpGet(Name = "Users")]
  public Task<List<User>> Get()
  {
    return _userRepository.GetAllUsers();
  }


}
