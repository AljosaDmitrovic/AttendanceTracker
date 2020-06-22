using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TrackAttendanceAPI.Models
{
    public partial class TrackAttendanceContext : DbContext
    {
        public TrackAttendanceContext()
        {
        }

        public TrackAttendanceContext(DbContextOptions<TrackAttendanceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointments> Appointments { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<Lectures> Lectures { get; set; }
        public virtual DbSet<Modules> Modules { get; set; }
        public virtual DbSet<Rooms> Rooms { get; set; }
        public virtual DbSet<Semesters> Semesters { get; set; }
        public virtual DbSet<SignEntries> SignEntries { get; set; }
        public virtual DbSet<StudentGroups> StudentGroups { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=TrackAttendanceDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointments>(entity =>
            {
                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appointments_ToGroups");
            });

            modelBuilder.Entity<Groups>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.LectureId).HasColumnName("LectureID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Lecture)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.LectureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Groups_ToLectures");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_Groups_ToRooms");
            });

            modelBuilder.Entity<Lectures>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SemesterId).HasColumnName("SemesterID");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Lectures)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK_Lectures_ToSemesters");
            });

            modelBuilder.Entity<Modules>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Rooms>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Semesters>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SignEntries>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.AppointmentId, e.Year });

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");

                entity.Property(e => e.Week1).HasMaxLength(100);

                entity.Property(e => e.Week10).HasMaxLength(100);

                entity.Property(e => e.Week11).HasMaxLength(100);

                entity.Property(e => e.Week12).HasMaxLength(100);

                entity.Property(e => e.Week13).HasMaxLength(100);

                entity.Property(e => e.Week14).HasMaxLength(100);

                entity.Property(e => e.Week15).HasMaxLength(100);

                entity.Property(e => e.Week2).HasMaxLength(100);

                entity.Property(e => e.Week3).HasMaxLength(100);

                entity.Property(e => e.Week4).HasMaxLength(100);

                entity.Property(e => e.Week5).HasMaxLength(100);

                entity.Property(e => e.Week6).HasMaxLength(100);

                entity.Property(e => e.Week7).HasMaxLength(100);

                entity.Property(e => e.Week8).HasMaxLength(100);

                entity.Property(e => e.Week9).HasMaxLength(100);

                entity.HasOne(d => d.Appointment)
                    .WithMany(p => p.SignEntries)
                    .HasForeignKey(d => d.AppointmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SignEntries_ToAppintments");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SignEntries)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SignEntries_ToUsers");
            });

            modelBuilder.Entity<StudentGroups>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.GroupId });

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.StudentGroups)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentGroups_ToGroups");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.StudentGroups)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentGroups_ToUsers");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IndexNumber).HasMaxLength(50);

                entity.Property(e => e.LoginName).HasMaxLength(40);

                entity.Property(e => e.ModuleId).HasColumnName("ModuleID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PasswordHash).HasMaxLength(128);

                entity.Property(e => e.PasswordSalt).HasMaxLength(128);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.SignDate).HasColumnType("date");

                entity.Property(e => e.Surname).HasMaxLength(50);

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ModuleId)
                    .HasConstraintName("FK_Users_ToModules");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
