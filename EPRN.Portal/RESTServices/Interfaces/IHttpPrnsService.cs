namespace EPRN.Portal.RESTServices.Interfaces
{
    public interface IHttpPrnsService
    {
        Task<int?> GetPrnTonnage(int id);

        Task SaveTonnage(int id, int tonnage);
    }
}
