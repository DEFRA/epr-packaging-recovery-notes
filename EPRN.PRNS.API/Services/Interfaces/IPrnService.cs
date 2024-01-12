namespace EPRN.PRNS.API.Services.Interfaces
{
    public interface IPrnService
    {
        Task<bool> PrnRecordExists(int id);

        Task<int> CreatePrnRecord();

        Task<double?> GetTonnage(int id);

        Task SaveTonnage(int id, double tonnage);
    }
}
