using EPRN.Common.Constants;
using EPRN.Common.Dtos;
using EPRN.Common.Extensions;
using EPRN.Waste.API.Configuration;
using EPRN.Waste.API.Services.Interfaces;
using Microsoft.Extensions.Options;
namespace EPRN.Waste.API.Services
{
    public class QuarterlyDatesService : IQuarterlyDatesService
    {
        private readonly List<int> _quarterStartMonths;
        private readonly IOptions<AppConfigSettings> _configSettings;
        
        public QuarterlyDatesService(IOptions<AppConfigSettings> configSettings)
        {
            _configSettings = configSettings ??
                              throw new ArgumentNullException(nameof(configSettings));
            
            _quarterStartMonths = configSettings.Value.QuarterStartMonths?.ToList() ??
                                  throw new ArgumentNullException(nameof(configSettings.Value.QuarterStartMonths));

            if (_configSettings.Value.ReturnDeadlineForQuarter == null)
                throw new ArgumentNullException(nameof(configSettings.Value.ReturnDeadlineDays));
        }
        
        public async Task<QuarterlyDatesDto> GetQuarterMonthsToDisplay(int currentMonth, bool hasSubmittedPreviousQuarterReturn)
        {
            var monthsToDisplay = new List<int>();
            var quarterToReturn = new QuarterlyDatesDto
            {
                QuarterlyMonths = new Dictionary<int, string>(),
                Notification = "" 
            };
            
            // Overrides
            if (_configSettings.Value.CurrentMonthOverride.HasValue)
                currentMonth = _configSettings.Value.CurrentMonthOverride.Value;

            var currentDay = _configSettings.Value.CurrentDayOverride ?? DateTime.Now.Day;

            // Construct the current date time
            DateTime currentDate = new(DateTime.Now.Year, currentMonth, currentDay);
            quarterToReturn.SubmissionDate = currentDate;

            if (_configSettings.Value.HasSubmittedReturnOverride.HasValue)
                hasSubmittedPreviousQuarterReturn = _configSettings.Value.HasSubmittedReturnOverride.Value;

            // Quick check for leap year            
            if (HandleFebruary29(hasSubmittedPreviousQuarterReturn,quarterToReturn))
                return quarterToReturn;

            var currentQuarter = GetCurrentQuarter(currentDate);
            var currentMonthInQuarter = GetCurrentMonthInQuarter(currentDate);
            var isWithinCurrentQuarter = IsWithinCurrentQuarter(currentDate, currentQuarter, currentMonthInQuarter);
            var previousQuarterDeadline = GetPreviousQuarterDeadline(currentQuarter, currentDate.Year);
            var isWithinReturnDeadline = IsWithinReturnDeadline(currentDate, previousQuarterDeadline);           
            
            if (isWithinCurrentQuarter && hasSubmittedPreviousQuarterReturn)
                AddCurrentQuarterMonths(monthsToDisplay, currentQuarter, currentMonthInQuarter);
            else if (isWithinReturnDeadline)
                HandleReturnDeadline(monthsToDisplay, currentQuarter, hasSubmittedPreviousQuarterReturn, quarterToReturn, previousQuarterDeadline, currentMonthInQuarter);
            else
                HandleLateReturn(monthsToDisplay, currentQuarter, hasSubmittedPreviousQuarterReturn, quarterToReturn);
            
            // Map month numbers to their names
            foreach (var month in monthsToDisplay)
                quarterToReturn.QuarterlyMonths[month] = month.RadioButtonDateString();
            
            return quarterToReturn;
        }

        private DateTime GetPreviousQuarterDeadline(int currentQuarter, int currentYear)
        {
            var quarterDeadlineDays = _configSettings.Value.ReturnDeadlineForQuarter
               .ToDictionary(kvp => int.Parse(kvp.Key.Substring(1)), kvp => kvp.Value);

            // Calculate the previous quarter
            var previousQuarter = currentQuarter == 0 ? 3 : currentQuarter - 1;

            // Get the deadline day for the previous quarter from the config settings
            var deadlineDay = quarterDeadlineDays[previousQuarter + 1];

  
            // Calculate the deadline month for the previous quarter
            var deadlineMonth = previousQuarter == 3 ? 2 : _quarterStartMonths[currentQuarter];  //(previousQuarter + 1) % 4 + 1;

            // Return the deadline date for the previous quarter
            return new DateTime(currentYear, deadlineMonth, deadlineDay);
        }

