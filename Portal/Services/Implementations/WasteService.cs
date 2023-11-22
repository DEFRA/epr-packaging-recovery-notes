using Portal.Resources;
using Portal.RESTServices.Interfaces;
using Portal.Services.Interfaces;
using Portal.ViewModels;

namespace Portal.Services.Implementations
{
    public class WasteService : IWasteService
    {
        private readonly IHttpWasteService _httpWasteService;

        public WasteService(IHttpWasteService httpWasteService)
        {
            _httpWasteService = httpWasteService ?? throw new ArgumentNullException(nameof(httpWasteService));
        }

        public DuringWhichMonthRequestViewModel GetCurrentQuarter(int journeyId)
        {
            var duringWhichMonthRequestViewModel = new DuringWhichMonthRequestViewModel
            {
                JourneyId = journeyId
            };

            int currentMonth = DateTime.Now.Month;

            switch (currentMonth)
            {
                case 1:
                case 2:
                case 3:
                    duringWhichMonthRequestViewModel.Quarter.Add(1, @WhichQuarterResources.January);
                    duringWhichMonthRequestViewModel.Quarter.Add(2, @WhichQuarterResources.February);
                    duringWhichMonthRequestViewModel.Quarter.Add(3, @WhichQuarterResources.March);
                    break;

                case 4:
                case 5:
                case 6:
                    duringWhichMonthRequestViewModel.Quarter.Add(4, @WhichQuarterResources.April);
                    duringWhichMonthRequestViewModel.Quarter.Add(5, @WhichQuarterResources.May);
                    duringWhichMonthRequestViewModel.Quarter.Add(6, @WhichQuarterResources.June);
                    break;
                case 7:
                case 8:
                case 9:
                    duringWhichMonthRequestViewModel.Quarter.Add(7, @WhichQuarterResources.July);
                    duringWhichMonthRequestViewModel.Quarter.Add(8, @WhichQuarterResources.August);
                    duringWhichMonthRequestViewModel.Quarter.Add(9, @WhichQuarterResources.September);
                    break;
                case 10:
                case 11:
                case 12:
                    duringWhichMonthRequestViewModel.Quarter.Add(10, @WhichQuarterResources.October);
                    duringWhichMonthRequestViewModel.Quarter.Add(11, @WhichQuarterResources.November);
                    duringWhichMonthRequestViewModel.Quarter.Add(12, @WhichQuarterResources.December);
                    break;
            }

            return duringWhichMonthRequestViewModel;
        }

        public async Task<WasteRecordStatusViewModel> GetWasteRecordStatus(int reprocessorId)
        {
            var vm = new WasteRecordStatusViewModel();
            
            await Task.Run(() => { 
                vm.WasteRecordStatus = EPRN.Common.Enums.WasteRecordStatuses.Complete;
                vm.WasteRecordStatusMessage = "No message yet";
            });
            return vm;
        }

        public async Task SaveSelectedMonth(int journeyId, int selectedMonth)
        {
            await _httpWasteService.SaveSelectedMonth(journeyId, selectedMonth);
        }
    }
}
