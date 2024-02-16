using EPRN.Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Moq;
using EPRN.Portal.Helpers;


namespace EPRN.UnitTests.Portal.Helpers
{
    [TestClass]
    public class CultureHelperTests
    {
        private Mock<IHttpContextAccessor> httpContextAccessor;
        private DefaultHttpContext httpContext;
        private CultureHelper _cultureHelper;

        [TestInitialize]
        public void Init()
        {
            httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContext = new DefaultHttpContext();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            _cultureHelper = new CultureHelper(httpContextAccessor.Object);
        }

        [TestMethod]
        public void GetCultureInfo_ValidHttpContext_ReturnsOppositeCulture()
        {
            // Arrange
            var requestCulture = new RequestCulture(CultureConstants.English);
            httpContext?.Features.Set<IRequestCultureFeature>(new RequestCultureFeature(requestCulture, null));

            // Act
            var result = _cultureHelper.GetCultureInfo();

            // Assert
            Assert.AreEqual(CultureConstants.Welsh.Name, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetCultureInfo_NullHttpContextAccessor_ThrowsException()
        {
            // Act & Assert
            _cultureHelper = new CultureHelper(null);
        }

        [TestMethod]
        public void GetCultureInfo_WelshCulture_ReturnsEnglishCulture()
        {
            // Arrange
            var requestCulture = new RequestCulture(CultureConstants.Welsh);
            httpContext?.Features.Set<IRequestCultureFeature>(new RequestCultureFeature(requestCulture, null));

            // Act
            var result = _cultureHelper.GetCultureInfo();

            // Assert
            Assert.AreEqual(CultureConstants.English.Name, result);
        }
    }
}
