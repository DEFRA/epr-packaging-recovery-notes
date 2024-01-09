namespace EPRN.PRNS.API.Services.Interfaces
{
    public interface IPrnService
    {
        Task<int> CreatePrnRecord();

        Task<int?> GetTonnage(int id);

        Task SaveTonnage(int id, int tonnage);
    }
}
