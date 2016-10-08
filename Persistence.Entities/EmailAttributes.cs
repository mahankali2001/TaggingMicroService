using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace Persistence.Entities.Common
{
     [System.ComponentModel.DataAnnotations.Schema.Table("EmailAttributes")]
    public class EmailAttributes : IEmailAttributes
    {
        [Key]
        public int Id { get; set; }

        public string ApplicationId { get; set; }

        #region IEmailAttributes Members

        public string FromAddress { get; set; }

        public string ToAddress { get; set; }

        public string CcAddress { get; set; }

        public string BccAddress { get; set; }

        public string Subject { get; set; }

        public string SMTPServer { get; set; }

        public string TemplateFileLocation { get; set; }

        public string Template { get; set; }

        [NotMapped]
        public NameValueCollection TemplateNameValues { get; set; }

        #endregion
    }
}