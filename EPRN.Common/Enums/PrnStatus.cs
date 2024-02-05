namespace EPRN.Common.Enums
{
    public enum PrnStatus
    {
        Draft = 0,
        CheckYourAnswersComplete = 1,
        // sent to a producer/compliance scheme
        Sent = 2, 
        // accepted by the producer/compliance scheme - this is the last step of the process therefore no need for Completed
        Accepted = 3,
        CancellationRequested = 4,
        Cancelled = 5
    }
}