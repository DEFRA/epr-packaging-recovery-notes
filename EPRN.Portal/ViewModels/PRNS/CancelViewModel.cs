using System.ComponentModel.DataAnnotations;

namespace EPRN.Portal.ViewModels.PRNS
{
    public class CancelViewModel
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string CancelReason { get; set; }
    }
}