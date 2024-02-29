using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Models
{
    public class LinkCertificateAccount
    {
        public int LinkCertId { get; set; }
        public int CourseId { get; set; }
        public string AccountId { get; set; }
        public string LinkCert { get; set; }

        public virtual Account? Account { get; set; }

        public virtual Course? Course { get; set; }
    }
}
