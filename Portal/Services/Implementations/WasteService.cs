using Portal.Models;
using Portal.Resources;
using Portal.Services.Interfaces;

namespace Portal.Services.Implementations
{
    public class WasteService : IWasteService
    {
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
    }
}
