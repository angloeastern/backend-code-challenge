using Application.Dto;
using Application.Interfaces;
using Domain.Models;
using Infrustructure.Integration;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShipTrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly IShipService _shipService;
        private readonly ISeaRoutesClient _client;

        public TrackingController(IShipService shipService, ISeaRoutesClient client)
        {
            _shipService = shipService;
            _client = client;
        }

        /// <summary>
        /// Get all ships, or filter
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShipDTO>>> GetShips()
        {
            return  Ok(await _shipService.GetShips());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShipDTO>> GetShip(int id)
        {
            return await _shipService.GetShip(id);
        }

        /// <summary>
        /// Create new ship
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateShip([FromBody] ShipDTO ship)
        {
            _shipService.CreateShip(ship);
            return Ok();
        }


        /// <summary>
        /// Update ship velocity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ship"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult UpdateVelocity(int id, [FromBody] ShipDTO ship)
        {
            if (ship != null)
            {
                _shipService.UpdateVelocity(id, ship);
                return Ok();
            }
            else
            {
                return BadRequest("Ship is null.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetClosestPort(int shipId)
        {
            var ship = await _shipService.GetShip(shipId);

            var closestPort = await _client.GetClosestPortAsync(new SeaRoutesRequest()
            {
                StartCoordLat = ship.Lat,
                StartCoordLon = ship.Lon,
                Velocity = ship.Velocity,
                EndCoordLat = "",
                EndCoordLon = ""
            });

            return Ok(closestPort);
        }
    }
}
