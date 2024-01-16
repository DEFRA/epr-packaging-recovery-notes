namespace EPRN.Common.Dtos
{
    public class JourneyAnswersDto
    {
        public string Month { get; set; }

        public double? Tonnes { get; set; }

        public string BaledWithWire { get; set; }

        public double? TonnageAdjusted { get; set; }

        public string? Note { get; set; }

        public string WasteType { get; set; }

        public string WasteSubType { get; set; }

        public string WhatDoneWithWaste { get; set; }

        public bool? Completed { get; set; }

    }
}
