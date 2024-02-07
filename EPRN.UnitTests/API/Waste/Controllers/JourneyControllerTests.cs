using EPRN.Common.Dtos;
using EPRN.Waste.API.Controllers;
using EPRN.Waste.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using EPRN.Common.Enums;

namespace EPRN.UnitTests.API.Waste.Controllers
{
    [TestClass]
    public class JourneyControllerTests
    {
        private JourneyController _journeyController;
        private Mock<IJourneyService> _mockJourneyService;

        [TestInitialize]
        public void Init()
        {
            _mockJourneyService = new Mock<IJourneyService>();
            var mockQuarterlyDatesService = new Mock<IQuarterlyDatesService>();
            _journeyController = new JourneyController(_mockJourneyService.Object, mockQuarterlyDatesService.Object);
        }

        [TestMethod]
        public async Task CreateJourney_CallsService()
        {
            // arrange
            var materialId = 56;
            var category = Category.Exporter;
            var companyReferenceId = Guid.NewGuid().ToString();

            // act
            await _journeyController.CreateJourney(materialId, category, companyReferenceId);

            // assert
            _mockJourneyService.Verify(s => 
                s.CreateJourney(
                    It.Is<int>(p => p == materialId),
                    It.Is<Category>(p => p == category),
                    It.Is<string>(p => p == companyReferenceId)
                ), Times.Once());
        }

        [TestMethod]
        public async Task WasteType_Returns_OK()
        {
            //Arrange
            var journeyId = 3;

            //Act
            var result = await _journeyController!.WasteType(journeyId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _mockJourneyService.Verify(s => s.GetWasteType(
                 It.Is<int>(p => p == journeyId))
             );
        }

        [TestMethod]
        public async Task WasteType_ReturnsBadRequest_WhenNoIdSupplied()
        {
            //Arrange

            //Act
            var result = await _journeyController!.WasteType(null);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockJourneyService.Verify(s => s.GetWasteType(
                 It.IsAny<int>()), Times.Never
             );
        }

        [TestMethod]
        public async Task GetSelectedMonth_Returns_OK_With_Valid_Id()
        {
            //Arrange
            var journeyId = 3;

            //Act
            var result = await _journeyController.GetSelectedMonth(journeyId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _mockJourneyService.Verify(s => s.GetSelectedMonth(It.Is<int>(p => p == journeyId)));
        }

        [TestMethod]
        public async Task SaveJourneyMonth_ReturnsOk_WhenValid()
        {
            //Arrange
            var journeyId = 5;
            var monthSelected = 7;

            //Act
            var result = await _journeyController!.SaveJourneyMonth(journeyId, monthSelected);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));
            _mockJourneyService.Verify(s => s.SaveSelectedMonth(
                 It.Is<int>(p => p == journeyId),
                 It.Is<int>(p => p == monthSelected)), Times.Once
             );
        }

        [TestMethod]
        public async Task SaveJourneyMonth_ReturnsBadRequest_WhenNoJourneyId()
        {
            //Arrange
            var monthSelected = 7;

            //Act
            var result = await _journeyController.SaveJourneyMonth(null, monthSelected);

            //Assert
            Assert.AreEqual(monthSelected, 7);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockJourneyService.Verify(s => s.SaveSelectedMonth(
                 It.IsAny<int>(),
                 It.Is<int>(p => p == monthSelected)), Times.Never
             );
        }

        [TestMethod]
        public async Task SaveJourneyMonth_ReturnsBadRequest_WhenNoMonthSelected()
        {
            //Arrange
            var journeyId = 5;
            var expectedWhatHaveYouDoneWaste = DoneWaste.SentItOn;

            _mockJourneyService.Setup(ws => ws.GetWhatHaveYouDoneWaste(It.Is<int>(p => p == journeyId))).ReturnsAsync(expectedWhatHaveYouDoneWaste);

            //Act
            var result = await _journeyController.SaveJourneyMonth(journeyId, null);

            //Assert
            Assert.AreEqual(journeyId, 5);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockJourneyService.Verify(s => s.SaveSelectedMonth(
                 It.Is<int>(p => p == journeyId),
                 It.IsAny<int>()), Times.Never
             );
        }

