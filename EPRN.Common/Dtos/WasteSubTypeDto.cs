namespace EPRN.Common.Dtos
{
    public class WasteSubTypeDto
    {
        public string Name { get; set; }

        public decimal Adjustment { get; set; }

        public int WasteTypeId { get; set; }

        public WasteTypeDto WasteType { get; set; }
    }
}
