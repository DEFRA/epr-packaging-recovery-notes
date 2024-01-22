namespace EPRN.Portal.ViewModels.Waste
{
    public class CYABaseViewModel
    {
        public int JourneyId { get; set; }

        //[Required(ErrorMessageResourceName = "ErrorMessage", ErrorMessageResourceType = typeof(CYAResources))]
        //[Range(typeof(bool), "true", "true", ErrorMessageResourceName = "ErrorMessage", ErrorMessageResourceType = typeof(CYAResources))]
        public bool Completed { get; set; }


        public string TypeOfWaste { get; internal set; }
        public string BaledWithWire { get; internal set; }
        public string TonnageAdjusted { get; internal set; }
        public string TonnageOfWaste { get; internal set; }
        public string MonthReceived { get; internal set; }
        public string Note { get; internal set; }
    }
}
