using EPRN.Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Portal.ViewModels.Waste
{
    public class CheckAnswersViewModel
    {
        public CheckAnswersViewModel()
        {
            Sections = new Dictionary<string, List<CheckAnswerViewModel>>();
        }

        public int JourneyId { get; set; }

        [Required(ErrorMessageResourceName = "ErrorMessage", ErrorMessageResourceType = typeof(CYAResources))]
        [Range(typeof(bool), "true", "true", ErrorMessageResourceName = "ErrorMessage", ErrorMessageResourceType = typeof(CYAResources))]
        public bool Completed { get; set; }

        public Dictionary<string, List<CheckAnswerViewModel>> Sections { get; set; }
    }
}
