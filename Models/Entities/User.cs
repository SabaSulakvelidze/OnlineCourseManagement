using System;
using System.Collections.Generic;

namespace OnlineCourseManagement.Models.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public byte[]? ProfileImage { get; set; }

    public string? ProfileImageFileName { get; set; }

    public string? ProfileImageContentType { get; set; }

    public string UserPassword { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public virtual ICollection<LecturersCourse> LecturersCourses { get; set; } = new List<LecturersCourse>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual ICollection<StudentsCourse> StudentsCourses { get; set; } = new List<StudentsCourse>();

    public virtual ICollection<UsersPosition> UsersPositions { get; set; } = new List<UsersPosition>();
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
