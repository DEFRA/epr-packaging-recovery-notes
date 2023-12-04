using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Moq;
using EPRN.Portal.Constants;
using EPRN.Portal.Helpers;

namespace EPRN.UnitTests.Portal.Helpers
{
    [TestClass]
    public class CultureHelperTests
    {
        private Mock<IHttpContextAccessor> httpContextAccessor;
        private DefaultHttpContext httpContext;

        [TestInitialize]
        public void Init()
        {
            httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContext = new DefaultHttpContext();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        }

        [TestMethod]
        public void GetCultureInfo_ValidHttpContext_ReturnsOppositeCulture()
        {
            // Arrange
            var requestCulture = new RequestCulture(CultureConstants.English);
            httpContext?.Features.Set<IRequestCultureFeature>(new RequestCultureFeature(requestCulture, null));

            // Act
            var result = CultureHelper.GetCultureInfo(httpContextAccessor?.Object!);

            // Assert
            Assert.AreEqual(CultureConstants.Welsh.Name, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetCultureInfo_NullHttpContextAccessor_ThrowsException()
        {
            // Act & Assert
            CultureHelper.GetCultureInfo(null!);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetCultureInfo_NullHttpContext_ThrowsException()
        {
            // Arrange
            httpContextAccessor?.Setup(x => x.HttpContext).Returns((HttpContext)null!);

            // Act & Assert
            CultureHelper.GetCultureInfo(httpContextAccessor?.Object!);
        }

        [TestMethod]
        public void GetCultureInfo_WelshCulture_ReturnsEnglishCulture()
        {
            // Arrange
            var requestCulture = new RequestCulture(CultureConstants.Welsh);
            httpContext?.Features.Set<IRequestCultureFeature>(new RequestCultureFeature(requestCulture, null));

            // Act
            var result = CultureHelper.GetCultureInfo(httpContextAccessor?.Object!);

            // Assert
            Assert.AreEqual(CultureConstants.English.Name, result);
        }
    }
}
