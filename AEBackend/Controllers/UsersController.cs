using Microsoft.AspNetCore.Mvc;

namespace AEBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
  private readonly ILogger<UsersController> _logger;

  public UsersController(ILogger<UsersController> logger)
  {
    _logger = logger;
  }

  [HttpGet(Name = "Users")]
  public UserDTO[] Get()
  {
    return [new UserDTO() { ID = 1 }, new UserDTO() { ID = 2 }];
  }
}
