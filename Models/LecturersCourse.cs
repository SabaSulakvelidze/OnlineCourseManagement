using System;
using System.Collections.Generic;

namespace OnlineCourseManagement.Models;

public partial class LecturersCourse
{
    public int LecturerId { get; set; }

    public int CourseId { get; set; }

    public DateTime AssignedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User Lecturer { get; set; } = null!;
}
