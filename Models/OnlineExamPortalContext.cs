using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Quizzy_Main.Models;

public partial class OnlineExamPortalContext : DbContext
{
    public OnlineExamPortalContext()
    {
    }

    public OnlineExamPortalContext(DbContextOptions<OnlineExamPortalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=enosislearning.database.windows.net;Database=OnlineExamPortal;User Id=enosis;Password=sisone@123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.ExamId).HasName("PK__Exam__297521A7B1CB82CC");

            entity.ToTable("Exam");

            entity.Property(e => e.ExamId).HasColumnName("ExamID");
            entity.Property(e => e.ExamEndDateTime).HasColumnType("datetime");
            entity.Property(e => e.ExamStartDateTime).HasColumnType("datetime");
            entity.Property(e => e.Finalscore).HasColumnName("FINALSCORE");
            entity.Property(e => e.TopicId).HasColumnName("TopicID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Topic).WithMany(p => p.Exams)
                .HasForeignKey(d => d.TopicId)
                .HasConstraintName("FK__Exam__TopicID__656C112C");

            entity.HasOne(d => d.User).WithMany(p => p.Exams)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Exam__UserID__6477ECF3");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06F8CD6DE64B7");

            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.CorrectOption)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DifficultyLevel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Option1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Option2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Option3)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Option4)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Question1)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Question");
            entity.Property(e => e.TopicId).HasColumnName("TopicID")
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Topic).WithMany(p => p.Questions)
                .HasForeignKey(d => d.TopicId)
                .HasConstraintName("FK__Questions__Topic__66603565");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.ResultId).HasName("PK__Result__97690228492064DA");

            entity.ToTable("Result");

            entity.Property(e => e.ResultId).HasColumnName("ResultID");
            entity.Property(e => e.CorrectOption)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExamId).HasColumnName("ExamID");
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.Result1).HasColumnName("Result");
            entity.Property(e => e.SelectedOption)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Exam).WithMany(p => p.Results)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("FK__Result__ExamID__6754599E");

            entity.HasOne(d => d.Question).WithMany(p => p.Results)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__Result__Question__68487DD7");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.TopicId).HasName("PK__Topic__022E0F7DBD852587");

            entity.ToTable("Topic");

            entity.Property(e => e.TopicId).HasColumnName("TopicID");
            entity.Property(e => e.TopicName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC87D685E8");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserRole)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
