using Portal.ViewModels;

namespace Portal.Services.Interfaces
{
    public interface IWasteService
    {
        DuringWhichMonthRequestViewModel GetCurrentQuarter(int journeyId);

        WhatHaveYouDoneWasteModel GetWasteModel(int journeyId);

        Task SaveSelectedMonth(int journeyId, int selectedMonth);

        Task SaveSelectedWasteType(int journeyId, string selectedWasteType);
    }
}
