﻿using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using System.Drawing;

namespace EPRN.PRNS.API.Services.Interfaces
{
    public interface IPrnService
    {
        Task<bool> PrnRecordExists(
            int id);

        Task<int> CreatePrnRecord(
            int materialId,
            Category category);

        Task<double?> GetTonnage(int id);

        Task SaveTonnage(int id, double tonnage);

        Task<ConfirmationDto> GetConfirmation(int id);

        Task<CheckYourAnswersDto> GetCheckYourAnswers(int id);

        Task SaveCheckYourAnswers(int id);

        Task CancelPrn(int id, string reason);
    }
}
