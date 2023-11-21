namespace EPRN.Common.Models
{
    public class WasteTypeDto : DtoIdBase
    {
        public string Name { get; set; } = string.Empty;

        public IEnumerable<WasteSubTypeDto> SubTypes { get; set; }
    }
}