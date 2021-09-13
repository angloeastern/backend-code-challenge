using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Interfaces;
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

        public async Task<List<PortDTO>> GetPortsAsync()
        {
            return _mapper.Map(await _repository.GetPortsAsync());
        }
    }
}
