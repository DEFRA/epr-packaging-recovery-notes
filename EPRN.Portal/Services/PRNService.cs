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

        public async Task<CreateViewModel> GetCreatePageViewModel()
        {

            var tableRowViewModelReprocessor = CreateRow("Glass", 100, 2, "#");

            var tableRowViewModelExporter = CreateRow("Aluminium", 5, 1, "#");
            var tableRowViewModelExporter2 = CreateRow("Board/paper", 17, 4, "#");

            var listOfRowsReprocessor = new List<TableRowViewModel>
            {
                tableRowViewModelReprocessor
            };

            var listOfRowsExporter = new List<TableRowViewModel>
            {
                tableRowViewModelExporter,
                tableRowViewModelExporter2
            };

            var tableViewModelReprocessor = CreateTable("23 Ruby Street, London, SE15 1LR", listOfRowsReprocessor);
            var tableViewModelExporter = CreateTable("123 Sesame Street, London, N12 8JJ", listOfRowsExporter);

            var listOfTables = new List<TableViewModel>
            {
                tableViewModelReprocessor,
                tableViewModelExporter
            };

            return new CreateViewModel
            {
                TableViewModels = listOfTables
            };
        }

        private TableRowViewModel CreateRow(string material, int tonnage, int noOfDrafts, string chooseLink)
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