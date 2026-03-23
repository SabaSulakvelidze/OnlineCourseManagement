using System;
using System.Collections.Generic;

namespace OnlineCourseManagement.Models;

public partial class Course
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public decimal Price { get; set; }

    public string PriceCurrency { get; set; } = null!;

    public virtual ICollection<LecturersCourse> LecturersCourses { get; set; } = new List<LecturersCourse>();

    public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();

    public virtual ICollection<StudentsCourse> StudentsCourses { get; set; } = new List<StudentsCourse>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
