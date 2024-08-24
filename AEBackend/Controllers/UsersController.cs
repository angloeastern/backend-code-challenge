using Microsoft.AspNetCore.Mvc;
using AEBackend.DTOs;

namespace AEBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{


  public UsersController()
  {

  }

  [HttpGet(Name = "Users")]
  public UserDTO[] Get()
  {
    return [new UserDTO() { ID = 1 }, new UserDTO() { ID = 2 }];
  }
}
