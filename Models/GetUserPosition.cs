using System;
using System.Collections.Generic;

namespace OnlineCourseManagement.Models;

public partial class GetUserPosition
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public Guid PositionId { get; set; }

    public string PositionName { get; set; } = null!;
}
