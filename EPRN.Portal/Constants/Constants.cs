using EPRN.Common.Enums;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;
using EPRN.Portal.ViewModels.Waste;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace EPRN.Portal.Constants
{
    public static class CultureConstants
    {
        public static readonly CultureInfo English = new CultureInfo("en-GB");
        public static readonly CultureInfo Welsh = new CultureInfo("cy-GB");
    }

    public static class Strings
    {
        public static class ApiEndPoints
        {
            public const string Journey = "Journey";
            public const string Waste = "Waste";
            public const string PRN = "PRN";
        }

        public static class QueryStrings
        {
            public const string ReturnToAnswers = "rtap";
            public const string ReturnToAnswersYes = "y";

        }
    }

    public static class Routes
    {
        public static class Areas
        {
            public static class Exporter
            {
                public const string PRNS = "PRNS";

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
                    }
                }
            }

            public static class Reprocessor
            {
                public const string PRNS = "PRNS";

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
                    }
                }
            }
        }

        public static class Controllers
        {
            public const string Home = "Home";
            public const string Prns = "Prns";
            public const string Waste = "Waste";

            public static class Actions
            {
                public static class Prns
                {
                    public const string Create = "Create";
                }

                public static class Waste
                {
                    public const string Done = "Done";
                    public const string Month = "Month";
                    public const string RecordWaste = "RecordWaste";
                    public const string SubTypes = "SubTypes";
                    public const string WasteRecordStatus = "WasteRecordStatus";
                    public const string Tonnes = "Tonnes";
                    public const string Baled = "Baled";
                    public const string ReProcessorExport = "ReProcessorExport";
                    public const string Note = "Note";
                }
            }
        }
    }
}

/*

Routes.Areas.Exporter.PRNS

 */