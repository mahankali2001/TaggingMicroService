using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table("TaggedObject")]
    public class TaggedObject
    {
        [Key]
        public int TaggedObjectId { get; set; }

        public int TagId { get; set; }

        public string ObjectTextId { get; set; }

        public int ObjectId { get; set; }

        public DateTime CreateDatetime { get; set; }

        public DateTime UpdatedDateTime { get; set; }

        public bool? IsActive { get; set; }

        [NotMapped]
        public int ApplicationId { get; set; }
    }
}
