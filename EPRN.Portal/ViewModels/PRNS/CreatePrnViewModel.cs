namespace EPRN.Portal.ViewModels.PRNS
{
    public class CreatePrnViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<TableViewModel> TableViewModels { get; set; } = new List<TableViewModel>();
    }
}
