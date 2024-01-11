namespace EPRN.Portal.ViewModels.Waste
{
    public class CheckAnswersViewModel
    {
        public CheckAnswersViewModel()
        {
            Sections = new Dictionary<string, List<CheckAnswerViewModel>>();
        }

        public int JourneyId { get; set; }

        public Dictionary<string, List<CheckAnswerViewModel>> Sections { get; set; }

        //public List<CheckAnswerViewModel> WasteReceivedAnswers { get; set; }

        //public List<CheckAnswerViewModel> WasteTypeAndWeightAnswers { get; set; }

        //public List<CheckAnswerViewModel> AdditionalInfoAnswers { get; set; }
    }
}
