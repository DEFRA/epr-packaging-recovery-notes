using EPRN.Common.Enums;

namespace EPRN.Common.Dtos
{
    public class NoteDto
    {
        public int Id { get; set; }

        public string Note { get; set; }

        public Category WasteCategory { get; set; }
    }
}
