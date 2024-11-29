using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SchoolApp.Data;

public partial class Mvc6DbContext : DbContext
{
    public Mvc6DbContext()
    {
    }

    public Mvc6DbContext(DbContextOptions<Mvc6DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Teacher> Teachers { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Courses__3214EC0727AF97D1");

            entity.HasIndex(e => e.Description, "IX_Courses_Description");

            entity.Property(e => e.Description).HasMaxLength(50);

            entity.HasOne(d => d.Teacher).WithMany(p => p.Courses)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK_Courses_Teachers");

            entity.HasMany(d => d.Students).WithMany(p => p.Courses)
                .UsingEntity<Dictionary<string, object>>(
                    "CoursesStudent",
                    r => r.HasOne<Student>().WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CoursesStudents_StudentId"),
                    l => l.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CoursesStudents_CourseId"),
                    j =>
                    {
                        j.HasKey("CourseId", "StudentId");
                        j.ToTable("CoursesStudents");
                        j.HasIndex(new[] { "CourseId" }, "IX_CoursesStudents_CourseId");
                        j.HasIndex(new[] { "StudentId" }, "IX_CoursesStudents_StudentId");
                    });
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3214EC07363651DC");

            entity.HasIndex(e => e.Am, "IX_Students_AM").IsUnique();

            entity.HasIndex(e => e.Institution, "IX_Students_Institution");

            entity.HasIndex(e => e.UserId, "IX_Students_UserId").IsUnique();

            entity.Property(e => e.Am)
                .HasMaxLength(10)
                .HasColumnName("AM");
            entity.Property(e => e.Department).HasMaxLength(50);
            entity.Property(e => e.Institution).HasMaxLength(50);

            entity.HasOne(d => d.User).WithOne(p => p.Student)
                .HasForeignKey<Student>(d => d.UserId)
                .HasConstraintName("FK_Students_Users");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teachers__3214EC07FE6B468B");

            entity.HasIndex(e => e.Institution, "IX_Teachers_Institution");

            entity.HasIndex(e => e.UserId, "IX_Teachers_UserId").IsUnique();

            entity.Property(e => e.Institution).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);

            entity.HasOne(d => d.User).WithOne(p => p.Teacher)
                .HasForeignKey<Teacher>(d => d.UserId)
                .HasConstraintName("FK_Teachers_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07ACE2DBFD");

            entity.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();

            entity.HasIndex(e => e.Username, "IX_Users_Username").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Firstname).HasMaxLength(50);
            entity.Property(e => e.Lastname).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(60);
            entity.Property(e => e.UserRole).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
