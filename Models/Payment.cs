using System;
using System.Collections.Generic;

namespace OnlineCourseManagement.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int PurchaseId { get; set; }

    public string Provider { get; set; } = null!;

    public string? TransactionId { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = null!;

    public int Status { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Purchase Purchase { get; set; } = null!;
}
