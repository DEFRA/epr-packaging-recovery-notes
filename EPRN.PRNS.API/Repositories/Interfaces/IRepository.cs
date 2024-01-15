using EPRN.Common.Dtos;
using EPRN.Common.Enums;

namespace EPRN.PRNS.API.Repositories.Interfaces
{
    public interface IRepository
    {
        Task<int> CreatePrnRecord(
            int materialType,
            Category category);

        Task<bool> PrnExists(
            int id, 
            Category category);

        Task<double?> GetTonnage(int id);

        Task UpdateTonnage(int id, double tonnes);

        Task<ConfirmationDto> GetConfirmation(int id);
    }
}