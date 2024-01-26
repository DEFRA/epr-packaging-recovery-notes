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

        public async Task<ViewSentPrnsViewModel> GetViewSentPrnsViewModel(int page = 1, string searchTerm = null)
        {
            //TODO: The real data retrieval is being implemented by Sajid

            var row1 = CreatePrnRow("PRN001", "Paper/board", "Tesco", "20/11/2023", 107.0, PrnRecordStatus.AwaitingAcceptance, "#");
            var row2 = CreatePrnRow("PRN002", "Steel", "Morrisons", "02/1/2024", 4.5, PrnRecordStatus.Rejected, "#");
            var row3 = CreatePrnRow("PRN003", "Glass Other", "Sainsbury's", "17/05/2023", 3.1, PrnRecordStatus.Cancelled, "#");
            var row4 = CreatePrnRow("PRN004", "Paper Composting", "Asda", "23/07/2023", 15.6, PrnRecordStatus.Accepted, "#");
            var row5 = CreatePrnRow("PRN005", "Glass Remelt", "Aldi", "4/12/2023", 27.8, PrnRecordStatus.AwaitingCancellation, "#");
            var row6 = CreatePrnRow("PRN006", "Aluminum", "Poundland", "19/03/2023", 91.2, PrnRecordStatus.Accepted, "#");
            var row7 = CreatePrnRow("PRN007", "Plastic", "Lidl", "07/04/2023", 43.9, PrnRecordStatus.Rejected, "#");
            var row8 = CreatePrnRow("PRN008", "Wood", "Wilko", "22/02/2023", 63.4, PrnRecordStatus.AwaitingAcceptance, "#");
            var row9 = CreatePrnRow("PRN009", "Wood Composting", "Farmfoods", "08/08/2023", 211.6, PrnRecordStatus.Cancelled, "#");
            var row10 = CreatePrnRow("PRN010", "Paper/board", "Co-op Food", "16/06/2023", 92.7, PrnRecordStatus.AwaitingAcceptance, "#");

            var row11 = CreatePrnRow("PRN011", "Paper/board", "Tesco", "20/11/2023", 107.0, PrnRecordStatus.AwaitingAcceptance, "#");
            var row12 = CreatePrnRow("PRN012", "Steel", "Morrisons", "02/1/2024", 4.5, PrnRecordStatus.Rejected, "#");
            var row13 = CreatePrnRow("PRN013", "Glass Other", "Sainsbury's", "17/05/2023", 3.1, PrnRecordStatus.Cancelled, "#");
            var row14 = CreatePrnRow("PRN014", "Paper Composting", "Asda", "23/07/2023", 15.6, PrnRecordStatus.Accepted, "#");
            var row15 = CreatePrnRow("PRN015", "Glass Remelt", "Aldi", "4/12/2023", 27.8, PrnRecordStatus.AwaitingCancellation, "#");
            var row16 = CreatePrnRow("PRN016", "Aluminum", "Poundland", "19/03/2023", 91.2, PrnRecordStatus.Accepted, "#");
            var row17 = CreatePrnRow("PRN017", "Plastic", "Lidl", "07/04/2023", 43.9, PrnRecordStatus.Rejected, "#");
            var row18 = CreatePrnRow("PRN018", "Wood", "Wilko", "22/02/2023", 63.4, PrnRecordStatus.AwaitingAcceptance, "#");
            var row19 = CreatePrnRow("PRN019", "Wood Composting", "Farmfoods", "08/08/2023", 211.6, PrnRecordStatus.Cancelled, "#");
            var row20 = CreatePrnRow("PRN020", "Paper/board", "Co-op Food", "16/06/2023", 92.7, PrnRecordStatus.AwaitingAcceptance, "#");

            var row21 = CreatePrnRow("PRN021", "Paper/board", "Tesco", "20/11/2023", 107.0, PrnRecordStatus.AwaitingAcceptance, "#");
            var row22 = CreatePrnRow("PRN022", "Steel", "Morrisons", "02/1/2024", 4.5, PrnRecordStatus.Rejected, "#");
            var row23 = CreatePrnRow("PRN023", "Glass Other", "Sainsbury's", "17/05/2023", 3.1, PrnRecordStatus.Cancelled, "#");
            var row24 = CreatePrnRow("PRN024", "Paper Composting", "Asda", "23/07/2023", 15.6, PrnRecordStatus.Accepted, "#");
            var row25 = CreatePrnRow("PRN025", "Glass Remelt", "Aldi", "4/12/2023", 27.8, PrnRecordStatus.AwaitingCancellation, "#");
            var row26 = CreatePrnRow("PRN026", "Aluminum", "Poundland", "19/03/2023", 91.2, PrnRecordStatus.Accepted, "#");
            var row27 = CreatePrnRow("PRN027", "Plastic", "Lidl", "07/04/2023", 43.9, PrnRecordStatus.Rejected, "#");
            var row28 = CreatePrnRow("PRN028", "Wood", "Wilko", "22/02/2023", 63.4, PrnRecordStatus.AwaitingAcceptance, "#");
            var row29 = CreatePrnRow("PRN029", "Wood Composting", "Farmfoods", "08/08/2023", 211.6, PrnRecordStatus.Cancelled, "#");
            var row30 = CreatePrnRow("PRN030", "Paper/board", "Co-op Food", "16/06/2023", 92.7, PrnRecordStatus.AwaitingAcceptance, "#");

            var listOfRows = new List<PrnRowViewModel>()
            {
                row1, row2, row3, row4, row5, row6, row7, row8, row9, row10,
                row11, row12, row13, row14, row15, row16, row17, row18, row19, row20,
                row21, row22, row23, row24, row25, row26, row27, row28, row29, row30
            };


            if (!string.IsNullOrEmpty(searchTerm))
            {
                listOfRows = listOfRows.Where(
                    e => e.PrnNumber.Contains(
                        searchTerm,
                        StringComparison.OrdinalIgnoreCase
                        )
                    ||
                        e.SentTo.Contains(
                            searchTerm,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var itemsPerPage = 10;
            var totalItems = listOfRows.Count;
            var totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            page = Math.Max(1, Math.Min(totalPages, page));

            var startIndex = (page - 1) * itemsPerPage;
            var paginatedItems = listOfRows.Skip(startIndex).Take(itemsPerPage);

            var paginationModel = new PaginationViewModel
            {
                TotalItems = totalItems,
                ItemsPerPage = itemsPerPage,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return new ViewSentPrnsViewModel
            {
                Rows = paginatedItems,
                Pagination = paginationModel
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