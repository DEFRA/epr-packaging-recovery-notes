﻿using EPRN.Common.Data.DataModels;
using EPRN.Common.Dtos;

namespace EPRN.PRNS.API.Repositories.Interfaces
{
    public interface IRepository
    {
        Task<int> CreatePrnRecord(PackagingRecoveryNote prnRecord);

        Task<bool> PrnExists(int id);

        Task<double?> GetTonnage(int id);

        Task UpdateTonnage(int id, double tonnes);

        Task<ConfirmationDto> GetConfirmation(int id);
    }
}