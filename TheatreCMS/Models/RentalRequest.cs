using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace TheatreCMS.Models
{
    public class RentalRequest
    {
        [Key]
        public int RentalRequestId { get; set; }
        public string ContactPerson { get; set; }
        public string Company { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime EndTime { get; set; }
        public string ProjectInfo { get; set; }
        public string Requests { get; set; }
        public byte[] Attachments { get; set; }
        public bool Accepted { get; set; }
        public bool ContractSigned { get; set; }

    }
}