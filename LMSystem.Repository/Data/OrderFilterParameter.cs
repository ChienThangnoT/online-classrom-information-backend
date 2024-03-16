using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class OrderFilterParameter
    {
        public string? Status { get; set; } = string.Empty;
        public string? AccountId { get; set; } = string.Empty;
    }
}