        private static int GetCurrentQuarter(DateTime currentDate)
        {
            return (currentDate.Month - 1) / 3;
        }
        private static int GetCurrentMonthInQuarter(DateTime currentDate)
        {
            return (currentDate.Month - 1) % 3 + 1;
        }
        private bool IsWithinCurrentQuarter(DateTime currentDate, int currentQuarter, int currentMonthInQuarter)
        {
            var quarterStartDate = new DateTime(currentDate.Year, _quarterStartMonths[currentQuarter], 1);
            return currentDate <= quarterStartDate.AddMonths(currentMonthInQuarter);
        }

        private static void AddCurrentQuarterMonths(List<int> monthsToDisplay, int currentQuarter, int currentMonthInQuarter)
        {
            for (var i = 1; i <= currentMonthInQuarter; i++)
                monthsToDisplay.Add((currentQuarter * 3) + i);
        }
        private static bool IsWithinReturnDeadline(DateTime currentDate, DateTime returnDeadline)
        {
            return currentDate <= returnDeadline;
        }
        
        private  void HandleReturnDeadline(List<int> monthsToDisplay, 
            int currentQuarter, 
            bool hasSubmittedPreviousQuarterReturn, 
            QuarterlyDatesDto quarterToReturn,
            DateTime previousQuarterDeadline,
            int currentMonthInQuarter)
        {
            // If the previous quarter return has been submitted, add the first month of the current quarter to the display
            if (hasSubmittedPreviousQuarterReturn)
                monthsToDisplay.Add((currentQuarter * 3) + 1);
            else
            {
                // If the previous quarter return has not been submitted, add all the months of the previous quarter to the display
                AddPreviousQuarterMonths(monthsToDisplay, currentQuarter);
                AddCurrentQuarterMonths(monthsToDisplay, currentQuarter, currentMonthInQuarter);

                // No warning displayed in January
                if (currentQuarter == 0 && currentMonthInQuarter == 1) 
                    return;
                
                // Warning message indicating that the return date is approaching
                quarterToReturn.Notification = Strings.Notifications.QuarterlyReturnDue;
                quarterToReturn.NotificationDeadlineDate = previousQuarterDeadline;
            }
        }

        private void AddPreviousQuarterMonths(List<int> monthsToDisplay, int currentQuarter)
        {
            const int monthsInQuarter = 3;
            const int totalMonthsInYear = 12;

            // Calculate the previous quarter
            var previousQuarter = currentQuarter == 0 ? 3 : currentQuarter - 1;

            // Get the start month of the previous quarter
            var startMonthOfPreviousQuarter = _quarterStartMonths[previousQuarter];
            
            // Add the months of the previous quarter to the display
            for (var monthIndex = 0; monthIndex < monthsInQuarter; monthIndex++)
            {
                var month = (startMonthOfPreviousQuarter + monthIndex - 1) % totalMonthsInYear + 1;
                monthsToDisplay.Add(month);
            }
        }
        
        private static void HandleLateReturn(List<int> monthsToDisplay, int currentQuarter, bool hasSubmittedPreviousQuarterReturn, QuarterlyDatesDto quarterToReturn)
        {
            monthsToDisplay.Add((currentQuarter * 3) + 1);
            
            if (!hasSubmittedPreviousQuarterReturn) 
                quarterToReturn.Notification = Strings.Notifications.QuarterlyReturnLate;
        }

        private static bool HandleFebruary29(bool hasSubmittedPreviousQuarterReturn, QuarterlyDatesDto quarterToReturn)
        {
            if (!quarterToReturn.SubmissionDate.IsFeb29()) 
                return false;
            
            if (!hasSubmittedPreviousQuarterReturn)
                quarterToReturn.Notification = Strings.Notifications.QuarterlyReturnLate;                 
                
            return true;
        }
    }
}
