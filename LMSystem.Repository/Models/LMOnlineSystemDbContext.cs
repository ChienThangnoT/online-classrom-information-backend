using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMSystem.Repository.Models;

public partial class LMOnlineSystemDbContext : IdentityDbContext<Account>
{
    public LMOnlineSystemDbContext()
    {
    }

    public LMOnlineSystemDbContext(DbContextOptions<LMOnlineSystemDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Account { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseCategory> CourseCategories { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<RatingCourse> RatingCourses { get; set; }

    public virtual DbSet<RegistrationCourse> RegistrationCourses { get; set; }

    public virtual DbSet<ReportProblem> ReportProblems { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<Step> Steps { get; set; }

    public virtual DbSet<StepCompleted> StepCompleteds { get; set; }

    public virtual DbSet<WishList> WishLists { get; set; }
    public virtual DbSet<Quiz> Quizzes { get; set; }
    public virtual DbSet<Question> Questions { get; set; }


    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=LAPTOP-9COIMEED;uid=sa;pwd=12345;database=LMOnlineSystemDB;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Account>().ToTable("Accounts");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Account");

            entity.ToTable("Accounts");
            entity.Property(e => e.Biography).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(155);
            entity.Property(e => e.LastName).HasMaxLength(155);
            entity.Property(e => e.ProfileImg).HasMaxLength(155);
            entity.Property(e => e.RefreshToken).HasMaxLength(155);
            entity.Property(e => e.RefreshTokenExpiryTime).HasColumnType("datetime");
            entity.Property(e => e.Sex).HasMaxLength(40);
            entity.Property(e => e.Status)
                .HasMaxLength(40)
                .IsFixedLength();
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CatgoryId);

            entity.ToTable("Category");

            entity.Property(e => e.Description)
                .HasMaxLength(450);
            entity.Property(e => e.Name)
                .HasMaxLength(155);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId);

            entity.ToTable("Course");

            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(450)
                .HasColumnName("ImageURL");
            entity.Property(e => e.PublicAt).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("Title");
            entity.Property(e => e.TotalDuration)
                .HasMaxLength(155);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.VideoPreviewUrl)
                .HasMaxLength(450)
                .HasColumnName("VideoPreviewURL");
        });

        modelBuilder.Entity<CourseCategory>(entity =>
        {
            entity.HasKey(e => e.CourseCategoryId);
            entity.ToTable("CourseCategory");

            entity.HasIndex(e => e.CategoryId, "IX_CourseCategory_CategoryId");

            entity.HasIndex(e => e.CourseId, "IX_CourseCategory_CourseId");

            entity.Property(e => e.CategoryId);
            entity.Property(e => e.CourseId);

            entity.HasOne(d => d.Category).WithMany(p => p.CourseCategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_CourseCategory_Category");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseCategories)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_CourseCategory_Course");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId);
            entity.ToTable("Notification");

            entity.HasIndex(e => e.AccountId, "IX_Notification_AccountId");

            entity.Property(e => e.AccountId);
            entity.Property(e => e.Message)
                .HasMaxLength(450);
            entity.Property(e => e.SendDate).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Notification_Accounts");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId);
            entity.ToTable("Order");

            entity.HasIndex(e => e.AccountId, "IX_Order_AccountId");

            entity.HasIndex(e => e.CourseId, "IX_Order_CourseId");

            entity.Property(e => e.AccountId);
            entity.Property(e => e.AccountName)
                .HasMaxLength(155);
            entity.Property(e => e.CourseId);
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(55);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(155);
            entity.Property(e => e.Status)
                .HasMaxLength(55);
            entity.Property(e => e.TransactionNo)
                .HasMaxLength(155);

            entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Order_Accounts");

            entity.HasOne(d => d.Course).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_Order_Course");
        });

        modelBuilder.Entity<RatingCourse>(entity =>
        {
            entity.HasKey(e => e.RatingId);

            entity.ToTable("RatingCourse");

            entity.HasIndex(e => e.RegistrationId, "IX_RatingCourse_RegistrationId");

            entity.Property(e => e.CommentContent)
                .HasMaxLength(255);
            entity.Property(e => e.RatingDate).HasColumnType("datetime");
            entity.Property(e => e.RegistrationId);

            entity.HasOne(d => d.Registration).WithMany(p => p.RatingCourses)
                .HasForeignKey(d => d.RegistrationId)
                .HasConstraintName("FK_RatingCourse_RegistrationCourse");
        });

        modelBuilder.Entity<RegistrationCourse>(entity =>
        {
            entity.HasKey(e => e.RegistrationId);

            entity.ToTable("RegistrationCourse");

            entity.HasIndex(e => e.AccountId, "IX_RegistrationCourse_AccountId");

            entity.HasIndex(e => e.CourseId, "IX_RegistrationCourse_CourseId");

            entity.Property(e => e.AccountId);
            entity.Property(e => e.CourseId);
            entity.Property(e => e.EnrollmentDate).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.RegistrationCourses)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_RegistrationCourse_Accounts");

            entity.HasOne(d => d.Course).WithMany(p => p.RegistrationCourses)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_RegistrationCourse_Course");
        });

        modelBuilder.Entity<ReportProblem>(entity =>
        {
            entity.HasKey(e => e.ReportId);

            entity.ToTable("ReportProblem");

            entity.HasIndex(e => e.AccountId, "IX_ReportProblem_AccountId");

            entity.Property(e => e.AccountId).IsRequired();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(450);
            entity.Property(e => e.ProcessingDate).HasColumnType("datetime");
            entity.Property(e => e.ReportStatus)
                .HasMaxLength(50);
            entity.Property(e => e.Title)
                .HasMaxLength(155);
            entity.Property(e => e.Type)
                .HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.ReportProblems)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_ReportProblem_Accounts");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.SectionId);
            entity.ToTable("Section");

            entity.HasIndex(e => e.CourseId, "IX_Section_CourseId");

            entity.Property(e => e.CourseId);
            entity.Property(e => e.Title)
                .HasMaxLength(155);

            entity.HasOne(d => d.Course).WithMany(p => p.Sections)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_Section_Course");
        });

        modelBuilder.Entity<Step>(entity =>
        {
            entity.HasKey(e => e.StepId);
            entity.ToTable("Step");

            entity.HasIndex(e => e.SectionId, "IX_Step_SectionId");

            entity.Property(e => e.SectionId);
            entity.Property(e => e.StepDescription)
                .HasMaxLength(450);
            entity.Property(e => e.Title)
                .HasMaxLength(155);
            entity.Property(e => e.VideoUrl)
                .HasMaxLength(255);

            entity.HasOne(d => d.Section).WithMany(p => p.Steps)
                .HasForeignKey(d => d.SectionId)
                .HasConstraintName("FK_Step_Section");
        });

        modelBuilder.Entity<StepCompleted>(entity =>
        {
            entity.HasKey(e => e.CompletedStepId);

            entity.ToTable("StepCompleted");

            entity.HasIndex(e => e.RegistrationId, "IX_StepCompleted_RegistrationId");

            entity.Property(e => e.DateCompleted).HasColumnType("datetime");
            entity.Property(e => e.RegistrationId);

            entity.HasOne(d => d.Registration).WithMany(p => p.StepCompleteds).HasForeignKey(d => d.RegistrationId);
        });

        modelBuilder.Entity<WishList>(entity =>
        {
            entity.HasKey(e => e.WishListId);
            entity.ToTable("WishList");

            entity.HasIndex(e => e.AccountId, "IX_WishList_AccountId");

            entity.HasIndex(e => e.CourseId, "IX_WishList_CourseId");

            entity.Property(e => e.AccountId);
            entity.Property(e => e.CourseId);

            entity.HasOne(d => d.Account).WithMany(p => p.WishLists)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_WishList_Accounts");

            entity.HasOne(d => d.Course).WithMany(p => p.WishLists)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_WishList_Course");
        });
        
        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.QuizId);
            entity.ToTable("Quiz");


            entity.HasIndex(e => e.StepId, "IX_Quiz_StepId");
            
            entity.Property(e => e.Title).HasMaxLength(150);

            entity.Property(e => e.Description).HasMaxLength(250);


            entity.HasOne(d => d.Step).WithMany(p => p.Quizzes)
                .HasForeignKey(d => d.StepId)
                .HasConstraintName("FK_Quiz_Step");
        });
        
        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId);
            entity.ToTable("Question");


            entity.HasIndex(e => e.QuizId, "IX_Question_QuizId");
            
            entity.Property(e => e.Title).HasMaxLength(250);
            entity.Property(e => e.Anwser1).HasMaxLength(250);
            entity.Property(e => e.Anwser2).HasMaxLength(250);
            entity.Property(e => e.Anwser3).HasMaxLength(250);
            entity.Property(e => e.Anwser4).HasMaxLength(250);
            entity.Property(e => e.AnwserCorrect).HasMaxLength(250);

            entity.HasOne(d => d.Quiz).WithMany(p => p.Question)
                .HasForeignKey(d => d.QuizId)
                .HasConstraintName("FK_Question_Quiz");
        });

        //base.OnModelCreating(modelBuilder);
        //OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
