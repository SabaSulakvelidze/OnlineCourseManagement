using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersPosition> UsersPositions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=OnlineCourseManagementDB;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Courses__3214EC079C42C126");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(2000);
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

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Position__3214EC07D9D8112D");

            entity.ToTable("Position");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.PositionName)
                .HasMaxLength(50)
                .IsUnicode(false);
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
            entity.Property(e => e.UserPassword)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        modelBuilder.Entity<UsersPosition>(entity =>
        {
            entity.HasKey(e => new { e.UsersId, e.PositionId }).HasName("PK__UsersPos__254209C504877765");

            entity.ToTable("UsersPosition");

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
