using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab4.Models
{
    public class Community
    {
        [Display(Name = "Registration Number")]
        [Required]
        public string ID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }

        public ICollection<CommunityMembership> CommunityMemberships { get; set; }
        public ICollection<Advertisement> Advertisements { get; set; }
    }
}