using System.Collections.Generic;
using Application.Dto;
using Application.Interfaces;
using Application.Mappers;
using Domain.Entities;
using Domain.Interfaces;

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

        public IEnumerable<ShipDTO> GetShips()
        {
            return _mapper.Map(_repository.GetShips());
        }

        public ShipDTO GetShip(int id)
        {
            return _mapper.Map(_repository.GetShip(id));
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
