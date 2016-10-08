using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table("Tag")]
    public class Tag
    {
        [Key]
        public int TagId { get; set; }

        public string TagName { get; set; }

        public int UserId { get; set; }

        public int ApplicationId { get; set; }

        public DateTime CreateDatetime { get; set; }

        public DateTime UpdatedDateTime { get; set; }

        public bool? IsActive { get; set; }

        [NotMapped]
        public string ErrorCode { get; set; }

        [NotMapped]
        public bool IsNew { get; set; }
    }
}
