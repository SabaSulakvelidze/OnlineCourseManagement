using System;
using System.Collections.Generic;

namespace OnlineCourseManagement.Models;

public partial class UsersPosition
{
    public Guid Id { get; set; }

    public int UsersId { get; set; }

    public Guid PositionId { get; set; }

    public virtual Position Position { get; set; } = null!;

    public virtual User Users { get; set; } = null!;
}
