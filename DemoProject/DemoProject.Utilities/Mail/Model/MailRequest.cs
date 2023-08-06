using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Utilities.Mail
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<AttachmentElement> Attachments { get; set; }
    }

    public class AttachmentElement
    {
        public Byte[] fileBytes { get; set; }
        public string fileName { get; set; }
    }
}
