using EPRN.Common.Data.DataModels;

namespace EPRN.PRNS.API.Repositories.Interfaces
{
    public interface IRepository
    {
        Task<int> CreatePrnRecord();

        Task<bool> PrnExists(int id);

        Task<PackagingRecoveryNote> GetPrnById(int id);

        Task<double?> GetTonnage(int id);

        Task UpdateTonnage(int id, double tonnes);
    }
}