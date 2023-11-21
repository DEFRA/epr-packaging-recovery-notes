using AutoMapper;
using PRN.Common.Models;
using Waste.API.Services.Interfaces;
using WasteManagement.API.Data;

namespace Waste.API.Services
{
    internal class WasteService : IWasteService
    {
        private readonly IMapper _mapper;
        private readonly WasteContext _context;

        public WasteService(
            IMapper mapper,
            WasteContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<WasteTypeDto> GetWasteTypes()
        {
            return _context
                    .WasteType.Select(wt => _mapper.Map<WasteTypeDto>(wt));
        }
    }
}
