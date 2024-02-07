using EPRN.Portal.Configuration;
using EPRN.Portal.Resources;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services.HomeServices;
using EPRN.Portal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using Moq;

namespace EPRN.UnitTests.Portal.Services
{
    [TestClass]
    public class ExporterServiceTests
    {
        private UserBasedExporterService _exporterHomeService;
        private IOptions<AppConfigSettings> _configSettings;
        private Mock<IUrlHelper> _mockUrlHelper;
        private Mock<IHttpJourneyService> _mockHttpJourneyService = null;

        [TestInitialize]
        public void Init()
        {
            _configSettings = Options.Create<AppConfigSettings>(new AppConfigSettings());
            _configSettings.Value.DeductionAmount_Exporter = 0.0;
            _configSettings.Value.DeductionAmount_Reprocessor = 0.0;
            _configSettings.Value.DeductionAmount_ExporterAndReprocessor = 0.0;
            _mockHttpJourneyService = new Mock<IHttpJourneyService>();

            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            var actionContextAccessor = new Mock<IActionContextAccessor>();
           _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            _mockUrlHelper.Setup(
                x => x.Action(
                    It.IsAny<UrlActionContext>()
                )
            )
            .Returns("callbackUrl");

            urlHelperFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>())).Returns(_mockUrlHelper.Object);

            _exporterHomeService = new UserBasedExporterService(
                _configSettings,
                _mockHttpJourneyService.Object,
                urlHelperFactory.Object,
                actionContextAccessor.Object
                );
        }

        [TestMethod]
        public async Task GetHomePage_ReturnsValidModel()
        {
            // Arrange

            // Act
            var viewModel = await _exporterHomeService.GetHomePage();

            // Assert
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(typeof(HomePageViewModel), viewModel.GetType());
        }

        [TestMethod]
        public async Task GetHomePage_Returns_Correct_Cards()
        {
            // Arrange

            // Act
            var viewModel = await _exporterHomeService.GetHomePage();

            // Assert
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(typeof(HomePageViewModel), viewModel.GetType());

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
