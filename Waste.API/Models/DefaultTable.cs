using System.ComponentModel.DataAnnotations;

namespace WasteManagement.API.Models
{
    public class DefaultTable : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
