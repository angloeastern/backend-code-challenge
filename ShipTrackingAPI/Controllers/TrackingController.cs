using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using Domain.Models;
using Infrustructure.Integration;

namespace ShipTrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly IPortService _portService;
        private readonly IShipService _shipService;
        private readonly ISeaRoutesClient _client;

        public TrackingController(IPortService portService, IShipService shipService, ISeaRoutesClient client)
        {
            _portService = portService;
            _shipService = shipService;
            _client = client;
        }

        /// <summary>
        /// Get all ships, or filter
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<ShipDTO>> GetShips()
        {
            return Ok(_shipService.GetShips());
        }

        [HttpGet("{id}")]
        public ActionResult<ShipDTO> GetShip(int id)
        {
            return _shipService.GetShip(id);
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
        public IActionResult GetClosestPort(int shipId)
        {
            var ship = _shipService.GetShip(shipId);

            var closestPort = _client.GetClosestPort(new SeaRoutesRequest()
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
