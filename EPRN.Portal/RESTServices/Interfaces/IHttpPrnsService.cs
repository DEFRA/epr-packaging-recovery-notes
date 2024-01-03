namespace EPRN.Portal.RESTServices.Interfaces
{
    public interface IHttpPrnsService
    {
        Task SaveTonnage(int id, double tonnage);
    }
}
