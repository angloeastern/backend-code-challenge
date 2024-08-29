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
  private ILogger<ShipsController> _logger;
  public ShipsController(ShipRepositoryUsingEF shipRepository, ILogger<ShipsController> logger)
  {
    _shipRepository = shipRepository;
    _logger = logger;
  }
  [HttpGet]
  [SwaggerOperation("See all ships in the system")]
  public async Task<ApiResult<List<Ship>>> Get()
  {
    try
    {
      var allShips = await _shipRepository.GetAllShips();

      return ApiResult.Success(allShips);
    }
    catch (System.Exception ex)
    {
      return ApiResult.Failure<List<Ship>>(new ApiError(ex.ToString()));
    }
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
          Id = Guid.NewGuid().ToString(),
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

  [HttpPut("{id}/Velocity")]
  [SwaggerOperation("Update velocity of a ship")]
  public async Task<ApiResult<Ship>> UpdateVelocity([FromRoute] string id, [FromBody] UpdateShipVelocityRequest updateShipVelocityRequest)
  {
    try
    {
      _logger.LogDebug("%%%%% Id: " + updateShipVelocityRequest.KnotVelocity);

      if (ModelState.IsValid)
      {
        Ship updatedShip = await _shipRepository.UpdateShipVelocity(id, updateShipVelocityRequest.KnotVelocity);
        if (updatedShip == null)
        {
          return ApiResult.Failure<Ship>(new ApiError("Ship not found"));
        }

        return ApiResult.Success(updatedShip);
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
  [HttpGet("Unassigneds")]
  [SwaggerOperation("See ships unassigned in the system")]
  public async Task<ApiResult<List<Ship>>> GetUnassigneds()
  {
    try
    {
      List<Ship> unassigneds = await _shipRepository.GetUnassigneds();

      return ApiResult.Success(unassigneds);
    }
    catch (System.Exception ex)
    {
      return ApiResult.Failure<List<Ship>>(new ApiError(ex.ToString()));
    }
  }

  [HttpGet("{id}/NearestPort")]
  [SwaggerOperation("See the nearest port to a ship with estimated arrival time to the port together with relevant details")]
  public async Task<ApiResult<NearestPortInfo>> GetNearest([FromRoute] string id)
  {
    try
    {
      if (ModelState.IsValid)
      {
        var ship = await _shipRepository.GetShipById(id);
        if (ship == null)
        {
          return ApiResult.Failure<NearestPortInfo>(new ApiError("Ship not found"));
        }
        var closesPort = await _shipRepository.GetNearestPort(id);
        TimeSpan estimatedArrivalTime = TimeSpan.Zero;
        NearestPortInfo nearestPortInfo = new()
        {
          EstimatedArrivalTime = TimeSpan.Zero,
          Port = null
        };
        if (closesPort != null)
          nearestPortInfo = new()
          {
            Port = closesPort,
            EstimatedArrivalTime = ship.EstimatedArrivalTimeTo(closesPort)
          };

        return ApiResult.Success(nearestPortInfo);
      }
      else
      {
        return ApiResult.Failure<NearestPortInfo>(string.Join(", ", GetModelStateErrors()));
      }
    }
    catch (System.Exception ex)
    {
      return ApiResult.Failure<NearestPortInfo>(new ApiError(ex.ToString()));
    }

  }

}