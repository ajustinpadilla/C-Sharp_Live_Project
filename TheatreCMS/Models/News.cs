using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheatreCMS.Models
{
    public class News
    {
        [Key]
        public int NewsId { get; set; }
        public string Headline { get; set; }
        public string Content { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastSaveDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? EmailDate { get; set; }
        public bool Hidden { get; set; }
    }
}