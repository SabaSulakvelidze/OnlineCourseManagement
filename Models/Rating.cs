using System;
using System.Collections.Generic;

namespace OnlineCourseManagement.Models.Entities;

public partial class Rating
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CourseId { get; set; }

    public string? Review { get; set; }

    public int Value { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
