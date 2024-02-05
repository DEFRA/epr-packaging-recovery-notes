using EPRN.Common.Enums;
using EPRN.Portal.Helpers.Extensions;

namespace EPRN.UnitTests.Portal.Helpers
{
    [TestClass]
    public class ExtensionMethodTests
    {
        [TestMethod]
        public void GetCancelPermission_Cancelled_ReturnsNotAllowed()
        {
            // Arrange
            PrnStatus status = PrnStatus.Cancelled;

            // Act
            var result = status.GetCancelPermission();

            // Assert
            Assert.AreEqual(CancelPermission.NotAllowed, result);
        }

        [TestMethod]
        public void GetCancelPermission_CancellationRequested_ReturnsNotAllowed()
        {
            // Arrange
            PrnStatus status = PrnStatus.CancellationRequested;

            // Act
            var result = status.GetCancelPermission();

            // Assert
            Assert.AreEqual(CancelPermission.NotAllowed, result);
        }

        [TestMethod]
        public void GetCancelPermission_Accepted_ReturnsRequestToCancelAllowed()
        {
            // Arrange
            PrnStatus status = PrnStatus.Accepted;

            // Act
            var result = status.GetCancelPermission();

            // Assert
            Assert.AreEqual(CancelPermission.RequestToCancelAllowed, result);
        }

        [TestMethod]
        public void GetCancelPermission_NotAccepted_ReturnsCancelAllowed()
        {
            // Arrange
            PrnStatus status = PrnStatus.Sent; // Assuming there's a 'Pending' status for not yet accepted

            // Act
            var result = status.GetCancelPermission();

            // Assert
            Assert.AreEqual(CancelPermission.CancelAllowed, result);
        }
    }
}
