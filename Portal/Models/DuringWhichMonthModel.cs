using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class DuringWhichMonthRequestViewModel
    {

        public Dictionary<string, string> Months { get; set; } = new Dictionary<string, string>();

        [Required(ErrorMessage = "Select the month the waste was received")]
        public string SelectedMonth { get; set; }
    }
}