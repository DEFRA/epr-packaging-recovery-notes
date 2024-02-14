using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Portal.Helpers;
using EPRN.Portal.Resources;
using EPRN.Portal.Resources.PRNS;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography.Xml;

namespace EPRN.Portal.Services
{
    public class PRNService : IPRNService
    {
        private readonly IMapper _mapper;
        private readonly IHttpPrnsService _httpPrnsService;

        public PRNService(
            IMapper mapper,
            IHttpPrnsService httpPrnsService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _httpPrnsService = httpPrnsService ?? throw new ArgumentNullException(nameof(httpPrnsService));
        }

        public async Task<TonnesViewModel> GetTonnesViewModel(
            int id)
        {
            return new TonnesViewModel
            {
                Id = id,
                Tonnes = await _httpPrnsService.GetPrnTonnage(id)
            };
        }

        public async Task SaveTonnes(
            TonnesViewModel tonnesViewModel)
        {
            await _httpPrnsService.SaveTonnage(
                tonnesViewModel.Id,
                tonnesViewModel.Tonnes.Value);
        }

        public async Task<ConfirmationViewModel> GetConfirmation(
            int id)
        {
            var confirmationDto = await _httpPrnsService.GetConfirmation(id);

            if (confirmationDto == null)
                throw new NullReferenceException(nameof(confirmationDto));

            if (string.IsNullOrWhiteSpace(confirmationDto.CompanyNameSentTo))
                confirmationDto.CompanyNameSentTo = "<UNKNOWN>";

            return _mapper.Map<ConfirmationViewModel>(confirmationDto);
        }

        public async Task<CreatePrnViewModel> CreatePrnViewModel()
        {
            // TODO: This needs to get the data from a future service

            var tableRowReprocessor = CreateRow(1, "Glass", 0, 2);

            var tableRowExporter = CreateRow(2, "Aluminium", 0.3, 1);
            var tableRowExporter2 = CreateRow(3, "Board/paper", 17, 4);

            var listOfRowsReprocessor = new List<TableRowViewModel>
            {
                tableRowReprocessor
            };

            var listOfRowsExporter = new List<TableRowViewModel>
            {
                tableRowExporter,
                tableRowExporter2
            };

            var tableReprocessor = CreateTable("23 Ruby Street, London, SE15 1LR", listOfRowsReprocessor);
            var tableExporter = CreateTable("123 Sesame Street, London, N12 8JJ", listOfRowsExporter);

            var listOfTables = new List<TableViewModel>
            {
                tableReprocessor,
                tableExporter
            };

            return new CreatePrnViewModel
            {
                Tables = listOfTables
            };
        }

        public async Task<CheckYourAnswersViewModel> GetCheckYourAnswersViewModel(int id)
        {
            var checkYourAnswersDto = await _httpPrnsService.GetCheckYourAnswers(id);

            return _mapper.Map<CheckYourAnswersViewModel>(checkYourAnswersDto);
        }

        public async Task SaveCheckYourAnswers(int id)
        {
            await _httpPrnsService.SaveCheckYourAnswers(id);
        }


        public async Task<int> CreatePrnRecord(
            int materialId,
            Category category)
        {
            return await _httpPrnsService.CreatePrnRecord(materialId, category);
        }

        private TableRowViewModel CreateRow(int materialId, string material, double tonnage, int noOfDrafts)
        {
            return new TableRowViewModel
            {
                MaterialId = materialId,
                MaterialName = material,
                Tonnage = tonnage,
                NoOfDrafts = noOfDrafts
            };
        }

        private TableViewModel CreateTable(string siteAddress, List<TableRowViewModel> rows)
        {
            return new TableViewModel
            {
                SiteAddress = siteAddress,
                Rows = rows
            };
        }

        public async Task<PrnSavedAsDraftViewModel> GetDraftPrnConfirmationModel(int id)
        {
            //TODO: The PrnNumber property needs to be system generated in the future
            return new PrnSavedAsDraftViewModel
            {
                Id = id,
                PrnNumber = "PRN222019EFGF",
            };
        }

