using AutoMapper;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;

namespace EPRN.Portal.Services
{
    public class PRNService : IPRNService
    {
        private IMapper _mapper;
        private IHttpPrnsService _httpPrnsService;

        public PRNService(
            IMapper mapper,
            IHttpPrnsService httpPrnsService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _httpPrnsService = httpPrnsService ?? throw new ArgumentNullException(nameof(httpPrnsService));
        }

        public async Task<TonnesViewModel> GetTonnesViewModel(int id)
        {
            await Task.CompletedTask;

            return new TonnesViewModel
            {
                JourneyId = id,
                Tonnes = await _httpPrnsService.GetPrnTonnage(id)
            };
        }

        public async Task SaveTonnes(TonnesViewModel tonnesViewModel)
        {
            await _httpPrnsService.SaveTonnage(
                tonnesViewModel.JourneyId,
                tonnesViewModel.Tonnes.Value);
        }

        public async Task<ConfirmationViewModel> GetConfirmation(int id)
        {
            var confirmationDto = await _httpPrnsService.GetConfirmation(id);

            return _mapper.Map<ConfirmationViewModel>(confirmationDto);
        }

        public async Task<CreatePrnViewModel> CreatePrnViewModel()
        {
            // TODO: This needs to get the data from a future service

            var tableRowReprocessor = CreateRow(1, "Glass", 0, 2);

            var tableRowExporter = CreateRow(2, "Aluminium", 0.3, 1);
            var tableRowExporter2 = CreateRow(3, "Board/paper", 17, 4);

            var listOfRowsReprocessor = new List<TableRowViewModel>
            {
                tableRowReprocessor
            };

            var listOfRowsExporter = new List<TableRowViewModel>
            {
                tableRowExporter,
                tableRowExporter2
            };

            var tableReprocessor = CreateTable("23 Ruby Street, London, SE15 1LR", listOfRowsReprocessor);
            var tableExporter = CreateTable("123 Sesame Street, London, N12 8JJ", listOfRowsExporter);

            var listOfTables = new List<TableViewModel>
            {
                tableReprocessor,
                tableExporter
            };

            return new CreatePrnViewModel
            {
                Tables = listOfTables
            };
        }

        private TableRowViewModel CreateRow(int materialId, string material, double tonnage, int noOfDrafts)
        {
            return new TableRowViewModel
            {
                MaterialId = materialId,
                MaterialName = material,
                Tonnage = tonnage,
                NoOfDrafts = noOfDrafts
            };
        }

        private TableViewModel CreateTable(string siteAddress, List<TableRowViewModel> rows)
        {
            return new TableViewModel
            {
                SiteAddress = siteAddress,
                Rows = rows
            };
        }
    }
}