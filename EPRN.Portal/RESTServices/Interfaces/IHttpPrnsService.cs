﻿using EPRN.Common.Dtos;
using EPRN.Common.Enums;

namespace EPRN.Portal.RESTServices.Interfaces
{
    public interface IHttpPrnsService
    {
        Task<double?> GetPrnTonnage(
            int id);

        Task SaveTonnage(
            int id,
            double tonnage);

        Task<ConfirmationDto> GetConfirmation(
            int id);

        Task<CheckYourAnswersDto> GetCheckYourAnswers(
            int id);

        Task<int> CreatePrnRecord(
            int materialId,
            Category category);

        Task SaveCheckYourAnswers(
            int id);

        Task<List<SentPrnsDto>> GetSentPrns(
            int? page,
            int pageSize,
            string? searchTerm,
            string? filterBy,
            string? sortBy
            );
    }
}
