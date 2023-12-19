using EPRN.PRNS.API.Models;

namespace EPRN.PRNS.API.Repositories.Interfaces
{
    public interface IRepository
    {
        bool LazyLoading { set; }

        Task<PackagingRecoveryNote> GetPrnById(int id);
    }
}