using EPRN.Common.Data.DataModels;

namespace EPRN.PRNS.API.Repositories.Interfaces
{
    public interface IRepository
    {
        Task<int> CreatePrnRecord();

        Task<PackagingRecoveryNote> GetPrnById(int id);

        Task<int?> GetTonnage(int id);

        Task UpdateTonnage(int id, int tonnes);
    }
}