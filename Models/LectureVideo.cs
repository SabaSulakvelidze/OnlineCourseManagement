using System;
using System.Collections.Generic;

namespace OnlineCourseManagement.Models;

public partial class LectureVideo
{
    public int Id { get; set; }

    public int LectureId { get; set; }

    public string OriginalFileName { get; set; } = null!;

    public string VideoUrl { get; set; } = null!;

    public string PublicId { get; set; } = null!;

    public DateTime UploadedAt { get; set; }

    public virtual Lecture Lecture { get; set; } = null!;
}
