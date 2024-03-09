using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class OrderPaymentModel
    {
        public int OrderId { get; set; }
        public string AccountId { get; set; }

        public int CourseId { get; set; }

        public double? TotalPrice { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string? AccountName { get; set; }

        public string? Status { get; set; } = OrderStatusEnum.Pending.ToString();
    }
    
    public class AddOrderPaymentModel
    {
        public string AccountId { get; set; }

        public int CourseId { get; set; }
    }
}