        [TestMethod]
        public async Task SaveWhatHaveYouDoneWaste_ReturnsOk_WhenValid()
        {
            //Arrange
            var journeyId = 5;

            //Act
            var result = await _journeyController!.SaveWhatHaveYouDoneWaste(journeyId, DoneWaste.ReprocessedIt);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));
            _mockJourneyService.Verify(s => s.SaveWhatHaveYouDoneWaste(
                 It.Is<int>(p => p == journeyId),
                 It.Is<DoneWaste>(p => p == DoneWaste.ReprocessedIt)), Times.Once
             );
        }

        [TestMethod]
        public async Task SaveWhatHaveYouDoneWaste_ReturnsBadRequest_WhenNoJourneyId()
        {
            //Arrange

            //Act
            var result = await _journeyController.SaveWhatHaveYouDoneWaste(null, DoneWaste.ReprocessedIt);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockJourneyService.Verify(s => s.SaveWhatHaveYouDoneWaste(
                 It.IsAny<int>(),
                 It.Is<DoneWaste>(p => p == DoneWaste.ReprocessedIt)), Times.Never
             );
        }

        [TestMethod]
        public async Task SetJourneyTonnage_WithValidParameters_SavesSuccesfully()
        {
            // arrange
            int journeyId = 4;
            double tonnage = 56.7;

            // act
            var result = await _journeyController.SetJourneyTonnage(journeyId, tonnage);

            // assert
            _mockJourneyService.Verify(s =>
                s.SaveTonnage(
                    It.Is<int>(p => p == journeyId),
                    It.Is<double>(p => p == tonnage)),
                Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task SetJourneyTonnage_WithNoJourneyID_ThrowsBadRequest()
        {
            // arrange
            double tonnage = 56.7;

            // act
            var result = await _journeyController.SetJourneyTonnage(null, tonnage);

            // assert
            _mockJourneyService.Verify(s =>
                s.SaveTonnage(
                    It.IsAny<int>(),
                    It.IsAny<double>()),
                Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task SetJourneyTonnage_WithNoTonnage_ThrowsBadRequest()
        {
            // arrange
            int journeyId = 4;

            // act
            var result = await _journeyController.SetJourneyTonnage(journeyId, null);

            // assert
            _mockJourneyService.Verify(s =>
                s.SaveTonnage(
                    It.IsAny<int>(),
                    It.IsAny<double>()),
                Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task GetWhatHaveYouDoneWaste_Returns_OK()
        {
            //Arrange
            var journeyId = 3;

            //Act
            var result = await _journeyController.GetWhatHaveYouDoneWaste(journeyId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _mockJourneyService.Verify(s => s.GetWhatHaveYouDoneWaste(It.Is<int>(p => p == journeyId)));
        }

        [TestMethod]
        public async Task GetWhatHaveYouDoneWaste_ReturnsBadRequest_WhenNoIdSupplied()
        {
            //Arrange

            //Act
            var result = await _journeyController.GetWhatHaveYouDoneWaste(null);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockJourneyService.Verify(s => s.GetWhatHaveYouDoneWaste(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task GetGetBaledWithWire_Returns_OK()
        {
            //Arrange
            var journeyId = 3;

            //Act
            var result = await _journeyController.GetBaledWithWire(journeyId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _mockJourneyService.Verify(s => s.GetBaledWithWire(It.Is<int>(p => p == journeyId)));
        }

        [TestMethod]
        public async Task GetGetBaledWithWire_ReturnsBadRequest_WhenNoIdSupplied()
        {
            //Arrange

            //Act
            var result = await _journeyController.GetBaledWithWire(null);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockJourneyService.Verify(s => s.GetBaledWithWire(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task GetJourneyAnswers_WithValidId_ReturnsOk()
        {
            //Arrange
            int validJourneyId = 1;

            //Act
            var result = await _journeyController.GetJourneyAnswers(validJourneyId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task GetJourneyAnswers_WithNullId_ReturnsBadRequest()
        {
            //Arrange
            int? nullJourneyId = null;

            //Act
            var result = await _journeyController.GetJourneyAnswers(nullJourneyId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Journey ID is missing", badRequestResult.Value);
        }

        [TestMethod]
        public async Task GetJourneyAnswers_WithZeroId_ReturnsBadRequest()
        {
            //Arrange
            int zeroJourneyId = 0;

            //Act
            var result = await _journeyController.GetJourneyAnswers(zeroJourneyId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Invalid journey ID", badRequestResult.Value);
        }

        [TestMethod]
        public async Task GetJourneyAnswers_WithValidId_ReturnsCorrectDto()
        {
            //Arrange
            int validJourneyId = 1;

            var expectedDto = new JourneyAnswersDto();
            expectedDto.Month = 2;
            expectedDto.Tonnes = 250;
            expectedDto.BaledWithWire = false;
            expectedDto.Adjustment = 34.5;
            expectedDto.Note = "Some text";
            expectedDto.WasteSubType = "sorted mixed paper/board";
            expectedDto.DoneWaste = DoneWaste.ReprocessedIt;
            expectedDto.Completed = true;

            _mockJourneyService.Setup(service => service.GetJourneyAnswers(It.IsAny<int>())).ReturnsAsync(expectedDto);

            //Act
            var result = await _journeyController.GetJourneyAnswers(validJourneyId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var actualDto = okResult.Value as JourneyAnswersDto;
            Assert.IsNotNull(actualDto);
            Assert.AreEqual(expectedDto, actualDto);
        }

        [TestMethod]
        public async Task GetJourneyAnswers_WithInValidId_ReturnsBadRequest()
        {
            //Arrange
            _mockJourneyService.Setup(service => service.GetJourneyAnswers(It.IsAny<int>())).ReturnsAsync(null as JourneyAnswersDto);
            int invalidJourneyId = -1;

            //Act
            var result = await _journeyController.GetJourneyAnswers(invalidJourneyId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Invalid journey ID", badRequestResult.Value);
        }

        [TestMethod]
        public async Task GetWasteRecordNote_WithValidId_ReturnsCorrectDto()
        {
            //Arrange
            int validJourneyId = 1;
            string validNote = "abc";
            Category wasteCategory = Category.Unknown;

            var expectedDto = new NoteDto();
            expectedDto.JourneyId = validJourneyId;
            expectedDto.Note = validNote;
            expectedDto.WasteCategory = wasteCategory;

            _mockJourneyService.Setup(service => service.GetWasteRecordNote(It.IsAny<int>())).ReturnsAsync(expectedDto);

            //Act
            var result = await _journeyController.GetWasteRecordNote(validJourneyId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var actualDto = okResult.Value as NoteDto;
            Assert.IsNotNull(actualDto);
            Assert.AreEqual(expectedDto, actualDto);
        }

        [TestMethod]
        public async Task GetJourneyAccredidationLimitAlert_WithValidId_ReturnsCorrectDto()
        {
            //Arrange
            int validJourneyId = 1;
            string userReferenceId = "someuser";
            double newQuantityEntered = 20;

            var expectedDto = new AccredidationLimitDto();
            expectedDto.JourneyId = validJourneyId;
            expectedDto.AccredidationLimit = Common.Constants.Double.AccredidationLimit;
            
            _mockJourneyService.Setup(service => service.GetAccredidationLimit(It.IsAny<string>(), It.IsAny<double>()))
                .ReturnsAsync(expectedDto);

            //Act
            var result = await _journeyController.GetUserAccredidationLimitAlert(validJourneyId, userReferenceId, newQuantityEntered);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var actualDto = okResult.Value as AccredidationLimitDto;
            Assert.IsNotNull(actualDto);
            Assert.AreEqual(expectedDto, actualDto);
        }

    }
}
