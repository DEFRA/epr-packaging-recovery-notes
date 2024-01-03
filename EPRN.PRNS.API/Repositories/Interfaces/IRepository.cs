using EPRN.PRNS.API.Models;

namespace EPRN.PRNS.API.Repositories.Interfaces
{
    public interface IRepository
    {
        Task<PackagingRecoveryNote> GetPrnById(int id);

        Task SaveTonnage(int id, double tonnes);
    }
}