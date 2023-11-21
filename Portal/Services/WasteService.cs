using AutoMapper;
using EPRN.Common.Models;
using Portal.Services.Interfaces;
using Portal.ViewModels.Waste;

namespace Portal.Services
{
    public class WasteService : BaseService, IWasteService
    {
        private readonly IMapper _mapper;

        public WasteService(
            string url,
            IMapper mapper,
            IHttpClientFactory httpClientFactory) : base(url, httpClientFactory)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));   
        }

        public async Task<WasteTypeViewModel> GetWasteTypeViewModel(int journeyId)
        {
            return new WasteTypeViewModel
            {
                JourneyId = journeyId,
                WasteTypes = await Get<List<WasteTypeDto>>("Waste/Types")!
            };
        }

        public Task SaveSelectedWasteType(WasteTypeViewModel wasteTypeViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
