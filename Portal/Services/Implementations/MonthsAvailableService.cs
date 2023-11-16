using Portal.Models;
using Portal.Services.Interfaces;

namespace Portal.Services.Implementations
{
    public class MonthsAvailableService : IMonthsAvailableService
    {
        public DuringWhichMonthRequestViewModel GetCurrentQuarter()
        {
            var model = new DuringWhichMonthRequestViewModel();

            int currentMonth = DateTime.Now.Month;

            switch (currentMonth)
            {
                case 1:
                case 2:
                case 3:
                    model.Months.Add("january", "January");
                    model.Months.Add("february", "February");
                    model.Months.Add("march", "March");
                    break;

                case 4:
                case 5:
                case 6:
                    model.Months.Add("april", "April");
                    model.Months.Add("may", "May");
                    model.Months.Add("june", "June");
                    break;
                case 7:
                case 8:
                case 9:
                    model.Months.Add("july", "July");
                    model.Months.Add("august", "August");
                    model.Months.Add("september", "September");
                    break;
                case 10:
                case 11:
                case 12:
                    model.Months.Add("october", "October");
                    model.Months.Add("november", "November");
                    model.Months.Add("december", "December");
                    break;
            }

            return model;
        }
    }
}
