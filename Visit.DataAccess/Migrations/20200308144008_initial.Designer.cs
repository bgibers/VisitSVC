﻿﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Visit.DataAccess.EntityFramework;

namespace Visit.Service.Migrations
{
    [DbContext(typeof(VisitContext))]
    [Migration("20200308144008_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Value")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.Like", b =>
                {
                    b.Property<int>("LikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<int>("FkPostId")
                        .HasColumnName("FK_PostId")
                        .HasColumnType("int(11)");

                    b.Property<string>("FkUserId")
                        .IsRequired()
                        .HasColumnName("FK_UserId")
                        .HasColumnType("varchar(255)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.HasKey("LikeId");

                    b.HasIndex("FkPostId")
                        .HasName("FK_PostId");

                    b.HasIndex("FkUserId")
                        .HasName("FK_UserId");

                    b.ToTable("Like");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<string>("LocationCode")
                        .HasColumnType("varchar(8)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<string>("LocationCountry")
                        .HasColumnType("varchar(500)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<string>("LocationName")
                        .HasColumnType("varchar(500)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<string>("LocationType")
                        .HasColumnType("varchar(500)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.HasKey("LocationId");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.LocationTag", b =>
                {
                    b.Property<int>("LocationTagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Location_TagId")
                        .HasColumnType("int(11)");

                    b.Property<int?>("FkTagId")
                        .HasColumnName("FK_TagId")
                        .HasColumnType("int(11)");

                    b.Property<int?>("FkUserLocationId")
                        .HasColumnName("FK_User_LocationId")
                        .HasColumnType("int(11)");

                    b.HasKey("LocationTagId");

                    b.HasIndex("FkTagId")
                        .HasName("Location_Tag_TagId");

                    b.HasIndex("FkUserLocationId")
                        .HasName("Location_Tag_LocationId");

                    b.ToTable("Location_Tag");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<int?>("FkPostTypeId")
                        .HasColumnName("FK_Post_TypeId")
                        .HasColumnType("int(11)");

                    b.Property<string>("FkUserId")
                        .HasColumnName("FK_UserId")
                        .HasColumnType("varchar(255)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<string>("PostCaption")
                        .HasColumnType("varchar(5000)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<string>("PostContentLink")
                        .HasColumnType("varchar(500)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<int?>("ReviewRating")
                        .HasColumnType("int(11)");

                    b.HasKey("PostId");

                    b.HasIndex("FkPostTypeId")
                        .HasName("FK_Post_PostType");

                    b.HasIndex("FkUserId")
                        .HasName("FK_Post_User");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.PostComment", b =>
                {
                    b.Property<int>("PostCommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<string>("CommentText")
                        .HasColumnType("varchar(500)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<DateTime?>("DatetimeOfComments")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("FkPostId")
                        .HasColumnName("FK_PostId")
                        .HasColumnType("int(11)");

                    b.Property<string>("FkUserIdOfCommenting")
                        .HasColumnName("FK_UserId_Of_commenting")
                        .HasColumnType("varchar(255)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.HasKey("PostCommentId");

                    b.HasIndex("FkPostId")
                        .HasName("FK_PostComment_Post");

                    b.HasIndex("FkUserIdOfCommenting")
                        .HasName("FK_PostComment_User");

                    b.ToTable("PostComment");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.PostTag", b =>
                {
                    b.Property<int>("PostTagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Post_TagId")
                        .HasColumnType("int(11)");

                    b.Property<int?>("FkPostId")
                        .HasColumnName("FK_PostId")
                        .HasColumnType("int(11)");

                    b.Property<int?>("FkTagId")
                        .HasColumnName("FK_TagId")
                        .HasColumnType("int(11)");

                    b.HasKey("PostTagId");

                    b.HasIndex("FkPostId")
                        .HasName("Post_Tag_PostId");

                    b.HasIndex("FkTagId")
                        .HasName("Post_Tag_TagId");

                    b.ToTable("Post_Tag");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.PostType", b =>
                {
                    b.Property<int>("PostTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<string>("Type")
                        .HasColumnType("varchar(350)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.HasKey("PostTypeId");

                    b.ToTable("PostType");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.PostUserLocation", b =>
                {
                    b.Property<int>("PostUserLocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Post_UserLocationId")
                        .HasColumnType("int(11)");

                    b.Property<int?>("FkLocationId")
                        .HasColumnName("FK_LocationId")
                        .HasColumnType("int(11)");

                    b.Property<int?>("FkPostId")
                        .HasColumnName("FK_PostId")
                        .HasColumnType("int(11)");

                    b.HasKey("PostUserLocationId");

                    b.HasIndex("FkLocationId")
                        .HasName("FK_Post_UserLocation_Location");

                    b.HasIndex("FkPostId")
                        .HasName("FK_Post_UserLocation_Post");

                    b.ToTable("Post_UserLocation");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<string>("Tag1")
                        .HasColumnName("Tag")
                        .HasColumnType("varchar(150)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.HasKey("TagId");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Avi")
                        .HasColumnType("varchar(150)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<long?>("FacebookId")
                        .HasColumnType("bigint");

                    b.Property<string>("Firstname")
                        .HasColumnType("varchar(150)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<int?>("FkBirthLocationId")
                        .HasColumnName("FK_BirthLocationId")
                        .HasColumnType("int(11)");

                    b.Property<int?>("FkResidenceLocationId")
                        .HasColumnName("FK_ResidenceLocationId")
                        .HasColumnType("int(11)");

                    b.Property<string>("Lastname")
                        .HasColumnType("varchar(150)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("FkBirthLocationId")
                        .HasName("FK_BirthLocationId");

                    b.HasIndex("FkResidenceLocationId")
                        .HasName("FK_ResidenceLocationId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.UserFollowing", b =>
                {
                    b.Property<int>("UserFollowingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<string>("FkFollowUserId")
                        .HasColumnName("FK_Follow_UserId")
                        .HasColumnType("varchar(255)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<string>("FkMainUserId")
                        .HasColumnName("FK_Main_UserId")
                        .HasColumnType("varchar(255)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<DateTime?>("FollowSince")
                        .HasColumnType("datetime(6)");

                    b.HasKey("UserFollowingId");

                    b.HasIndex("FkFollowUserId")
                        .HasName("FK_UserFollowing_User1");

                    b.HasIndex("FkMainUserId")
                        .HasName("FK_UserFollowing_User");

                    b.ToTable("UserFollowing");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.UserLocation", b =>
                {
                    b.Property<int>("UserLocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("User_LocationId")
                        .HasColumnType("int(11)");

                    b.Property<string>("City")
                        .HasColumnType("varchar(255)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<int?>("FkLocationId")
                        .HasColumnName("FK_LocationId")
                        .HasColumnType("int(11)");

                    b.Property<string>("FkUserId")
                        .HasColumnName("FK_UserId")
                        .HasColumnType("varchar(255)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<string>("Status")
                        .HasColumnType("varchar(500)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<string>("Venue")
                        .HasColumnType("varchar(500)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.HasKey("UserLocationId");

                    b.HasIndex("FkLocationId")
                        .HasName("FK_User_Location_Location");

                    b.HasIndex("FkUserId")
                        .HasName("FK_User_Location_User");

                    b.ToTable("User_Location");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.UserMessage", b =>
                {
                    b.Property<int>("UserMessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<string>("FkRecieverUserId")
                        .HasColumnName("FK_Reciever_UserId")
                        .HasColumnType("varchar(255)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<string>("FkSenderUserId")
                        .HasColumnName("FK_Sender_UserId")
                        .HasColumnType("varchar(255)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<string>("MessageContent")
                        .HasColumnType("varchar(5000)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<DateTime?>("MessageSentTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("UserMessageId");

                    b.HasIndex("FkRecieverUserId")
                        .HasName("FK_UserMessage_User1");

                    b.HasIndex("FkSenderUserId")
                        .HasName("FK_UserMessage_User");

                    b.ToTable("UserMessage");
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
                    b.HasOne("Visit.DataAccess.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Visit.DataAccess.Models.User", null)
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

                    b.HasOne("Visit.DataAccess.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Visit.DataAccess.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Visit.DataAccess.Models.Like", b =>
                {
                    b.HasOne("Visit.DataAccess.Models.Post", "FkPost")
                        .WithMany("Like")
                        .HasForeignKey("FkPostId")
                        .HasConstraintName("Like_ibfk_1")
                        .IsRequired();

                    b.HasOne("Visit.DataAccess.Models.User", "FkUser")
                        .WithMany("Like")
                        .HasForeignKey("FkUserId")
                        .HasConstraintName("Like_ibfk_2")
                        .IsRequired();
                });

            modelBuilder.Entity("Visit.DataAccess.Models.LocationTag", b =>
                {
                    b.HasOne("Visit.DataAccess.Models.Tag", "FkTag")
                        .WithMany("LocationTag")
                        .HasForeignKey("FkTagId")
                        .HasConstraintName("Location_Tag_TagId");

                    b.HasOne("Visit.DataAccess.Models.UserLocation", "FkUserLocation")
                        .WithMany("LocationTag")
                        .HasForeignKey("FkUserLocationId")
                        .HasConstraintName("Location_Tag_LocationId");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.Post", b =>
                {
                    b.HasOne("Visit.DataAccess.Models.PostType", "FkPostType")
                        .WithMany("Post")
                        .HasForeignKey("FkPostTypeId")
                        .HasConstraintName("FK_Post_PostType");

                    b.HasOne("Visit.DataAccess.Models.User", "FkUser")
                        .WithMany("Post")
                        .HasForeignKey("FkUserId")
                        .HasConstraintName("FK_Post_User");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.PostComment", b =>
                {
                    b.HasOne("Visit.DataAccess.Models.Post", "FkPost")
                        .WithMany("PostComment")
                        .HasForeignKey("FkPostId")
                        .HasConstraintName("FK_PostComment_Post");

                    b.HasOne("Visit.DataAccess.Models.User", "FkUserIdOfCommentingNavigation")
                        .WithMany("PostComment")
                        .HasForeignKey("FkUserIdOfCommenting")
                        .HasConstraintName("FK_PostComment_User");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.PostTag", b =>
                {
                    b.HasOne("Visit.DataAccess.Models.Post", "FkPost")
                        .WithMany("PostTag")
                        .HasForeignKey("FkPostId")
                        .HasConstraintName("Post_Tag_PostId");

                    b.HasOne("Visit.DataAccess.Models.Tag", "FkTag")
                        .WithMany("PostTag")
                        .HasForeignKey("FkTagId")
                        .HasConstraintName("Post_Tag_TagId");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.PostUserLocation", b =>
                {
                    b.HasOne("Visit.DataAccess.Models.UserLocation", "FkLocation")
                        .WithMany("PostUserLocation")
                        .HasForeignKey("FkLocationId")
                        .HasConstraintName("FK_Post_UserLocation_Location");

                    b.HasOne("Visit.DataAccess.Models.Post", "FkPost")
                        .WithMany("PostUserLocation")
                        .HasForeignKey("FkPostId")
                        .HasConstraintName("FK_Post_UserLocation_Post");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.User", b =>
                {
                    b.HasOne("Visit.DataAccess.Models.Location", "FkBirthLocation")
                        .WithMany("UserFkBirthLocation")
                        .HasForeignKey("FkBirthLocationId")
                        .HasConstraintName("User_ibfk_1");

                    b.HasOne("Visit.DataAccess.Models.Location", "FkResidenceLocation")
                        .WithMany("UserFkResidenceLocation")
                        .HasForeignKey("FkResidenceLocationId")
                        .HasConstraintName("User_ibfk_2");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.UserFollowing", b =>
                {
                    b.HasOne("Visit.DataAccess.Models.User", "FkFollowUser")
                        .WithMany("UserFollowingFkFollowUser")
                        .HasForeignKey("FkFollowUserId")
                        .HasConstraintName("FK_UserFollowing_User1");

                    b.HasOne("Visit.DataAccess.Models.User", "FkMainUser")
                        .WithMany("UserFollowingFkMainUser")
                        .HasForeignKey("FkMainUserId")
                        .HasConstraintName("FK_UserFollowing_User");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.UserLocation", b =>
                {
                    b.HasOne("Visit.DataAccess.Models.Location", "FkLocation")
                        .WithMany("UserLocation")
                        .HasForeignKey("FkLocationId")
                        .HasConstraintName("FK_User_Location_Location");

                    b.HasOne("Visit.DataAccess.Models.User", "FkUser")
                        .WithMany("UserLocation")
                        .HasForeignKey("FkUserId")
                        .HasConstraintName("FK_User_Location_User");
                });

            modelBuilder.Entity("Visit.DataAccess.Models.UserMessage", b =>
                {
                    b.HasOne("Visit.DataAccess.Models.User", "FkRecieverUser")
                        .WithMany("UserMessageFkRecieverUser")
                        .HasForeignKey("FkRecieverUserId")
                        .HasConstraintName("FK_UserMessage_User1");

                    b.HasOne("Visit.DataAccess.Models.User", "FkSenderUser")
                        .WithMany("UserMessageFkSenderUser")
                        .HasForeignKey("FkSenderUserId")
                        .HasConstraintName("FK_UserMessage_User");
                });
#pragma warning restore 612, 618
        }
    }
}