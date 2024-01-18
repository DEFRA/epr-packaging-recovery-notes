﻿using System;
using System.Globalization;
using EPRN.Waste.API.Configuration;
using EPRN.Waste.API.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace EPRN.Waste.API.Services
{
    public class QuarterlyDatesService : IQuarterlyDatesService
    {
        private readonly List<int> _quarterStartMonths; 
        private readonly List<int> _returnDeadlineDays; 
        private readonly IOptions<AppConfigSettings> _configSettings;
        public QuarterlyDatesService(IOptions<AppConfigSettings> configSettings)
        {
            _configSettings = configSettings ??
                              throw new ArgumentNullException(nameof(configSettings));
            _quarterStartMonths = configSettings.Value.QuarterStartMonths?.ToList() ??
                                  throw new ArgumentNullException(nameof(configSettings.Value.QuarterStartMonths));
            _returnDeadlineDays = configSettings.Value.ReturnDeadlineDays?.ToList() ??
                                  throw new ArgumentNullException(nameof(configSettings.Value.ReturnDeadlineDays));
        }

        public async Task<Dictionary<int, string>> GetQuarterMonthsToDisplay(int currentMonth, bool hasSubmittedPreviousQuarterReturn)
        {
            var monthsToDisplay = new List<int>();
            var quarterToReturn = new Dictionary<int, string>();
            
            // Overrides
            if (_configSettings.Value.CurrentMonthOverride.HasValue)
                currentMonth = _configSettings.Value.CurrentMonthOverride.Value;

            DateTime currentDate = new(DateTime.Now.Year, currentMonth, DateTime.Now.Day);

            if (_configSettings.Value.HasSubmittedReturnOverride.HasValue)
                hasSubmittedPreviousQuarterReturn = _configSettings.Value.HasSubmittedReturnOverride.Value;        

            var currentQuarter =  GetCurrentQuarter(currentDate);
            var currentMonthInQuarter = GetCurrentMonthInQuarter(currentDate);
            var isWithinCurrentQuarter = IsWithinCurrentQuarter(currentDate, currentQuarter, currentMonthInQuarter);
            var isWithinReturnDeadline = IsWithinReturnDeadline(currentDate, currentQuarter);
            if (isWithinCurrentQuarter)
                AddCurrentQuarterMonths(monthsToDisplay, currentQuarter, currentMonthInQuarter);
            else if (isWithinReturnDeadline)
                HandleReturnDeadline(monthsToDisplay, currentQuarter, hasSubmittedPreviousQuarterReturn);
            else
                HandleLateReturn(monthsToDisplay, currentQuarter, hasSubmittedPreviousQuarterReturn);
            
            // Map month numbers to their names
            foreach (var month in monthsToDisplay) 
                quarterToReturn[month] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            
            return quarterToReturn;
        }

        private static int GetCurrentQuarter(DateTime currentDate)
        {
            return (currentDate.Month - 1) / 3;
        }

        private static int GetCurrentMonthInQuarter(DateTime currentDate)
        {
            return (currentDate.Month - 1) % 3 + 1;
        }

        /// <summary>
        /// Determines whether the current date is within the current quarter.
        /// </summary>
        /// <param name="currentDate">The current date.</param>
        /// <param name="currentQuarter">The current quarter (0-based).</param>
        /// <param name="currentMonthInQuarter">The current month within the quarter (1-based).</param>
        /// <returns>True if the current date is within the current quarter; otherwise, false.</returns>
        private bool IsWithinCurrentQuarter(DateTime currentDate, int currentQuarter, int currentMonthInQuarter)
        {
            var quarterStartDate = new DateTime(currentDate.Year, _quarterStartMonths[currentQuarter], 1);
            return currentDate <= quarterStartDate.AddMonths(currentMonthInQuarter);
        }

        /// <summary>
        /// Adds the months of the current quarter to the provided list.
        /// </summary>
        /// <param name="monthsToDisplay">The list to which the months are added.</param>
        /// <param name="currentQuarter">The current quarter (0-based).</param>
        /// <param name="currentMonthInQuarter">The current month within the quarter (1-based).</param>
        private static void AddCurrentQuarterMonths(List<int> monthsToDisplay, int currentQuarter, int currentMonthInQuarter)
        {
            for (var i = 1; i <= currentMonthInQuarter; i++) 
                monthsToDisplay.Add((currentQuarter * 3) + i);
        }

        /// <summary>
        /// Determines whether the current date is within the return deadline of the current quarter.
        /// </summary>
        /// <param name="currentDate">The current date.</param>
        /// <param name="currentQuarter">The current quarter.</param>
        /// <returns>True if the current date is within the return deadline; otherwise, false.</returns>
        private bool IsWithinReturnDeadline(DateTime currentDate, int currentQuarter)
        {
            DateTime returnDeadline = new(currentDate.Year, _quarterStartMonths[currentQuarter], _returnDeadlineDays[currentQuarter]);
            
            return currentDate <= returnDeadline;
        }

        private static void HandleReturnDeadline(List<int> monthsToDisplay, int currentQuarter, bool hasSubmittedPreviousQuarterReturn)
        {
            // If the previous quarter return has been submitted, add the first month of the current quarter to the display
            if (hasSubmittedPreviousQuarterReturn)
                monthsToDisplay.Add((currentQuarter * 3) + 1);
            else
            {
                // If the previous quarter return has not been submitted, add all the months of the previous quarter to the display
                AddPreviousQuarterMonths(monthsToDisplay, currentQuarter);
                
                // Display a warning message indicating that the return date is approaching
                Console.WriteLine("Warning: Return date is approaching");
            }
        }

        private static void AddPreviousQuarterMonths(List<int> monthsToDisplay, int currentQuarter)
        {
            const int monthsInQuarter = 3;
            const int totalMonthsInYear = 12;

            var previousQuarter = currentQuarter - 1;

            for (var monthIndex = 1; monthIndex <= monthsInQuarter; monthIndex++)
            {
                var month = ((previousQuarter * monthsInQuarter + monthIndex - 1) % totalMonthsInYear) + 1;
                monthsToDisplay.Add(month);
            }
        }

        private static void HandleLateReturn(List<int> monthsToDisplay, int currentQuarter, bool hasSubmittedPreviousQuarterReturn)
        {
            monthsToDisplay.Add((currentQuarter * 3) + 1);
            if (!hasSubmittedPreviousQuarterReturn)
            {
                Console.WriteLine("Warning: Return is late");
            }
        }
    }
}



