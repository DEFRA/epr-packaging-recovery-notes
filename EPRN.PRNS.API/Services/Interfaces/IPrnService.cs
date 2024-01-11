using EPRN.Common.Dtos;

namespace EPRN.PRNS.API.Services.Interfaces
{
    public interface IPrnService
    {
        Task<int> CreatePrnRecord();

        Task<double?> GetTonnage(int id);

        Task SaveTonnage(int id, double tonnage);

        Task<ConfirmationDto> GetConfirmation(int id);
    }
}
