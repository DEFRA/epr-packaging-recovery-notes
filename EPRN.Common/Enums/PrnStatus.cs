namespace EPRN.Common.Enums
{
    public enum PrnStatus
    {
        // DO NOT change the order of the current enum items, new items can be added to the end
        Draft,
        CheckYourAnswersComplete,
        Sent, // sent to a producer/compliance scheme
        Accepted, // accepted by the producer/compliance scheme
        Complete,
        Cancelled,
        AwaitingAcceptance,
        Rejected,
        AwaitingCancellation
    }
}
