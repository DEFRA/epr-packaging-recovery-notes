namespace EPRN.Portal.RESTServices.Interfaces
{
    public interface IHttpPrnsService
    {
        Task<double?> GetPrnTonnage(int id);

        Task SaveTonnage(int id, double tonnage);

        Task<int> CreatePrnRecord(int materialId);
    }
}
