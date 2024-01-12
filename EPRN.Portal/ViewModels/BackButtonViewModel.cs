namespace EPRN.Portal.ViewModels
{
    public class BackButtonViewModel
    {
        public bool ShouldDisplay => !string.IsNullOrEmpty(PreviousUrl);

        public string PreviousUrl { get; set; }
    }
}
