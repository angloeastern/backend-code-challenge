using AEBackend.Controllers;
using AEBackend.Controllers.Utils;
using AEBackend.DomainModels;
using AEBackend.DTOs;
using AEBackend.Repositories.RepositoryUsingEF;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]

public class ShipsController : ApplicationController
{
  private ShipRepositoryUsingEF _shipRepository;
  public ShipsController(ShipRepositoryUsingEF shipRepository)
  {
    _shipRepository = shipRepository;
  }
  [HttpGet]
  [SwaggerOperation("See all ships in the system")]
  public Task<ApiResult<List<User>>> Get()
  {
    throw new Exception();
  }

  [HttpPost]
  [SwaggerOperation("Add a ship to the system")]
  public async Task<ApiResult<Ship>> Add([FromBody] CreateShipRequest createShipRequest)
  {
    try
    {
      if (ModelState.IsValid)
      {
        bool isShipNameExists = await _shipRepository.IsShipNameExists(createShipRequest.Name.Trim());
        if (isShipNameExists)
        {
          return ApiResult.Failure<Ship>(new ApiError("Ship Name already taken"));
        }

        Ship newShip = new()
        {
          Lat = createShipRequest.Lat,
          Longi = createShipRequest.Long,
          Name = createShipRequest.Name,
          Velocity = new Knot(createShipRequest.KnotVelocity)
        };
        await _shipRepository.CreateShip(newShip);

        return ApiResult.Success(newShip);
      }
      else
      {
        return ApiResult.Failure<Ship>(string.Join(", ", GetModelStateErrors()));
      }
    }
    catch (System.Exception ex)
    {
      return ApiResult.Failure<Ship>(new ApiError(ex.ToString()));
    }
  }

  [HttpPut("{0}/Velocity")]
  [SwaggerOperation("Update velocity of a ship")]
  public Task<ApiResult<List<User>>> UpdateVelocity()
  {
    throw new Exception();
  }
  [HttpGet("Unassigneds")]
  [SwaggerOperation("See ships unassigned in the system")]
  public Task<ApiResult<List<User>>> GetUnassigneds()
  {
    throw new Exception();
  }

  [HttpGet("NearestPort")]
  [SwaggerOperation("See the nearest port to a ship with estimated arrival time to the port together with relevant details")]
  public Task<ApiResult<NearestPortInfo>> GetNearest()
  {
    throw new Exception();
  }

}