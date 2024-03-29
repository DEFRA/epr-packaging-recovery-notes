﻿using EPRN.Common.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPRN.Common.Data.DataModels
{
    public class PrnHistory : IdBaseEntity
    {
        [ForeignKey("PackagingRecoveryNote")]
        public int PrnId { get; set; }

        public PrnStatus Status { get; set; }

        [MaxLength(200)]
        public string? Reason { get; set; }

        public DateTime Created { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public PackagingRecoveryNote PackagingRecoveryNote { get; set; }
    }
}
