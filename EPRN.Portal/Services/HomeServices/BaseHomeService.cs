using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels;

namespace EPRN.Portal.Services
{
    public abstract class BaseHomeService : IHomeService
    {
        public virtual async Task<HomepageViewModel> GetHomePage()
        {
            // TODO: Replace with actual data in the future
            var homePageViewModel = new HomepageViewModel
            {
                Name = "Green LTD",
                ContactName = "John Watson",
                AccountNumber = "12 Head office St, Liverpool, L12 345 - 0098678"
            };

            homePageViewModel.CardViewModels = GetCardViewModels();

            return homePageViewModel;
        }

        protected abstract List<CardViewModel> GetCardViewModels();

        protected CardViewModel GetCardViewModel(string title, string description)
        {
            var cardViewModel = new CardViewModel
            {
                Title = title,
                Description = description
            };

            return cardViewModel;
        }

        public abstract double? GetBaledWithWireDeductionPercentage();

    }
}
