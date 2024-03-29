﻿using Microsoft.EntityFrameworkCore;
using Visit.DataAccess.Models;

namespace Visit.DataAccess.EntityFramework
{
    public partial class VisitContext : DbContext
    {
        public VisitContext()
        {
        }

        public VisitContext(DbContextOptions<VisitContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EfmigrationsHistory> EfmigrationsHistory { get; set; }
        public virtual DbSet<Like> Like { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<LocationTag> LocationTag { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<PostComment> PostComment { get; set; }
        public virtual DbSet<PostTag> PostTag { get; set; }
        public virtual DbSet<PostType> PostType { get; set; }
        public virtual DbSet<PostUserLocation> PostUserLocation { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserFollowing> UserFollowing { get; set; }
        public virtual DbSet<UserLocation> UserLocation { get; set; }
        public virtual DbSet<UserMessage> UserMessage { get; set; }
        public virtual DbSet<UserNotification> UserNotification { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;port=1521;user=root;password=clemson17;database=VisitV2", x => x.ServerVersion("8.0.17-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EfmigrationsHistory>(entity =>
            {
                entity.HasKey(e => e.MigrationId)
                    .HasName("PRIMARY");

                entity.ToTable("__EFMigrationsHistory");

                entity.Property(e => e.MigrationId)
                    .HasColumnType("varchar(95)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasColumnType("varchar(32)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasIndex(e => e.FkPostId)
                    .HasName("FK_PostId");

                entity.HasIndex(e => e.FkUserId)
                    .HasName("FK_UserId");

                entity.Property(e => e.LikeId).HasColumnType("int(11)");

                entity.Property(e => e.FkPostId)
                    .HasColumnName("FK_PostId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserId)
                    .IsRequired()
                    .HasColumnName("FK_UserId")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.TimeOfLike).HasColumnType("datetime");

                entity.HasOne(d => d.FkPost)
                    .WithMany(p => p.Like)
                    .HasForeignKey(d => d.FkPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Like_ibfk_1");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.Like)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Like_ibfk_2");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.LocationId).HasColumnType("int(11)");

                entity.Property(e => e.LocationCode)
                    .HasColumnType("varchar(8)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.LocationCountry)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.LocationName)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.LocationType)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<LocationTag>(entity =>
            {
                entity.ToTable("Location_Tag");

                entity.HasIndex(e => e.FkTagId)
                    .HasName("Location_Tag_TagId");

                entity.HasIndex(e => e.FkUserLocationId)
                    .HasName("Location_Tag_LocationId");

                entity.Property(e => e.LocationTagId)
                    .HasColumnName("Location_TagId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkTagId)
                    .HasColumnName("FK_TagId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserLocationId)
                    .HasColumnName("FK_User_LocationId")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.FkTag)
                    .WithMany(p => p.LocationTag)
                    .HasForeignKey(d => d.FkTagId)
                    .HasConstraintName("Location_Tag_TagId");

                entity.HasOne(d => d.FkUserLocation)
                    .WithMany(p => p.LocationTag)
                    .HasForeignKey(d => d.FkUserLocationId)
                    .HasConstraintName("Location_Tag_LocationId");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasIndex(e => e.FkPostTypeId)
                    .HasName("FK_Post_PostType");

                entity.HasIndex(e => e.FkUserId)
                    .HasName("FK_Post_User");

                entity.Property(e => e.PostId).HasColumnType("int(11)");

                entity.Property(e => e.Deleted)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.FkPostTypeId)
                    .HasColumnName("FK_Post_TypeId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserId)
                    .HasColumnName("FK_UserId")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.PostCaption)
                    .HasColumnType("varchar(5000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.PostContentLink)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.PostTime).HasColumnType("datetime");

                entity.Property(e => e.ReviewRating).HasColumnType("int(11)");

                entity.HasOne(d => d.FkPostType)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.FkPostTypeId)
                    .HasConstraintName("FK_Post_PostType");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.FkUserId)
                    .HasConstraintName("FK_Post_User");
            });

            modelBuilder.Entity<PostComment>(entity =>
            {
                entity.HasIndex(e => e.FkPostId)
                    .HasName("FK_PostComment_Post");

                entity.HasIndex(e => e.FkUserIdOfCommenting)
                    .HasName("FK_PostComment_User");

                entity.Property(e => e.PostCommentId).HasColumnType("int(11)");

                entity.Property(e => e.CommentText)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Deleted)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.FkPostId)
                    .HasColumnName("FK_PostId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserIdOfCommenting)
                    .HasColumnName("FK_UserId_Of_commenting")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.HasOne(d => d.FkPost)
                    .WithMany(p => p.PostComment)
                    .HasForeignKey(d => d.FkPostId)
                    .HasConstraintName("FK_PostComment_Post");

                entity.HasOne(d => d.FkUserIdOfCommentingNavigation)
                    .WithMany(p => p.PostComment)
                    .HasForeignKey(d => d.FkUserIdOfCommenting)
                    .HasConstraintName("FK_PostComment_User");
            });

            modelBuilder.Entity<PostTag>(entity =>
            {
                entity.ToTable("Post_Tag");

                entity.HasIndex(e => e.FkPostId)
                    .HasName("Post_Tag_PostId");

                entity.HasIndex(e => e.FkTagId)
                    .HasName("Post_Tag_TagId");

                entity.Property(e => e.PostTagId)
                    .HasColumnName("Post_TagId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkPostId)
                    .HasColumnName("FK_PostId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkTagId)
                    .HasColumnName("FK_TagId")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.FkPost)
                    .WithMany(p => p.PostTag)
                    .HasForeignKey(d => d.FkPostId)
                    .HasConstraintName("Post_Tag_PostId");

                entity.HasOne(d => d.FkTag)
                    .WithMany(p => p.PostTag)
                    .HasForeignKey(d => d.FkTagId)
                    .HasConstraintName("Post_Tag_TagId");
            });

            modelBuilder.Entity<PostType>(entity =>
            {
                entity.Property(e => e.PostTypeId).HasColumnType("int(11)");

                entity.Property(e => e.Type)
                    .HasColumnType("varchar(350)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<PostUserLocation>(entity =>
            {
                entity.ToTable("Post_UserLocation");

                entity.HasIndex(e => e.FkLocationId)
                    .HasName("FK_Post_UserLocation_Location");

                entity.HasIndex(e => e.FkPostId)
                    .HasName("FK_Post_UserLocation_Post");

                entity.Property(e => e.PostUserLocationId)
                    .HasColumnName("Post_UserLocationId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkLocationId)
                    .HasColumnName("FK_LocationId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkPostId)
                    .HasColumnName("FK_PostId")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.FkLocation)
                    .WithMany(p => p.PostUserLocation)
                    .HasForeignKey(d => d.FkLocationId)
                    .HasConstraintName("FK_Post_UserLocation_Location");

                entity.HasOne(d => d.FkPost)
                    .WithMany(p => p.PostUserLocation)
                    .HasForeignKey(d => d.FkPostId)
                    .HasConstraintName("FK_Post_UserLocation_Post");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.TagId).HasColumnType("int(11)");

                entity.Property(e => e.Tag1)
                    .HasColumnName("Tag")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.FkBirthLocationId)
                    .HasName("FK_BirthLocationId");

                entity.HasIndex(e => e.FkResidenceLocationId)
                    .HasName("FK_ResidenceLocationId");

                entity.Property(e => e.Id)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Avi)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.BirthLocation)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Education)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Email)
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.FcmToken)
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Firstname)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.FkBirthLocationId)
                    .HasColumnName("FK_BirthLocationId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkResidenceLocationId)
                    .HasColumnName("FK_ResidenceLocationId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Lastname)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.ResidenceLocation)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Title)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.HasOne(d => d.FkBirthLocation)
                    .WithMany(p => p.UserFkBirthLocation)
                    .HasForeignKey(d => d.FkBirthLocationId)
                    .HasConstraintName("User_ibfk_1");

                entity.HasOne(d => d.FkResidenceLocation)
                    .WithMany(p => p.UserFkResidenceLocation)
                    .HasForeignKey(d => d.FkResidenceLocationId)
                    .HasConstraintName("User_ibfk_2");
            });

            modelBuilder.Entity<UserFollowing>(entity =>
            {
                entity.HasIndex(e => e.FkFollowUserId)
                    .HasName("FK_UserFollowing_User1");

                entity.HasIndex(e => e.FkMainUserId)
                    .HasName("FK_UserFollowing_User");

                entity.Property(e => e.UserFollowingId).HasColumnType("int(11)");

                entity.Property(e => e.FkFollowUserId)
                    .HasColumnName("FK_Follow_UserId")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.FkMainUserId)
                    .HasColumnName("FK_Main_UserId")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.HasOne(d => d.FkFollowUser)
                    .WithMany(p => p.UserFollowingFkFollowUser)
                    .HasForeignKey(d => d.FkFollowUserId)
                    .HasConstraintName("FK_UserFollowing_User1");

                entity.HasOne(d => d.FkMainUser)
                    .WithMany(p => p.UserFollowingFkMainUser)
                    .HasForeignKey(d => d.FkMainUserId)
                    .HasConstraintName("FK_UserFollowing_User");
            });

            modelBuilder.Entity<UserLocation>(entity =>
            {
                entity.ToTable("User_Location");

                entity.HasIndex(e => e.FkLocationId)
                    .HasName("FK_User_Location_Location");

                entity.HasIndex(e => e.FkUserId)
                    .HasName("FK_User_Location_User");

                entity.Property(e => e.UserLocationId)
                    .HasColumnName("User_LocationId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CheckedOff)
                    .HasColumnName("Checked_Off")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.City)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.FkLocationId)
                    .HasColumnName("FK_LocationId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserId)
                    .HasColumnName("FK_UserId")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Status)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Venue)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.HasOne(d => d.FkLocation)
                    .WithMany(p => p.UserLocation)
                    .HasForeignKey(d => d.FkLocationId)
                    .HasConstraintName("FK_User_Location_Location");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.UserLocation)
                    .HasForeignKey(d => d.FkUserId)
                    .HasConstraintName("FK_User_Location_User");
            });

            modelBuilder.Entity<UserMessage>(entity =>
            {
                entity.HasIndex(e => e.FkRecieverUserId)
                    .HasName("FK_UserMessage_User1");

                entity.HasIndex(e => e.FkSenderUserId)
                    .HasName("FK_UserMessage_User");

                entity.Property(e => e.UserMessageId).HasColumnType("int(11)");

                entity.Property(e => e.FkRecieverUserId)
                    .HasColumnName("FK_Reciever_UserId")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.FkSenderUserId)
                    .HasColumnName("FK_Sender_UserId")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.MessageContent)
                    .HasColumnType("varchar(5000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.HasOne(d => d.FkRecieverUser)
                    .WithMany(p => p.UserMessageFkRecieverUser)
                    .HasForeignKey(d => d.FkRecieverUserId)
                    .HasConstraintName("FK_UserMessage_User1");

                entity.HasOne(d => d.FkSenderUser)
                    .WithMany(p => p.UserMessageFkSenderUser)
                    .HasForeignKey(d => d.FkSenderUserId)
                    .HasConstraintName("FK_UserMessage_User");
            });

            modelBuilder.Entity<UserNotification>(entity =>
            {
                entity.HasKey(e => e.NotificationId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.FkPostId)
                    .HasName("FK_PostId");

                entity.HasIndex(e => e.FkUserId)
                    .HasName("FK_UserId");

                entity.HasIndex(e => e.FkUserWhoNotified)
                    .HasName("FK_UserWhoNotified");

                entity.HasIndex(e => e.LikeId)
                    .HasName("LikeId");

                entity.HasIndex(e => e.PostCommentId)
                    .HasName("PostCommentId");

                entity.Property(e => e.NotificationId).HasColumnType("int(11)");

                entity.Property(e => e.DatetimeOfNot).HasColumnType("datetime");

                entity.Property(e => e.FkPostId)
                    .HasColumnName("FK_PostId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserId)
                    .IsRequired()
                    .HasColumnName("FK_UserId")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.FkUserWhoNotified)
                    .IsRequired()
                    .HasColumnName("FK_UserWhoNotified")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.LikeId).HasColumnType("int(11)");

                entity.Property(e => e.PostCommentId).HasColumnType("int(11)");

                entity.HasOne(d => d.FkPost)
                    .WithMany(p => p.UserNotification)
                    .HasForeignKey(d => d.FkPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserNotification_ibfk_4");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.UserNotificationFkUser)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserNotification_ibfk_3");

                entity.HasOne(d => d.FkUserWhoNotifiedNavigation)
                    .WithMany(p => p.UserNotificationFkUserWhoNotifiedNavigation)
                    .HasForeignKey(d => d.FkUserWhoNotified)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserNotification_ibfk_5");

                entity.HasOne(d => d.Like)
                    .WithMany(p => p.UserNotification)
                    .HasForeignKey(d => d.LikeId)
                    .HasConstraintName("UserNotification_ibfk_2");

                entity.HasOne(d => d.PostComment)
                    .WithMany(p => p.UserNotification)
                    .HasForeignKey(d => d.PostCommentId)
                    .HasConstraintName("UserNotification_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
