using EPRN.Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Portal.ViewModels.Waste
{
    public class NoteViewModel
    {
        public int JourneyId { get; set; }
//        public string WasteType { get; set; }

        [StringLength(200, ErrorMessageResourceName = "ErrorMessageTooLong", ErrorMessageResourceType = typeof(NoteResources))]
        public string NoteContent { get; set; }

    }
}