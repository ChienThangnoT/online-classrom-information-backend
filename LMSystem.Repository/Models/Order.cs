using System;
using System.Collections.Generic;

namespace LMSystem.Repository.Models;

public partial class Order
{
    public string OrderId { get; set; }

    public string AccountId { get; set; }

    public string CourseId { get; set; }

    public double? TotalPrice { get; set; }

    public string PaymentMethod { get; set; }

    public string TransactionNo { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string CurrencyCode { get; set; }

    public string AccountName { get; set; }

    public string Status { get; set; }

    public virtual Account Account { get; set; }

    public virtual Course Course { get; set; }
}
