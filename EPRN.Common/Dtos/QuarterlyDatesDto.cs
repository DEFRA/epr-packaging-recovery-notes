namespace EPRN.Common.Dtos
{
    public class QuarterlyDatesDto
    {
        public Dictionary<int, string> QuarterlyMonths { get; set; }
        public String Notification { get; set; }
        public DateTime NotificationDeadlineDate { get; set; }
    }
}
