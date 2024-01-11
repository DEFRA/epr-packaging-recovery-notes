namespace EPRN.Portal.ViewModels.Waste
{
    public class CheckAnswersViewModel
    {
        public int JourneyId { get; set; }

        public List<CheckAnswerViewModel> WasteReceivedAnswers { get; set; }

        public List<CheckAnswerViewModel> WasteTypeAndWeightAnswers { get; set; }

        public List<CheckAnswerViewModel> AdditionalInfoAnswers { get; set; }
    }
}
