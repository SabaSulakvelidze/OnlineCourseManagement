using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models.Procedures;

namespace OnlineCourseManagement.Models
{
    public partial class OnlineCourseManagementDbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersByPosition>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null);
            });

            modelBuilder.Entity<UsersCourses>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null);
            });
        }
    }
}
