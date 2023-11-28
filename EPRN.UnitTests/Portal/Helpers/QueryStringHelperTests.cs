using Microsoft.AspNetCore.Http;
using Moq;
using Portal.Helpers;

namespace EPRN.UnitTests.Portal.Helpers
{
    [TestClass]
    public class QueryStringHelperTests
    {
        private Mock<IHttpContextAccessor>? httpContextAccessor;
        private DefaultHttpContext? httpContext;

        [TestInitialize]
        public void Init()
        {
            httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContext = new DefaultHttpContext();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        }

        [TestMethod]
        public void RemoveCultureQueryString_CultureQueryStringPresent_RemovesCultureQueryString()
        {
            // Arrange
            httpContext!.Request.QueryString = new QueryString("?param1=value1&culture=en-GB&param2=value2");
            var queryStringHelper = new QueryStringHelper(httpContextAccessor!.Object);

            // Act
            var result = queryStringHelper.RemoveCultureQueryString();

            // Assert
            Assert.AreEqual("&param1=value1&param2=value2", result);
        }

        [TestMethod]
        public void RemoveCultureQueryString_CultureQueryStringNotPresent_ReturnsOriginalQueryString()
        {
            // Arrange
            httpContext!.Request.QueryString = new QueryString("?param1=value1&param2=value2");
            var queryStringHelper = new QueryStringHelper(httpContextAccessor!.Object);

            // Act
            var result = queryStringHelper.RemoveCultureQueryString();

            // Assert
            Assert.AreEqual("&param1=value1&param2=value2", result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveCultureQueryString_NullHttpContextAccessor_ThrowsException()
        {
            // Arrange
            var queryStringHelper = new QueryStringHelper(null!);

            // Act & Assert
            queryStringHelper.RemoveCultureQueryString();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveCultureQueryString_NullHttpContext_ThrowsException()
        {
            // Arrange
            httpContextAccessor!.Setup(x => x.HttpContext).Returns((HttpContext)null!);

            var queryStringHelper = new QueryStringHelper(httpContextAccessor.Object);

            // Act & Assert
            queryStringHelper.RemoveCultureQueryString();
        }
    }
}
