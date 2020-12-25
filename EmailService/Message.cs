using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmailService
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Attachment { get; set; }

        public Message(IEnumerable<string> to, string subject, string content, string attachment)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(address => new MailboxAddress(address))) ;
            Subject = subject;
            Content = content;
            Attachment = attachment;
        }
    }
}
