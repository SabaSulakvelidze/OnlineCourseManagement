using System;
using System.Collections.Generic;

namespace OnlineCourseManagement.Models;

public partial class PurchaseCourse
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CourseId { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; } = null!;

    public int Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
