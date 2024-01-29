﻿using EPRN.Common.Dtos;
using EPRN.Common.Enums;

namespace EPRN.PRNS.API.Repositories.Interfaces
{
    public interface IRepository
    {
        Task<int> CreatePrnRecord(
            int materialType,
            Category category);

        Task<bool> PrnExists(
            int id);

        Task<double?> GetTonnage(int id);

        Task UpdateTonnage(int id, double tonnes);

        Task<ConfirmationDto> GetConfirmation(int id);

        Task<CheckYourAnswersDto> GetCheckYourAnswersData(int id);

        Task UpdatePrnStatus(int id, PrnStatus status);

        Task<List<SentPrnsDto>> GetSentPrns(
            int? page,
            int pageSize,
            string? searchTerm,
            string? filterBy,
            string? sortBy
            );
    }
}