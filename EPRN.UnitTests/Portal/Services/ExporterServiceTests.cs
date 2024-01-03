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
            
            Assert.AreEqual(viewModel.CardViewModels[1].Title, HomePageResources.HomePage_ManagePern_Title);
            Assert.AreEqual(viewModel.CardViewModels[1].Description, HomePageResources.HomePage_ManagePern_Description);
            Assert.IsTrue(viewModel.CardViewModels[1].Links.Count() == 3);

            Assert.IsTrue(viewModel.CardViewModels[2].Title != null);
            Assert.IsTrue(viewModel.CardViewModels[2].Title == HomePageResources.HomePage_Returns_Title);
            Assert.IsTrue(viewModel.CardViewModels[2].Description == HomePageResources.HomePage_Returns_Description);
            Assert.IsTrue(viewModel.CardViewModels[2].Links != null);
            Assert.IsTrue(viewModel.CardViewModels[2].Links.Count == 1);

        }
    }
}
