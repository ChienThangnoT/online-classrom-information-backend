﻿// <auto-generated />
using System;
using LMSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LMSystem.Repository.Migrations
{
    [DbContext(typeof(LMOnlineSystemDbContext))]
    [Migration("20240119103035_AddIdentityToDB")]
    partial class AddIdentityToDB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LMSystem.Repository.Models.Account", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Biography")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateOnly?>("BirthDate")
                        .HasColumnType("date");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(155)
                        .HasColumnType("nvarchar(155)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(155)
                        .HasColumnType("nvarchar(155)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ProfileImg")
                        .IsRequired()
                        .HasMaxLength(155)
                        .HasColumnType("nvarchar(155)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasMaxLength(155)
                        .HasColumnType("nvarchar(155)");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nchar(40)")
                        .IsFixedLength();

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("LMSystem.Repository.Models.Category", b =>
                {
                    b.Property<string>("CatgoryId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(155)
                        .HasColumnType("nvarchar(155)");

                    b.HasKey("CatgoryId");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("LMSystem.Repository.Models.Course", b =>
                {
                    b.Property<string>("CourseId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool?>("CourseIsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("ImageURL");

                    b.Property<bool?>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<string>("KnowdledgeDescription")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.Property<DateTime?>("PublicAt")
                        .HasColumnType("datetime");

                    b.Property<double?>("SalesCampaign")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("title");

                    b.Property<string>("TotalDuration")
                        .IsRequired()
                        .HasMaxLength(155)
                        .HasColumnType("nvarchar(155)");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime");

                    b.Property<string>("VideoPreviewUrl")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("VideoPreviewURL");

                    b.HasKey("CourseId");

                    b.ToTable("Course", (string)null);
                });

            modelBuilder.Entity("LMSystem.Repository.Models.CourseCategory", b =>
                {
                    b.Property<string>("CourseCategoryId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CourseId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CourseCategoryId");

                    b.HasIndex(new[] { "CategoryId" }, "IX_CourseCategory_CategoryId");

                    b.HasIndex(new[] { "CourseId" }, "IX_CourseCategory_CourseId");

                    b.ToTable("CourseCategory", (string)null);
                });

            modelBuilder.Entity("LMSystem.Repository.Models.Notification", b =>
                {
                    b.Property<string>("NotificationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AccountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("SendDate")
                        .HasColumnType("datetime");

                    b.HasKey("NotificationId");

                    b.HasIndex(new[] { "AccountId" }, "IX_Notification_AccountId");

                    b.ToTable("Notification", (string)null);
                });

            modelBuilder.Entity("LMSystem.Repository.Models.Order", b =>
                {
                    b.Property<string>("OrderId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AccountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasMaxLength(155)
                        .HasColumnType("nvarchar(155)");

                    b.Property<string>("CourseId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasMaxLength(55)
                        .HasColumnType("nvarchar(55)");

                    b.Property<DateTime?>("PaymentDate")
                        .HasColumnType("datetime");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasMaxLength(155)
                        .HasColumnType("nvarchar(155)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(55)
                        .HasColumnType("nvarchar(55)");

                    b.Property<double?>("TotalPrice")
                        .HasColumnType("float");

                    b.Property<string>("TransactionNo")
                        .IsRequired()
                        .HasMaxLength(155)
                        .HasColumnType("nvarchar(155)");

                    b.HasKey("OrderId");

                    b.HasIndex(new[] { "AccountId" }, "IX_Order_AccountId");

                    b.HasIndex(new[] { "CourseId" }, "IX_Order_CourseId");

                    b.ToTable("Order", (string)null);
                });

            modelBuilder.Entity("LMSystem.Repository.Models.RatingCourse", b =>
                {
                    b.Property<string>("RatingId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CommentContent")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool?>("IsRatingStatus")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("RatingDate")
                        .HasColumnType("datetime");

                    b.Property<string>("RegistrationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("RatingId");

                    b.HasIndex(new[] { "RegistrationId" }, "IX_RatingCourse_RegistrationId");

                    b.ToTable("RatingCourse", (string)null);
                });

            modelBuilder.Entity("LMSystem.Repository.Models.RegistrationCourse", b =>
                {
                    b.Property<string>("RegistrationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AccountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CourseId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("EnrollmentDate")
                        .HasColumnType("datetime");

                    b.Property<bool?>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<double?>("LearningProgress")
                        .HasColumnType("float");

                    b.HasKey("RegistrationId");

                    b.HasIndex(new[] { "AccountId" }, "IX_RegistrationCourse_AccountId");

                    b.HasIndex(new[] { "CourseId" }, "IX_RegistrationCourse_CourseId");

                    b.ToTable("RegistrationCourse", (string)null);
                });

            modelBuilder.Entity("LMSystem.Repository.Models.ReportProblem", b =>
                {
                    b.Property<string>("ReportId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AccountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("ProcessingDate")
                        .HasColumnType("datetime");

                    b.Property<string>("ReportStatus")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(155)
                        .HasColumnType("nvarchar(155)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ReportId");

                    b.HasIndex(new[] { "AccountId" }, "IX_ReportProblem_AccountId");

                    b.ToTable("ReportProblem", (string)null);
                });

            modelBuilder.Entity("LMSystem.Repository.Models.Section", b =>
                {
                    b.Property<string>("SectionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CourseId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("Position")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(155)
                        .HasColumnType("nvarchar(155)");

                    b.HasKey("SectionId");

                    b.HasIndex(new[] { "CourseId" }, "IX_Section_CourseId");

                    b.ToTable("Section", (string)null);
                });

            modelBuilder.Entity("LMSystem.Repository.Models.Step", b =>
                {
                    b.Property<string>("StepId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("Duration")
                        .HasColumnType("int");

                    b.Property<int?>("Position")
                        .HasColumnType("int");

                    b.Property<string>("SectionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("StepDescription")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(155)
                        .HasColumnType("nvarchar(155)");

                    b.Property<string>("VideoUrl")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("StepId");

                    b.HasIndex(new[] { "SectionId" }, "IX_Step_SectionId");

                    b.ToTable("Step", (string)null);
                });

            modelBuilder.Entity("LMSystem.Repository.Models.StepCompleted", b =>
                {
                    b.Property<string>("CompletedStepId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("DateCompleted")
                        .HasColumnType("datetime");

                    b.Property<string>("RegistrationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CompletedStepId");

                    b.HasIndex(new[] { "RegistrationId" }, "IX_StepCompleted_RegistrationId");

                    b.ToTable("StepCompleted", (string)null);
                });

            modelBuilder.Entity("LMSystem.Repository.Models.WishList", b =>
                {
                    b.Property<string>("WishListId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AccountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CourseId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("WishListId");

                    b.HasIndex(new[] { "AccountId" }, "IX_WishList_AccountId");

                    b.HasIndex(new[] { "CourseId" }, "IX_WishList_CourseId");

                    b.ToTable("WishList", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("LMSystem.Repository.Models.CourseCategory", b =>
                {
                    b.HasOne("LMSystem.Repository.Models.Category", "Category")
                        .WithMany("CourseCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_CourseCategory_Category");

                    b.HasOne("LMSystem.Repository.Models.Course", "Course")
                        .WithMany("CourseCategories")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_CourseCategory_Course");

                    b.Navigation("Category");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("LMSystem.Repository.Models.Notification", b =>
                {
                    b.HasOne("LMSystem.Repository.Models.Account", "Account")
                        .WithMany("Notifications")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Notification_Accounts");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("LMSystem.Repository.Models.Order", b =>
                {
                    b.HasOne("LMSystem.Repository.Models.Account", "Account")
                        .WithMany("Orders")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Order_Accounts");

                    b.HasOne("LMSystem.Repository.Models.Course", "Course")
                        .WithMany("Orders")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Order_Course");

                    b.Navigation("Account");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("LMSystem.Repository.Models.RatingCourse", b =>
                {
                    b.HasOne("LMSystem.Repository.Models.RegistrationCourse", "Registration")
                        .WithMany("RatingCourses")
                        .HasForeignKey("RegistrationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_RatingCourse_RegistrationCourse");

                    b.Navigation("Registration");
                });

            modelBuilder.Entity("LMSystem.Repository.Models.RegistrationCourse", b =>
                {
                    b.HasOne("LMSystem.Repository.Models.Account", "Account")
                        .WithMany("RegistrationCourses")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_RegistrationCourse_Accounts");

                    b.HasOne("LMSystem.Repository.Models.Course", "Course")
                        .WithMany("RegistrationCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_RegistrationCourse_Course");

                    b.Navigation("Account");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("LMSystem.Repository.Models.ReportProblem", b =>
                {
                    b.HasOne("LMSystem.Repository.Models.Account", "Account")
                        .WithMany("ReportProblems")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ReportProblem_Accounts");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("LMSystem.Repository.Models.Section", b =>
                {
                    b.HasOne("LMSystem.Repository.Models.Course", "Course")
                        .WithMany("Sections")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Section_Course");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("LMSystem.Repository.Models.Step", b =>
                {
                    b.HasOne("LMSystem.Repository.Models.Section", "Section")
                        .WithMany("Steps")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Step_Section");

                    b.Navigation("Section");
                });

            modelBuilder.Entity("LMSystem.Repository.Models.StepCompleted", b =>
                {
                    b.HasOne("LMSystem.Repository.Models.RegistrationCourse", "Registration")
                        .WithMany("StepCompleteds")
                        .HasForeignKey("RegistrationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Registration");
                });

            modelBuilder.Entity("LMSystem.Repository.Models.WishList", b =>
                {
                    b.HasOne("LMSystem.Repository.Models.Account", "Account")
                        .WithMany("WishLists")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_WishList_Accounts");

                    b.HasOne("LMSystem.Repository.Models.Course", "Course")
                        .WithMany("WishLists")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_WishList_Course");

                    b.Navigation("Account");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("LMSystem.Repository.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("LMSystem.Repository.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LMSystem.Repository.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("LMSystem.Repository.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LMSystem.Repository.Models.Account", b =>
                {
                    b.Navigation("Notifications");

                    b.Navigation("Orders");

                    b.Navigation("RegistrationCourses");

                    b.Navigation("ReportProblems");

                    b.Navigation("WishLists");
                });

            modelBuilder.Entity("LMSystem.Repository.Models.Category", b =>
                {
                    b.Navigation("CourseCategories");
                });

            modelBuilder.Entity("LMSystem.Repository.Models.Course", b =>
                {
                    b.Navigation("CourseCategories");

                    b.Navigation("Orders");

                    b.Navigation("RegistrationCourses");

                    b.Navigation("Sections");

                    b.Navigation("WishLists");
                });

            modelBuilder.Entity("LMSystem.Repository.Models.RegistrationCourse", b =>
                {
                    b.Navigation("RatingCourses");

                    b.Navigation("StepCompleteds");
                });

            modelBuilder.Entity("LMSystem.Repository.Models.Section", b =>
                {
                    b.Navigation("Steps");
                });
#pragma warning restore 612, 618
        }
    }
}
