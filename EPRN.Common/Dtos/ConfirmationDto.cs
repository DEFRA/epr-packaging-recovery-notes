namespace EPRN.Common.Dtos
{
    public class ConfirmationDto
    {
        public int Id { get; set; }

        public bool PrnComplete { get; set; }

        public string CompanyNameSentTo { get; set; }

        public string PRNReferenceNumber { get; set; }
    }
}