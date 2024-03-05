using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class SendRequestModel
    {

        public string AccountId { get; set; }

        public string? Type { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }
    }
}
