namespace EPRN.Portal.ViewModels.PRNS
{
    public class CheckYourAnswersViewModel
    {
        public int Id { get; set; }

        public bool AnswersComplete => !string.IsNullOrWhiteSpace(MaterialName) && Tonnage.HasValue && !string.IsNullOrWhiteSpace(RecipientName);

        public string MaterialName { get; set; }

        public double? Tonnage { get; set; }

        public string RecipientName { get; set; }

        public string Notes { get; set; }
    }
}
