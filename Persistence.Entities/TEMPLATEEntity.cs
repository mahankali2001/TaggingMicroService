using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
   // [Table("TemplateEntity")]
    public class TEMPLATEEntity
    {
        [Key]
        public int Id { get; set; }

        public string Data { get; set; }
    }
}
