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
            var listOfTables = new List<TableViewModel>();

            var listOfRowsReprocessor = new List<TableRowViewModel>();

            var listOfRowsExporter = new List<TableRowViewModel>();

            var tableRowViewModelReprocessor = new TableRowViewModel
            {
                Material = "Glass",
                Tonnage = 100,
                NoOfDrafts = 2,
                ChooseLink = "#"
            };

            var tableRowViewModelExporter = new TableRowViewModel
            {
                Material = "Aluminium",
                Tonnage = 5,
                NoOfDrafts = 1,
                ChooseLink = "#"
            };

            var tableRowViewModelExporter2 = new TableRowViewModel
            {
                Material = "Board/paper",
                Tonnage = 17,
                NoOfDrafts = 4,
                ChooseLink = "#"
            };

            listOfRowsReprocessor.Add(tableRowViewModelReprocessor);

            listOfRowsExporter.Add(tableRowViewModelExporter);
            listOfRowsExporter.Add(tableRowViewModelExporter2);

            var tableViewModelReprocessor = new TableViewModel
            {
                SiteAddress = "23 Ruby Street, London, SE15 1LR",
                Rows = listOfRowsReprocessor
            };

            var tableViewModelExporter = new TableViewModel
            {
                SiteAddress = "123 Sesame Street, London, N12 8JJ",
                Rows = listOfRowsExporter
            };

            listOfTables.Add(tableViewModelReprocessor);
            listOfTables.Add(tableViewModelExporter);

            var viewModel = new CreateViewModel();

            viewModel.TableViewModels = listOfTables;

            return viewModel;
        }

    }
}