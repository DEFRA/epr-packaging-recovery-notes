#nullable enable
using EPRN.PRNS.API.Data;
using EPRN.PRNS.API.Models;
using EPRN.PRNS.API.Repositories.Interfaces;

namespace EPRN.PRNS.API.Repositories
{
    public class Repository : IRepository
    {
        private readonly PrnContext _prnContext;

        public bool LazyLoading { get; set; }
        public Task<PackagingRecoveryNote> GetPrnById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
