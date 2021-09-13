using Application.Dto;
using Application.Interfaces;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ShipService : IShipService
    {
        private readonly IShipRepository _repository;
        private readonly IShipMapper _mapper;

        public ShipService(IShipRepository repository, IShipMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShipDTO>> GetShips()
        {
            return _mapper.Map(await _repository.GetShipsAsync());
        }

        public async Task<ShipDTO> GetShip(int id)
        {
            return _mapper.Map(await _repository.GetShipAsync(id));
        }

        public void CreateShip(ShipDTO ship)
        {
           _repository.CreateShip(_mapper.Map(ship));
        }

        public void UpdateVelocity(int id, ShipDTO ship)
        {
           _repository.UpdateVelocity(id, _mapper.Map(ship));
        }
    }
}
