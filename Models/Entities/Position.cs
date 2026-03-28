using System;
using System.Collections.Generic;

namespace OnlineCourseManagement.Models.Entities;

public partial class Position
{
    public Guid Id { get; set; }

    public string PositionName { get; set; } = null!;

    public virtual ICollection<UsersPosition> UsersPositions { get; set; } = new List<UsersPosition>();
}
