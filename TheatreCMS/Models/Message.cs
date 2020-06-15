using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheatreCMS.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime? IsViewed { get; set; }
        public int? ParentId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

    }
}