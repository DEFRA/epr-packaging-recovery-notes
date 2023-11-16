using WasteManagement.API.Models;

namespace Waste.API.Models
{
    public class WasteJourney : BaseEntity
    {
        

        public int WasteTypeId { get;set; }

        public int WasteSubTypeId { get;set; }

        public WasteType WasteType { get; set; }

        public WasteSubType WasteSubType { get; set; }
    }
}
