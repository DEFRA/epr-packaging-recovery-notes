namespace EPRN.Common.Enums
{
    public enum PrnStatus
    {
        Draft,
        CheckYourAnswersComplete,
        // sent to a producer/compliance scheme
        Sent, 
        // accepted by the producer/compliance scheme - this is the last step of the process therefore no need for Completed
        Accepted, 
        Cancelled
    }
}