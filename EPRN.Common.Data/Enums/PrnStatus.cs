namespace EPRN.Common.Data.Enums
{
    public enum PrnStatus
    {
        Draft,
        CheckYourAnswersComplete,
        Sent, // sent to a producer/compliance scheme
        Accepted, // accepted by the producer/compliance scheme
        Complete
    }
}