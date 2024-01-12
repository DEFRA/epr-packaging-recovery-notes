namespace EPRN.Portal.ViewModels.PRNS
{
    public class ConfirmationViewModel
    {
        public int Id { get; set; }

        public bool PrnComplete { get; set; }

        public string CompanyNameSentTo { get; set; }

        public string PRNReferenceNumber { get; set; }
    }
}
