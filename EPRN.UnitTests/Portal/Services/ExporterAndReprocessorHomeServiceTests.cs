using EPRN.Portal.Resources;
using EPRN.Portal.Services;
using EPRN.Portal.ViewModels;
using EPRN.Portal.ViewModels.Waste;

namespace EPRN.UnitTests.Portal.Services
{
    [TestClass]
    public class ExporterAndReprocessorHomeServiceTests
    {
        private ExporterAndReprocessorHomeService _exporterAndReprocessorHomeService;

        [TestInitialize]
        public void Init()
        {
            _exporterAndReprocessorHomeService = new ExporterAndReprocessorHomeService();
        }

        [TestMethod]
        public async Task GetHomePage_ReturnsValidModel()
        {
            // Arrange

            // Act
            var viewModel = await _exporterAndReprocessorHomeService.GetHomePage();

            // Assert
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(typeof(HomePageViewModel), viewModel.GetType());
        }

        [TestMethod]
        public async Task GetHomePage_Returns_Correct_Cards()
        {
            // Arrange

            // Act
            var viewModel = await _exporterAndReprocessorHomeService.GetHomePage();

            // Assert
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(typeof(HomePageViewModel), viewModel.GetType());

            Assert.IsTrue(viewModel.CardViewModels.Count() == 5);
            Assert.IsTrue(viewModel.CardViewModels[0].Title == HomePageResources.HomePage_Waste_Title);
            Assert.IsTrue(viewModel.CardViewModels[0].Links.Count == 2);

            Assert.AreEqual(viewModel.CardViewModels[1].Title, HomePageResources.HomePage_ManagePrnPern_Title);
            Assert.AreEqual(viewModel.CardViewModels[1].Description, HomePageResources.HomePage_ManagePrnPern_Description);
            Assert.IsTrue(viewModel.CardViewModels[1].Links.Count() == 3);
        }
    }
}
