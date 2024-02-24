using System.Globalization;

namespace EPRN.Common.Constants;

public static class CultureConstants
{
    public static readonly CultureInfo English = new("en-GB");
    public static readonly CultureInfo Welsh = new("cy-GB");
}

public static class Strings
{
    public static class ApiEndPoints
    {
        public const string Journey = "Journey";
        public const string Waste = "Waste";
        public const string PRN = "PRN";
        public const string Returns = "Returns";
    }

    public static class Notifications
    {
        public const string QuarterlyReturnDue = "QuarterlyReturnDue";
        public const string QuarterlyReturnLate = "QuarterlyReturnLate";
    }

    public static class QueryStrings
    {
        public const string ReturnToAnswers = "rtap";
        public const string ReturnToAnswersYes = "y";
    }

    public static class RepoStrings
    {
        public const string DeleteDraft = "Deleted draft";
    }

    /// <summary>
    /// This class should be used to store the values of areas, controllers and action names
    /// So we can use this instead of strings directly in controllers and views
    /// </summary>
    public static class Routes
    {
        public static class Areas
        {
            public const string Exporter = "Exporter";
            public const string Reprocessor = "Reprocessor";

            public static class Controllers
            {
                public static class Exporter
                {
                    public const string PRNS = "PRNS";
                }

                public static class Reprocessor
                {
                    public const string PRNS = "PRNS";
                }
            }

            public static class Actions
            {
                public static class PRNS
                {
                    public const string Tonnes = "Tonnes";
                    public const string CreatePrn = "CreatePrn";
                    public const string Confirmation = "Confirmation";
                    public const string CheckYourAnswers = "CheckYourAnswers";
                    public const string SentTo = "SentTo";
                    public const string WhatToDo = "WhatToDo";
                    public const string Cancel = "Cancel";
                    public const string RequestCancel = "RequestCancel";
                    public const string Cancelled = "Cancelled";
                    public const string RequestCancelConfirmed = "RequestCancelConfirmed";
                    public const string DecemberWaste = "DecemberWaste";
                    public const string DeleteDraft = "DeleteDraft";
                    public const string DraftConfirmation = "DraftConfirmation";
                    public const string PrnSavedAsDraftConfirmation = "PrnSavedAsDraftConfirmation";
                    public const string DraftPrns = "DraftPrns";
                }
            }
        }

        public static class Controllers
        {
            public const string Home = "Home";
            public const string PRNS = "Prns";
            public const string Waste = "Waste";
        }

        public static class Actions
        {
            public static class PRNS
            {
                public const string Create = "Create";
                public const string View = "View";
                public const string ViewSentPrns = "ViewSentPrns";
                public const string PrnSavedAsDraftConfirmation = "PrnSavedAsDraftConfirmation";
            }

            public static class Waste
            {
                public const string Create = "Create";
                public const string Done = "Done";
                public const string Month = "Month";
                public const string RecordWaste = "RecordWaste";
                public const string SubTypes = "SubTypes";
                public const string WasteRecordStatus = "WasteRecordStatus";
                public const string Tonnes = "Tonnes";
                public const string Baled = "Baled";
                public const string ReProcessorExport = "ReProcessorExport";
                public const string Note = "Note";
                public const string DecemberWaste = "DecemberWaste";
                public const string AccredidationLimit = "AccredidationLimit";
            }
        }
    }
}
