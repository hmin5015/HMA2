using System;
using System.ComponentModel.DataAnnotations;

namespace HMA2.Models
{
    public partial class BaseEntity
    {
        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [Timestamp]
        public byte[] Rowstamp { get; set; }
    }
}