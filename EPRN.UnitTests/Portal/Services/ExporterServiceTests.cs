using EPRN.Portal.Resources;
using EPRN.Portal.Services;
using EPRN.Portal.Services.HomeServices;
using EPRN.Portal.ViewModels;

namespace EPRN.UnitTests.Portal.Services
{
    [TestClass]
    public class ExporterServiceTests
    {
        private ExporterHomeService _exporterHomeService;

        [TestInitialize]
        public void Init()
        {
            _exporterHomeService = new ExporterHomeService();
        }

        [TestMethod]
        public async Task GetHomePage_ReturnsValidModel()
        {
            // Arrange

            // Act
            var viewModel = await _exporterHomeService.GetHomePage();

            // Assert
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(typeof(HomepageViewModel), viewModel.GetType());
        }

        [TestMethod]
        public async Task GetHomePage_Returns_Correct_Cards()
        {
            // Arrange

            // Act
            var viewModel = await _exporterHomeService.GetHomePage();

            // Assert
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(typeof(HomepageViewModel), viewModel.GetType());

            Assert.IsTrue(viewModel.CardViewModels.Count() == 5);
            Assert.IsTrue(viewModel.CardViewModels[0].Title == HomePageResources.HomePage_Waste_Title);
            Assert.IsTrue(viewModel.CardViewModels[0].Links.Count == 2);

            Assert.AreEqual(viewModel.CardViewModels[1].Title, HomePageResources.HomePage_ManagePern_Title);
            Assert.AreEqual(viewModel.CardViewModels[1].Description, HomePageResources.HomePage_ManagePern_Description);
            Assert.IsTrue(viewModel.CardViewModels[1].Links.Count() == 3);
        }
    }
}
