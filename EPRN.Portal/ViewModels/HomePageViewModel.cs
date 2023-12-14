namespace EPRN.Portal.ViewModels
{
    public class HomepageViewModel
    {
        public string Name { get; set; } = string.Empty;

        public string ContactName { get; set; } = string.Empty;

        public string AccountNumber { get; set; } = string.Empty;

        public List<CardViewModel> CardViewModels { get; set; } = new List<CardViewModel>();
    }
}