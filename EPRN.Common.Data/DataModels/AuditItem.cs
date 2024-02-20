using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPRN.Common.Data.DataModels
{
    public class AuditItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Table { get; set; } // on what table did the change occur

        [Required]
        public string Action { get; set; } // what action occurred (Add, Modfify, etc)

        public int? EntityId {get;set; } // primary key of the record. Nullable as may be a new record

        [Required]
        public string Username { get; set; }

        public DateTime Timestamp { get; set; }

        public ICollection<AuditChange> Changes { get; set; }
    }

    public class AuditChange
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("AuditItem")]
        public int AuditItemId { get; set; }

        public string Field { get; set; } // on what field did the change occur (if any)

        public string OriginalValue { get; set; }

        public string NewValue { get; set; }

        public AuditItem AuditItem { get; set; }
    }
}
