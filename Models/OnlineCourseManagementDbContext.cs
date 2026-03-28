using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models.Entities;
using OnlineCourseManagement.Models.Procedures;

namespace OnlineCourseManagement.Models;

public partial class OnlineCourseManagementDbContext : DbContext
{
    public OnlineCourseManagementDbContext()
    {
    }

    public OnlineCourseManagementDbContext(DbContextOptions<OnlineCourseManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Lecture> Lectures { get; set; }

    public virtual DbSet<LectureVideo> LectureVideos { get; set; }

    public virtual DbSet<LecturersCourse> LecturersCourses { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<StudentsCourse> StudentsCourses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersByPosition> UsersByPosition { get; set; }

    public virtual DbSet<UsersCourses> UsersCourses { get; set; }

    public virtual DbSet<UsersPosition> UsersPositions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=OnlineCourseManagementDB;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UsersByPosition>().HasNoKey().ToView(null);

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Courses__3214EC079C42C126");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PriceCurrency)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Lecture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Lectures__3214EC0749266654");

            entity.HasIndex(e => e.CourseId, "IX_Lectures_CourseId");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Course).WithMany(p => p.Lectures)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_Course_Lecture");
        });

        modelBuilder.Entity<LectureVideo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LectureV__3214EC0765EDDA64");

            entity.HasIndex(e => e.LectureId, "IX_LectureVideos_LectureId");

            entity.Property(e => e.OriginalFileName).HasMaxLength(250);
            entity.Property(e => e.PublicId).HasMaxLength(1000);
            entity.Property(e => e.UploadedAt).HasColumnType("datetime");
            entity.Property(e => e.VideoUrl).HasMaxLength(1000);

            entity.HasOne(d => d.Lecture).WithMany(p => p.LectureVideos)
                .HasForeignKey(d => d.LectureId)
                .HasConstraintName("FK_Lecture_LectureVideos");
        });

        modelBuilder.Entity<LecturersCourse>(entity =>
        {
            entity.HasKey(e => new { e.LecturerId, e.CourseId }).HasName("PK__Lecturer__36EA6E27A308D3F4");

            entity.HasIndex(e => e.CourseId, "IX_LecturersCourses_CourseId");

            entity.Property(e => e.AssignedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Course).WithMany(p => p.LecturersCourses)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Lecturers__Cours__0A9D95DB");

            entity.HasOne(d => d.Lecturer).WithMany(p => p.LecturersCourses)
                .HasForeignKey(d => d.LecturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Lecturers__Lectu__09A971A2");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Position__3214EC07D9D8112D");

            entity.ToTable("Position");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.PositionName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Purchase__3214EC07CA029FCE");

            entity.HasIndex(e => e.CourseId, "IX_Purchases_CourseId");

            entity.HasIndex(e => e.UserId, "IX_Purchases_UserId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Course).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Purchases__Cours__17F790F9");

            entity.HasOne(d => d.User).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Purchases__UserI__17036CC0");
        });

        modelBuilder.Entity<StudentsCourse>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.CourseId }).HasName("PK__Students__5E57FC837298EC71");

            entity.HasIndex(e => e.CourseId, "IX_StudentsCourses_CourseId");

            entity.Property(e => e.EnrolledAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("EnrolledAT");
            entity.Property(e => e.Grade).HasDefaultValue(0);
            entity.Property(e => e.Progress).HasDefaultValue(0);

            entity.HasOne(d => d.Course).WithMany(p => p.StudentsCourses)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentsC__Cours__06CD04F7");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentsCourses)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentsC__Stude__05D8E0BE");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC079E27A083");

            entity.HasIndex(e => e.Email, "UQ_Users_Email").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ProfileImageContentType).HasMaxLength(50);
            entity.Property(e => e.ProfileImageFileName).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        modelBuilder.Entity<UsersPosition>(entity =>
        {
            entity.HasKey(e => new { e.UsersId, e.PositionId }).HasName("PK__UsersPos__254209C504877765");

            entity.ToTable("UsersPosition");

            entity.HasIndex(e => e.PositionId, "IX_UsersPosition_PositionId");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Position).WithMany(p => p.UsersPositions)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsersPosi__Posit__797309D9");

            entity.HasOne(d => d.Users).WithMany(p => p.UsersPositions)
                .HasForeignKey(d => d.UsersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsersPosi__Users__787EE5A0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
