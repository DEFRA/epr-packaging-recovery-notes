using EPRN.Portal.Resources;

namespace EPRN.Portal.ViewModels.PRNS
{
    public class TableRowViewModel
    {
        public string Material { get; set; }
        public double Tonnage { get; set; }
        public int NoOfDrafts { get; set; }
        public string ChooseLink { get; set; }
        public string BalanceWarning { get; set; } = @CreatePrnPern.TitlePrnPern;

    }
}
