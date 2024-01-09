﻿using EPRN.Common.Data.DataModels;

namespace EPRN.PRNS.API.Repositories.Interfaces
{
    public interface IRepository
    {
        Task<int> CreatePrnRecord(PackagingRecoveryNote prnRecord);

        Task<PackagingRecoveryNote> GetPrnById(int id);

        Task<double?> GetTonnage(int id);

        Task UpdateTonnage(int id, double tonnes);
    }
}