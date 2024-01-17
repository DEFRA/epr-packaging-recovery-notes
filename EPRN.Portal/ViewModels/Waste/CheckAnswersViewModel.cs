namespace EPRN.Portal.ViewModels.Waste
{
    public class CheckAnswersViewModel
    {
        public CheckAnswersViewModel()
        {
            Sections = new Dictionary<string, List<CheckAnswerViewModel>>();
        }

        public int JourneyId { get; set; }

        public bool Completed { get; set; }

        public Dictionary<string, List<CheckAnswerViewModel>> Sections { get; set; }
    }
}
