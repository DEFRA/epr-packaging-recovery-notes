namespace EPRN.Common.Dtos
{
    public class QuarterlyDatesDto
    {
        public Dictionary<int, string> QuarterlyMonths { get; set; }
        public string Notification { get; set; }
        public DateTime NotificationDeadlineDate { get; set; }
        public DateTime SubmissionDate { get; set; }
        public int? SelectedMonth { get; set; }
    }
}