        public async Task<CancelViewModel> GetCancelViewModel(int id)
        {
            var dto = await _httpPrnsService.GetStatusAndProducer(id);

            return _mapper.Map<CancelViewModel>(dto);
        }

        public async Task<RequestCancelViewModel> GetRequestCancelViewModel(int id)
        {
            var dto = await _httpPrnsService.GetStatusAndProducer(id);

            return _mapper.Map<RequestCancelViewModel>(dto);
        }

        public async Task CancelPRN(CancelViewModel cancelViewModel)
        {
            await _httpPrnsService.CancelPRN(
                cancelViewModel.Id,
                cancelViewModel.CancelReason);
        }

        public async Task RequestToCancelPRN(RequestCancelViewModel requestCancelViewModel)
        {
            await _httpPrnsService.RequestCancelPRN(
                requestCancelViewModel.Id,
                requestCancelViewModel.CancelReason);
        }

        public async Task<(DecemberWasteViewModel, bool)> GetDecemberWasteModel(int id)
        {
            var dto = await _httpPrnsService.GetDecemberWaste(id);

            return (_mapper.Map<DecemberWasteViewModel>(dto), dto.IsWithinMonth);
        }

        public async Task SaveDecemberWaste(DecemberWasteViewModel decemberWasteModel)
        {
            if (decemberWasteModel == null)
                throw new ArgumentNullException(nameof(decemberWasteModel));

            if (decemberWasteModel.WasteForDecember == null)
                throw new ArgumentNullException(nameof(decemberWasteModel.WasteForDecember));

            await _httpPrnsService.SaveDecemberWaste(
                decemberWasteModel.Id,
                decemberWasteModel.WasteForDecember.Value);
        }

        public async Task<ViewSentPrnsViewModel> GetViewSentPrnsViewModel(GetSentPrnsViewModel request)
        {
            var getSentPrnsDto = _mapper.Map<GetSentPrnsDto>(request);
            var sentPrnsDto = await _httpPrnsService.GetSentPrns(getSentPrnsDto);
           
            var viewModel = _mapper.Map<ViewSentPrnsViewModel>(sentPrnsDto);

            viewModel.FilterItems = EnumHelpers.ToSelectList(typeof(PrnStatus),
                ViewSentPrnResources.FilterBy,
                PrnStatus.Accepted, 
                PrnStatus.AwaitingAcceptance, 
                PrnStatus.Rejected,
                PrnStatus.AwaitingCancellation, 
                PrnStatus.Cancelled);
            
            viewModel.SortItems = new List<SelectListItem>
            {
                new() { Value = "", Text = @ViewSentPrnResources.SortBy }, 
                new() { Value = "1", Text = "Material" },
                new() { Value = "2", Text = "Sent to" }
            };

            return viewModel;
        }

        public async Task<ViewPRNViewModel> GetViewPrnViewModel(string reference)
        {
            var dto = await _httpPrnsService.GetPrnDetails(reference);

            return _mapper.Map<ViewPRNViewModel>(dto);
        }

        public async Task<DraftConfirmationViewModel> GetDraftConfirmationViewModel(int id)
        {
            var dto = await _httpPrnsService.GetPrnReference(id);
            var actionPrnViewModel = new DraftConfirmationViewModel
            { 
                Id = id
            };
            return actionPrnViewModel;
        }

        public async Task SaveDraftPrn(DraftConfirmationViewModel draftConfirmationViewModel)
        {
            if (draftConfirmationViewModel == null)
                throw new ArgumentNullException(nameof(draftConfirmationViewModel));

            if (draftConfirmationViewModel.DoWithPRN == null)
                throw new ArgumentNullException(nameof(draftConfirmationViewModel.DoWithPRN));

            await _httpPrnsService.SaveDraftPrn(draftConfirmationViewModel.Id);
        }
    }
}