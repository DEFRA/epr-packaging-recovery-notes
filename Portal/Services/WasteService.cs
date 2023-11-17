using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Portal.Services.Interfaces;

namespace Portal.Services
{
    public class WasteService : IWasteService
    {
        private readonly IMapper _mapper;

        public WasteService(IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
    }
}
