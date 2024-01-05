using EPRN.PRNS.API.Models;

namespace EPRN.PRNS.API.Repositories.Interfaces
{
    public interface IRepository
    {
        Task<int> CreatePrnRecord();

        Task<PackagingRecoveryNote> GetPrnById(int id);

        Task<double?> GetTonnage(int id);

        Task UpdateTonnage(int id, double tonnes);
    }
}