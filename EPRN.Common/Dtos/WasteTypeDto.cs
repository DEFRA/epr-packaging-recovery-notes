namespace EPRN.Common.Dtos
{
    public class WasteTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public IEnumerable<WasteSubTypeDto> SubTypes { get; set; }
    }
}
