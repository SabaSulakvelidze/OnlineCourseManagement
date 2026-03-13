using System;
using System.Collections.Generic;

namespace OnlineCourseManagement.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public byte[]? ProfileImage { get; set; }

    public string? ProfileImageFileName { get; set; }

    public string? ProfileImageContentType { get; set; }
}
