using System;
using System.Collections.Generic;

namespace OnlineCourseManagement.Models.Entities;

public partial class StudentsCourse
{
    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public DateTime? EnrolledAt { get; set; }

    public int Status { get; set; }

    public int? Grade { get; set; }

    public int? Progress { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User Student { get; set; } = null!;
}
