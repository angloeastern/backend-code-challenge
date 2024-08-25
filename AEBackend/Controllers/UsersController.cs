using Microsoft.AspNetCore.Mvc;
using AEBackend.DTOs;
using Asp.Versioning;

namespace AEBackend.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
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
