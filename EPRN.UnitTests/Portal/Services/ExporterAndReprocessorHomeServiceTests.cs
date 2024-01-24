using EPRN.Portal.Configuration;
using EPRN.Portal.Resources;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services;
using EPRN.Portal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;

namespace EPRN.UnitTests.Portal.Services
{
    [TestClass]
    public class ExporterAndReprocessorHomeServiceTests
    {
        private ExporterAndReprocessorHomeService _exporterAndReprocessorHomeService;
        private IOptions<AppConfigSettings> _configSettings;
        private Mock<IHttpJourneyService> _mockHttpJourneyService = null;

        [TestInitialize]
        public void Init()
        {
            _configSettings = Options.Create<AppConfigSettings>(new AppConfigSettings());
            _configSettings.Value.DeductionAmount_Exporter = 0.0;
            _configSettings.Value.DeductionAmount_Reprocessor = 0.0;
            _configSettings.Value.DeductionAmount_ExporterAndReprocessor = 0.0;
            _mockHttpJourneyService = new Mock<IHttpJourneyService>();

            _exporterAndReprocessorHomeService = new ExporterAndReprocessorHomeService(
                _configSettings, 
                _mockHttpJourneyService.Object
                );
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
