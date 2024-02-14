namespace EPRN.Portal.ViewModels.PRNS
{
    public class ViewDraftPrnViewModel
    {
        public string PrnNumber { get; set; }
        public string Material {  get; set; }
        public string SentTo { get; set; }
        public DateTime DateCreated { get; set; }
        public int Tonnage { get; set; }

    }
}
