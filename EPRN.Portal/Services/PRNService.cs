using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;

namespace EPRN.Portal.Services
{
    public class PRNService : IPRNService
    {
        public TonnesViewModel GetTonnesViewModel(int id)
        {
            return new TonnesViewModel
            {
                JourneyId = id,
            };
        }

        public Task SaveTonnes(TonnesViewModel tonnesViewModel)
        {
            throw new NotImplementedException();
        }

        public async Task<CreateViewModel> GetCreateViewModel()
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

            listOfRowsReprocessor.Add(tableRowViewModelReprocessor);

            listOfRowsExporter.Add(tableRowViewModelExporter);

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