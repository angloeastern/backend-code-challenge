using System.Collections.Generic;
using Application.Dto;
using Application.Interfaces;
using Application.Mappers;
using Domain.Interfaces;

namespace Application.Services
{
    public class PortService : IPortService
    {
        private readonly IPortRepository _repository;
        private readonly IPortMapper _mapper;

        public PortService(IPortRepository repository, IPortMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<PortDTO> GetPorts()
        {
            return _mapper.Map(_repository.GetPorts());
        }
    }
}
