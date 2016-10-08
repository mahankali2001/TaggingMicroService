using System.Collections.Specialized;

namespace Persistence.Entities
{
    public interface IEmailAttributes
    {
        string FromAddress { get; set; }
        string ToAddress { get; set; }
        string CcAddress { get; set; }
        string BccAddress { get; set; }
        string Subject { get; set; }
        string SMTPServer { get; set; }
        string Template { get; set; }
        string TemplateFileLocation { get; set; }
        NameValueCollection TemplateNameValues { get; set; }
    }
}