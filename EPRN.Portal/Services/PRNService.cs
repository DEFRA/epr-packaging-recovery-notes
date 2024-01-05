using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;

namespace EPRN.Portal.Services
{
    public class PRNService : IPRNService
    {
        private IHttpPrnsService _httpPrnsService;

        public PRNService(IHttpPrnsService httpPrnsService)
        {
            _httpPrnsService = httpPrnsService ?? throw new ArgumentNullException(nameof(httpPrnsService));
        }

        public async Task<TonnesViewModel> GetTonnesViewModel(int id)
        {
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

        public async Task<CreatePrnViewModel> CreatePrnViewModel()
        {
            // TODO: This needs to get the data from a future service

            var tableRowReprocessor = CreateRow("Glass", 0, 2, "#");

            var tableRowExporter = CreateRow("Aluminium", 0.3, 1, "#");
            var tableRowExporter2 = CreateRow("Board/paper", 17, 4, "#");

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
                TableViewModels = listOfTables
            };
        }

        private TableRowViewModel CreateRow(string material, double tonnage, int noOfDrafts, string chooseLink)
        {
            return new TableRowViewModel
            {
                Material = material,
                Tonnage = tonnage,
                NoOfDrafts = noOfDrafts,
                ChooseLink = chooseLink
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