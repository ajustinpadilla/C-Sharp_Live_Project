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

        [StringLength(40, ErrorMessage = "Error. This field is limited to 40 characters.")]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
        
        [StringLength(100, ErrorMessage = "Error. This field is limited to 100 characters.")]
        public string Company { get; set; }
        
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }
        
        [Display(Name = "Project Info")]
        [StringLength(2000, ErrorMessage = "Error. This field is limited to 2000 characters.")]
        public string ProjectInfo { get; set; }
        
        [StringLength(1000, ErrorMessage = "Error. This field is limited to 1000 characters.")]
        public string Requests { get; set; }
        public byte[] Attachments { get; set; }
        public bool Accepted { get; set; }

        [Display(Name = "Contract Signed")]
        public bool ContractSigned { get; set; }
        
    


       }

    }





