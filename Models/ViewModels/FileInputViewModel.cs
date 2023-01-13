using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace lab4.Models.ViewModels
{
    public class FileInputViewModel
    {
        public string CommunityId { get; set; }
        public string CommunityTitle { get; set; }
        public IFormFile File { get; set; }
    }
}