namespace EPRN.Common.Data.Enums
{
    public enum PrnStatus
    {
        Created = 0,
        // DO NOT change the order of the current enum items, new items can be added to the end
        Draft = 1,
        CheckYourAnswersComplete = 2,
        // sent to a producer/compliance scheme
        Sent = 3,
        // accepted by the producer/compliance scheme - this is the last step of the process therefore no need for Completed
        Accepted = 4,
        CancellationRequested = 5,
        Cancelled = 6,
        Complete,
        AwaitingAcceptance,
        Rejected,
        AwaitingCancellation
    }
}