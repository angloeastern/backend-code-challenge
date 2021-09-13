using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Interfaces;

namespace UnitTest
{
    public class PortServiceFake : IPortService
    {
        private readonly List<PortDTO> _ports;

        public PortServiceFake()
        {
            _ports = new List<PortDTO>()
            {
                new PortDTO() {Id = 25 ,Name = "Port1",Lat ="34.123456" ,Lon ="45.123456" },
                new PortDTO() {Id = 37 ,Name = "Port2",Lat ="27.123456" ,Lon ="31.123456" },
                new PortDTO() {Id = 48 ,Name = "Port3",Lat ="12.123456" ,Lon ="29.123456" },
            };
        }

        public async Task<List<PortDTO>> GetPortsAsync()
        {
            return await Task.Run(() => _ports);
        }
    }
}
