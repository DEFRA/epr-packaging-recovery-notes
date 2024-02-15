using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Portal.Helpers;
using EPRN.Portal.Resources;
using EPRN.Portal.Resources.PRNS;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services;
using EPRN.Portal.ViewModels.PRNS;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;

namespace EPRN.UnitTests.Portal.Services
{
    [TestClass]
    public class PrnServiceTests
    {
        private PRNService _prnService;
        private Mock<IMapper> _mockMapper;
        private Mock<IHttpPrnsService> _mockHttpPrnsService;

        [TestInitialize]
        public void Init()
        {
            _mockHttpPrnsService = new Mock<IHttpPrnsService>();
            _mockMapper = new Mock<IMapper>();

            _prnService = new PRNService(
                _mockMapper.Object,
                _mockHttpPrnsService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PrnService_ThrowsException_WhenNoMapperSupplied()
        {
            _prnService = new PRNService(
                null,
                _mockHttpPrnsService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PrnService_ThrowsException_WhenNoHttpPrnServiceSupplied()
        {
            _prnService = new PRNService(
                _mockMapper.Object,
                null);
        }

        [TestMethod]
        public async Task GetTonnesViewModel_CallsHttpService_WithCorrectParameters()
        {
            // arrange
            var id = 8;

            // act
            await _prnService.GetTonnesViewModel(id);

            // assert
            _mockHttpPrnsService.Verify(s =>
                s.GetPrnTonnage(
                    It.Is<int>(p => p == id)),
                Times.Once);
        }

        [TestMethod]
        public async Task SaveTonnes_CallsService_WithCorrectParameters()
        {
            // arrange
            var id = 9;
            var tonnes = 4.89;

            var viewModel = new TonnesViewModel
            {
                Id = id,
                Tonnes = tonnes,
            };

            // act
            await _prnService.SaveTonnes(viewModel);

            // assert
            _mockHttpPrnsService.Verify(s =>
                s.SaveTonnage(
                    It.Is<int>(p => p == id),
                    It.Is<double>(p => p == tonnes)),
                Times.Once);
        }

        [TestMethod]
        public async Task GetConfirmation_CallService_WithCorrectParameters()
        {
            // arrange
            var id = 3;
            _mockHttpPrnsService.Setup(s => s.GetConfirmation(id)).ReturnsAsync(new ConfirmationDto());

            // act
            await _prnService.GetConfirmation(id);

            // assert
            _mockHttpPrnsService.Verify(s =>
                s.GetConfirmation(
                    It.Is<int>(p => p == id)),
                Times.Once);
        }

        [TestMethod]
        public async Task GetCancelViewModel_CallsService_Successfully()
        {
            // arrange
            var id = 123;
            var dto = new StatusAndProducerDto();
            _mockHttpPrnsService.Setup(s => s.GetStatusAndProducer(id)).ReturnsAsync(dto);

            // act
            var viewModel = await _prnService.GetCancelViewModel(id);

            // assert
            _mockHttpPrnsService.Verify(s =>
                s.GetStatusAndProducer(
                    It.Is<int>(p => p == id)),
                Times.Once);
            _mockMapper.Verify(m =>
                m.Map<CancelViewModel>(
                    It.Is<StatusAndProducerDto>(p => p == dto)),
                Times.Once);
        }

        [TestMethod]
        public async Task CancelPRN_CallsService_Successfully()
        {
            // arrange
            var id = 432;
            var reason = "fsdgdsfg";
            var viewModel = new CancelViewModel
            {
                Id = id,
                CancelReason = reason,
            };

            // act
            await _prnService.CancelPRN(viewModel);

            // assert
            _mockHttpPrnsService.Verify(s =>
                s.CancelPRN(
                    It.Is<int>(p => p == id),
                    It.Is<string>(p => p == reason)),
                Times.Once);
        }

        [TestMethod]
        public async Task GetRequestCancelViewModel_ShouldReturnViewModel()
        {
            // Arrange
            int id = 1;
            var expectedDto = new StatusAndProducerDto();

            _mockHttpPrnsService
                .Setup(x => x.GetStatusAndProducer(It.IsAny<int>()))
                .ReturnsAsync(expectedDto);

            // Act
            var result = await _prnService.GetRequestCancelViewModel(id);

            // Assert
            _mockHttpPrnsService.Verify(s =>
                s.GetStatusAndProducer(
                    It.Is<int>(p => p == id)),
                Times.Once);
            _mockMapper.Verify(m =>
                m.Map<RequestCancelViewModel>(
                    It.Is<StatusAndProducerDto>(p => p == expectedDto)),
                Times.Once);
        }

        #region GetViewSentPrnsViewModel

        [TestMethod]
        public async Task GetViewSentPrnsViewModel_HappyPath()
        {
            // Arrange
            var request = new GetSentPrnsViewModel();
            var getSentPrnsDto = new GetSentPrnsDto();
            var sentPrnsDto = new SentPrnsDto();
            var expectedViewModel = new ViewSentPrnsViewModel();

            _mockMapper.Setup(m => m.Map<GetSentPrnsDto>(request)).Returns(getSentPrnsDto);
            _mockHttpPrnsService.Setup(service => service.GetSentPrns(getSentPrnsDto)).ReturnsAsync(sentPrnsDto);
            _mockMapper.Setup(m => m.Map<ViewSentPrnsViewModel>(sentPrnsDto)).Returns(expectedViewModel);

            // Act
            var result = await _prnService.GetViewSentPrnsViewModel(request);

            // Assert
            _mockMapper.Verify(m => m.Map<GetSentPrnsDto>(request), Times.Once);
            _mockHttpPrnsService.Verify(s => s.GetSentPrns(getSentPrnsDto), Times.Once);
            _mockMapper.Verify(m => m.Map<ViewSentPrnsViewModel>(sentPrnsDto), Times.Once);
            Assert.AreSame(expectedViewModel, result);
        }

        [TestMethod]
        public async Task GetViewSentPrnsViewModel_ShouldMapRequestToDto()
        {
            // Arrange
            var request = new GetSentPrnsViewModel();
            var getSentPrnsDto = new GetSentPrnsDto();
            var sentPrnsDto = new SentPrnsDto();
            var expectedViewModel = new ViewSentPrnsViewModel();

            _mockMapper.Setup(m => m.Map<GetSentPrnsDto>(request)).Returns(getSentPrnsDto);
            _mockHttpPrnsService.Setup(service => service.GetSentPrns(getSentPrnsDto)).ReturnsAsync(sentPrnsDto);
            _mockMapper.Setup(m => m.Map<ViewSentPrnsViewModel>(sentPrnsDto)).Returns(expectedViewModel);

            // Act
            var result = await _prnService.GetViewSentPrnsViewModel(request);

            // Assert
            _mockMapper.Verify(m => m.Map<GetSentPrnsDto>(request), Times.Once());
        }

        [TestMethod]
        public async Task GetViewSentPrnsViewModel_ShouldCallGetSentPrnsService()
        {
            // Arrange
            var request = new GetSentPrnsViewModel();
            var getSentPrnsDto = new GetSentPrnsDto();
            var sentPrnsDto = new SentPrnsDto();
            var expectedViewModel = new ViewSentPrnsViewModel();

            _mockMapper.Setup(m => m.Map<GetSentPrnsDto>(request)).Returns(getSentPrnsDto);
            _mockHttpPrnsService.Setup(service => service.GetSentPrns(getSentPrnsDto)).ReturnsAsync(sentPrnsDto);
            _mockMapper.Setup(m => m.Map<ViewSentPrnsViewModel>(sentPrnsDto)).Returns(expectedViewModel);

            // Act
            var result = await _prnService.GetViewSentPrnsViewModel(request);

            // Assert
            _mockHttpPrnsService.Verify(s => s.GetSentPrns(It.IsAny<GetSentPrnsDto>()), Times.Once());
        }

        [TestMethod]
        public async Task GetViewSentPrnsViewModel_ShouldMapDtoToViewModel()
        {
            // Arrange
            var request = new GetSentPrnsViewModel();
            _mockMapper.Setup(m => m.Map<ViewSentPrnsViewModel>(It.IsAny<GetSentPrnsDto>())).Returns(new ViewSentPrnsViewModel());

            // Act
            var result = await _prnService.GetViewSentPrnsViewModel(request);

            // Assert
            _mockMapper.Verify(m => m.Map<ViewSentPrnsViewModel>(It.IsAny<GetSentPrnsDto>()), Times.Once());
        }

        [TestMethod]
        public async Task GetViewSentPrnsViewModel_ShouldSetFilterItems()
        {
            // Arrange
            var request = new GetSentPrnsViewModel();
            var getSentPrnsDto = new GetSentPrnsDto();
            var sentPrnsDto = new SentPrnsDto();
            var expectedViewModel = new ViewSentPrnsViewModel();

            var expectedFilterItems = EnumHelpers.ToSelectList(typeof(PrnStatus),
                ViewSentPrnResources.FilterBy,
                MasterResources.ResourceManager,
                PrnStatus.Accepted,
                PrnStatus.AwaitingAcceptance,
                PrnStatus.Rejected,
                PrnStatus.AwaitingCancellation,
                PrnStatus.Cancelled);

            _mockMapper.Setup(m => m.Map<GetSentPrnsDto>(request)).Returns(getSentPrnsDto);
            _mockHttpPrnsService.Setup(service => service.GetSentPrns(getSentPrnsDto)).ReturnsAsync(sentPrnsDto);
            _mockMapper.Setup(m => m.Map<ViewSentPrnsViewModel>(sentPrnsDto)).Returns(expectedViewModel);

            // Act
            var result = await _prnService.GetViewSentPrnsViewModel(request);

            // Assert
            Assert.IsNotNull(result.FilterItems);
            Assert.IsTrue(expectedFilterItems.Count == result.FilterItems.Count);
            Assert.AreEqual(expectedFilterItems[1].Value, result.FilterItems[1].Value);
        }

        [TestMethod]
        public async Task GetViewSentPrnsViewModel_ShouldSetSortItems()
        {
            // Arrange
            var request = new GetSentPrnsViewModel();
            var getSentPrnsDto = new GetSentPrnsDto();
            var sentPrnsDto = new SentPrnsDto();
            var expectedViewModel = new ViewSentPrnsViewModel();

            var expectedSortItems = new List<SelectListItem>
            {
                new() { Value = "", Text = @ViewSentPrnResources.SortBy },
                new() { Value = "1", Text = "Material" },
                new() { Value = "2", Text = "Sent to" }
            };

            _mockMapper.Setup(m => m.Map<GetSentPrnsDto>(request)).Returns(getSentPrnsDto);
            _mockHttpPrnsService.Setup(service => service.GetSentPrns(getSentPrnsDto)).ReturnsAsync(sentPrnsDto);
            _mockMapper.Setup(m => m.Map<ViewSentPrnsViewModel>(sentPrnsDto)).Returns(expectedViewModel);

            // Act
            var result = await _prnService.GetViewSentPrnsViewModel(request);

            // Assert
            Assert.IsNotNull(result.SortItems);
            Assert.IsTrue(expectedSortItems.Count == result.SortItems.Count);
            Assert.AreEqual(expectedSortItems[1].Value, result.SortItems[1].Value);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task GetViewSentPrnsViewModel_ThrowsNullReferenceException_WhenRequestObjectIsNull()
        {
            // Arrange
            var request = (GetSentPrnsViewModel)null;

            _mockMapper.Setup(m => m.Map<GetSentPrnsDto>(request)).Returns((GetSentPrnsDto)null);

            // Act
            var result = await _prnService.GetViewSentPrnsViewModel(request);

            // Assert
            Assert.IsNull(result);
        }
        #endregion

        [TestMethod]
        public async Task GetViewPrnViewModel_ShouldReturnMappedViewModel()
        {
            // Arrange
            var reference = "YourReferenceValue";
            var expectedDto = new PRNDetailsDto(); // Replace YourDto with the actual type returned by _httpPrnsService.GetPrnDetails
            var expectedViewModel = new ViewPRNViewModel(); // Replace ViewPRNViewModel with the actual ViewModel type

            // Mocking the IHttpPrnsService
            _mockHttpPrnsService.Setup(service => service.GetPrnDetails(reference))
                .ReturnsAsync(expectedDto);

            // Mocking the IMapper
            _mockMapper.Setup(m => m.Map<ViewPRNViewModel>(expectedDto))
                .Returns(expectedViewModel);

            // Act
            var result = await _prnService.GetViewPrnViewModel(reference);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedViewModel, result);
        }

        [TestMethod]
        public async Task GetDecemberWasteViewModel_ShouldReturnMappedViewModel()
        {
            // Arrange
            int reference = 25;
            var expectedDto = new DecemberWasteDto();
            var expectedViewModel = new DecemberWasteViewModel();

            expectedViewModel.BalanceAvailable = 260;
            expectedViewModel.Id = reference;

            // Mocking the IHttpPrnsService
            _mockHttpPrnsService.Setup(service => service.GetDecemberWaste(reference))
                .ReturnsAsync(expectedDto);

            // Mocking the IMapper
            _mockMapper.Setup(m => m.Map<DecemberWasteViewModel>(expectedDto))
                .Returns(expectedViewModel);

            // Act
            var result = await _prnService.GetDecemberWasteModel(reference);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedViewModel.Id, result.Item1.Id);
            Assert.AreEqual(expectedViewModel.BalanceAvailable, result.Item1.BalanceAvailable);
        }

        #region GetDeleteDraftPrnViewModel

        [TestMethod]
        public async Task GetDeleteDraftPrnViewModel_ReturnsCorrectViewModel_WhenIdIsValid()
        {
            // Arrange
            int id = 123;
            var dto = new DeleteDraftPrnDto();
            var expectedViewModel = new DeleteDraftPrnViewModel();

            _mockHttpPrnsService.Setup(service => service.GetPrnReference(id)).ReturnsAsync(dto);
            _mockMapper.Setup(m => m.Map<DeleteDraftPrnViewModel>(dto)).Returns(expectedViewModel);

            // Act
            var result = await _prnService.GetDeleteDraftPrnViewModel(id);

            // Assert
            Assert.AreEqual(expectedViewModel, result);
        }

        [TestMethod]
        public async Task GetDeleteDraftPrnViewModel_ReturnsNullViewModel_WhenDtoIsNull()
        {
            // Arrange
            int id = 123;
            _mockHttpPrnsService.Setup(service => service.GetPrnReference(id)).ReturnsAsync((DeleteDraftPrnDto)null);

            // Act
            var result = await _prnService.GetDeleteDraftPrnViewModel(id);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetDeleteDraftPrnViewModel_ChecksMapperIsCalled_WithCorrectParameters()
        {
            // Arrange
            int id = 123;
            var dto = new DeleteDraftPrnDto();

            _mockHttpPrnsService.Setup(service => service.GetPrnReference(id)).ReturnsAsync(dto);

            // Act
            var result = await _prnService.GetDeleteDraftPrnViewModel(id);

            // Assert
            _mockMapper.Verify(m => m.Map<DeleteDraftPrnViewModel>(dto), Times.Once);
        }

        [TestMethod]
        public async Task GetDeleteDraftPrnViewModel_ReturnsNullViewModel_WhenMappingFails()
        {
            // Arrange
            int id = 123;
            var dto = new DeleteDraftPrnDto();

            _mockHttpPrnsService.Setup(service => service.GetPrnReference(id)).ReturnsAsync(dto);
            _mockMapper.Setup(m => m.Map<DeleteDraftPrnViewModel>(dto)).Returns((DeleteDraftPrnViewModel)null);

            // Act
            var result = await _prnService.GetDeleteDraftPrnViewModel(id);

            // Assert
            Assert.IsNull(result);
            _mockMapper.Verify(m => m.Map<DeleteDraftPrnViewModel>(dto), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetDeleteDraftPrnViewModel_MapperThrowsException_AndReturnsNullViewModel()
        {
            // Arrange
            int id = 123;
            var dto = new DeleteDraftPrnDto();

            _mockHttpPrnsService.Setup(service => service.GetPrnReference(id)).ReturnsAsync(dto);
            _mockMapper.Setup(m => m.Map<DeleteDraftPrnViewModel>(dto)).Throws(new Exception());

            // Act
            var result = await _prnService.GetDeleteDraftPrnViewModel(id);

            // Assert
            Assert.IsNull(result);
            _mockMapper.Verify(m => m.Map<DeleteDraftPrnViewModel>(dto), Times.Once);
        }

        [TestMethod]
        public async Task DeleteDraftPrn_CallsDeleteDraftPrn_OnHttpPrnsService_WithValidId()
        {
            // Arrange
            var viewModel = new DeleteDraftPrnViewModel { Id = 123 };

            // Act
            await _prnService.DeleteDraftPrn(viewModel);

            // Assert
            _mockHttpPrnsService.Verify(mock => mock.DeleteDraftPrn(viewModel.Id), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task DeleteDraftPrn_ThrowsNullReferenceException_WhenViewModelIsNull()
        {
            // Arrange
            DeleteDraftPrnViewModel viewModel = null;

            // Act & Assert
            var result = _prnService.DeleteDraftPrn(viewModel);

            // Assert
            _mockHttpPrnsService.Verify(mock => mock.DeleteDraftPrn(viewModel.Id), Times.Never);
        }

        #endregion
    }
}
