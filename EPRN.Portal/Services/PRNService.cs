using AutoMapper;
using EPRN.Common.Enums;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;

namespace EPRN.Portal.Services
{
    public class PRNService : IPRNService
    {
        private IMapper _mapper;
        private IHttpPrnsService _httpPrnsService;

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

        public async Task<ViewSentPrnsViewModel> GetViewSentPrnsViewModel()
        {
            //TODO: The real data retrieval is being implemented by Sajid

            var row1 = CreatePrnRow("PRN001", "Paper/board", "Tesco", "20/11/2023", 107.0, PrnRecordStatus.AwaitingAcceptance, "#");
            var row2 = CreatePrnRow("PRN002", "Steel", "Tesco", "20/11/2023", 107.0, PrnRecordStatus.Rejected, "#");
            var row3 = CreatePrnRow("PRN003", "Glass Other", "Tesco", "20/11/2023", 107.0, PrnRecordStatus.Cancelled, "#");
            var row4 = CreatePrnRow("PRN004", "Paper Composting", "Tesco", "20/11/2023", 107.0, PrnRecordStatus.Accepted, "#");
            var row5 = CreatePrnRow("PRN005", "Glass Remelt", "Tesco", "20/11/2023", 107.0, PrnRecordStatus.AwaitingCancellation, "#");
            var row6 = CreatePrnRow("PRN006", "Aluminum", "Tesco", "20/11/2023", 107.0, PrnRecordStatus.Accepted, "#");
            var row7 = CreatePrnRow("PRN007", "Plastic", "Tesco", "20/11/2023", 107.0, PrnRecordStatus.Rejected, "#");
            var row8 = CreatePrnRow("PRN008", "Wood", "Tesco", "20/11/2023", 107.0, PrnRecordStatus.AwaitingAcceptance, "#");
            var row9 = CreatePrnRow("PRN009", "Wood Composting", "Tesco", "20/11/2023", 107.0, PrnRecordStatus.Cancelled, "#");
            var row10 = CreatePrnRow("PRN010", "Paper/board", "Tesco", "20/11/2023", 107.0, PrnRecordStatus.AwaitingAcceptance, "#");

            var listOfRows = new List<PrnRowViewModel>()
            {
                row1, row2, row3, row4, row5, row6, row7, row8, row9, row10
            };

            return new ViewSentPrnsViewModel
            {
                Rows = listOfRows
            };
        }

        private PrnRowViewModel CreatePrnRow(
            string prnNumber,
            string material,
            string sentTo,
            string dateCreated,
            double tonnes,
            PrnRecordStatus status,
            string link)
        {
            return new PrnRowViewModel
            {
                PrnNumber = prnNumber,
                Material = material,
                SentTo = sentTo,
                DateCreated = dateCreated,
                Tonnes = tonnes,
                Status = status,
                Link = link
            };
        }
    }
}