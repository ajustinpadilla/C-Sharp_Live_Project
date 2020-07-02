using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheatreCMS.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }          //primary key
        [Display(Name = "From")]
        [ForeignKey("Sender")]
        public string SenderId { get; set; }           //user.Id of sender
        
        [Display(Name ="To")]
        [ForeignKey("Recipient")]                       //currently, SenderId and RecipientId are nullable because of Cascade errors if set to required, fix with Fluent API
        public string RecipientId { get; set; }         //user.Id of recipient
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy h:mm tt}")]
        public DateTime SentTime { get; set; }         //datetime when message was sent
        public DateTime? IsViewed { get; set; }         //null if unread, datetime of when it was opened
        public int? ParentId { get; set; }              //used for replies
        public string Subject { get; set; }             //subject of message
        public string Body { get; set; }                //body of message

        public virtual ApplicationUser Sender { get; set; } //nav property to Sender (as User)
        public virtual ApplicationUser Recipient { get; set; }  //nav property to Recipient (as User)

    }
}