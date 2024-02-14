namespace EPRN.Common.Enums
{
    public enum PrnStatus
    {
        // DO NOT change the order of the current enum items, new items can be added to the end
        Created,
        Draft,
        CheckYourAnswersComplete,
        // sent to a producer/compliance scheme
        Sent, 
        // accepted by the producer/compliance scheme - this is the last step of the process therefore no need for Completed
        Accepted ,
        CancellationRequested ,
        Cancelled ,
        Complete,
        AwaitingAcceptance,
        Rejected,
        AwaitingCancellation
    }
}