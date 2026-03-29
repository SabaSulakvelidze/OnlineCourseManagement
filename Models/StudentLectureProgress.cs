using System;
using System.Collections.Generic;

namespace OnlineCourseManagement.Models;

public partial class StudentLectureProgress
{
    public int StudentId { get; set; }

    public int LectureId { get; set; }

    public DateTime? CompletedAt { get; set; }

    public virtual Lecture Lecture { get; set; } = null!;

    public virtual User Student { get; set; } = null!;
}
