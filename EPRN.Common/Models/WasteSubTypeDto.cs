namespace EPRN.Common.Models
{
    public class WasteSubTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Adjustment { get; set; }

        public int WasteTypeId { get; set; }

        public WasteTypeDto WasteType { get; set; }
    }
}
