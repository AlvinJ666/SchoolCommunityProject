using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab4.Models.ViewModels
{
    public class StudentMembershipViewModel
    {
        public Student Student { get; set; }
        public IEnumerable<StudentViewModel> Memberships { get; set; }
    }
}
