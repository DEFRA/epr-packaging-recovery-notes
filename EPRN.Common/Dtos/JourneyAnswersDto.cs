namespace EPRN.Common.Dtos
{
    public class JourneyAnswersDto
    {
        public int JourneyId { get; set; }

        public string Month { get; set; }

        public string Tonnes { get; set; }

        public string BaledWithWire { get; set; }

        public string TonnageAdjusted { get; set; }

        public string? Note { get; set; }

        public string WasteType { get; set; }

        public string WasteSubType { get; set; }

        public string WhatDoneWithWaste { get; set; }

        public bool Completed { get; set; }

    }
}
