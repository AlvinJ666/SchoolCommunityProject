using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab4.Models
{
    public class Advertisement
    {
        public int Id
        {
            get; set;
        }


        [Required]
        [DisplayName("File Name")]
        public string FileName
        {
            get;
            set;
        }

        [Required]
        [Url]
        public string Url
        {
            get;
            set;
        }
    }
}
