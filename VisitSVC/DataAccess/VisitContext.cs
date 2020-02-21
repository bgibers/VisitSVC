using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VisitSVC
{
    public class VisitContext : DbContext
    {
        public VisitContext()
        {
        }

        public VisitContext(DbContextOptions<VisitContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<PostComments> PostComments { get; set; }
        public virtual DbSet<PostLocations> PostLocations { get; set; }
        public virtual DbSet<PostTypes> PostTypes { get; set; }
        public virtual DbSet<Posts> Posts { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserFollowing> UserFollowing { get; set; }
        public virtual DbSet<UserLocations> UserLocations { get; set; }
        public virtual DbSet<UserMessages> UserMessages { get; set; }
        public virtual DbSet<UserTags> UserTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;port=1521;user=VisitDBA;password=Clemson17;database=VisitV2", x => x.ServerVersion("8.0.17-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("location");

                entity.Property(e => e.LocationId)
                    .HasColumnName("Location_Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationCode)
                    .HasColumnName("location_code")
                    .HasColumnType("varchar(8)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.LocationCountry)
                    .HasColumnName("location_country")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.LocationName)
                    .HasColumnName("location_name")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.LocationType)
                    .HasColumnName("location_type")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<PostComments>(entity =>
            {
                entity.HasIndex(e => e.FkPostId)
                    .HasName("FK_PostComments_posts");

                entity.HasIndex(e => e.FkUserIdOfCommenting)
                    .HasName("FK_PostComments_User");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CommentText)
                    .HasColumnName("Comment_Text")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FkPostId)
                    .HasColumnName("FK_Post_Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserIdOfCommenting)
                    .HasColumnName("FK_User_ID_Of_commenting")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.FkPost)
                    .WithMany(p => p.PostComments)
                    .HasForeignKey(d => d.FkPostId)
                    .HasConstraintName("FK_PostComments_posts");

                entity.HasOne(d => d.FkUserIdOfCommentingNavigation)
                    .WithMany(p => p.PostComments)
                    .HasForeignKey(d => d.FkUserIdOfCommenting)
                    .HasConstraintName("FK_PostComments_User");
            });

            modelBuilder.Entity<PostLocations>(entity =>
            {
                entity.HasKey(e => e.PostLocationId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.FkLocationId)
                    .HasName("FK_PostLocations_location");

                entity.HasIndex(e => e.FkPostId)
                    .HasName("FK_PostLocations_posts");

                entity.Property(e => e.PostLocationId)
                    .HasColumnName("PostLocation_Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkLocationId)
                    .HasColumnName("FK_Location_Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkPostId)
                    .HasColumnName("FK_Post_Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.FkLocation)
                    .WithMany(p => p.PostLocations)
                    .HasForeignKey(d => d.FkLocationId)
                    .HasConstraintName("FK_PostLocations_location");

                entity.HasOne(d => d.FkPost)
                    .WithMany(p => p.PostLocations)
                    .HasForeignKey(d => d.FkPostId)
                    .HasConstraintName("FK_PostLocations_posts");
            });

            modelBuilder.Entity<PostTypes>(entity =>
            {
                entity.HasKey(e => e.PostTypeId)
                    .HasName("PRIMARY");

                entity.ToTable("post_types");

                entity.Property(e => e.PostTypeId)
                    .HasColumnName("post_type_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PostType)
                    .HasColumnName("post_type")
                    .HasColumnType("varchar(350)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Posts>(entity =>
            {
                entity.HasKey(e => e.PostId)
                    .HasName("PRIMARY");

                entity.ToTable("posts");

                entity.HasIndex(e => e.FkPostTypeId)
                    .HasName("FK_posts_post_types");

                entity.HasIndex(e => e.FkUserId)
                    .HasName("FK_posts_User");

                entity.Property(e => e.PostId)
                    .HasColumnName("Post_Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkPostTypeId)
                    .HasColumnName("FK_Post_Type_Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserId)
                    .HasColumnName("FK_User_Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PostCaption)
                    .HasColumnName("post_caption")
                    .HasColumnType("varchar(5000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.PostContentLink)
                    .HasColumnName("post_content_link")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ReviewRating).HasColumnType("int(11)");

                entity.HasOne(d => d.FkPostType)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.FkPostTypeId)
                    .HasConstraintName("FK_posts_post_types");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.FkUserId)
                    .HasConstraintName("FK_posts_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.FkBirthLocationId)
                    .HasName("FK_birth_location_ID");

                entity.HasIndex(e => e.FkResidenceLocationId)
                    .HasName("FK_residence_location_ID");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.Avi)
                    .HasColumnName("avi")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Birthday).HasColumnName("birthday");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Firstname)
                    .HasColumnName("firstname")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FkBirthLocationId)
                    .HasColumnName("FK_birth_location_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkResidenceLocationId)
                    .HasColumnName("FK_residence_location_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasColumnType("varchar(350)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

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
                entity.HasKey(e => e.FollowUserId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.FkFollowUserId)
                    .HasName("FK_UserFollowing_User1");

                entity.HasIndex(e => e.FkMainUserId)
                    .HasName("FK_UserFollowing_User");

                entity.Property(e => e.FollowUserId)
                    .HasColumnName("FollowUser_Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkFollowUserId)
                    .HasColumnName("FK_Follow_User_Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkMainUserId)
                    .HasColumnName("FK_Main_User_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.FkFollowUser)
                    .WithMany(p => p.UserFollowingFkFollowUser)
                    .HasForeignKey(d => d.FkFollowUserId)
                    .HasConstraintName("FK_UserFollowing_User1");

                entity.HasOne(d => d.FkMainUser)
                    .WithMany(p => p.UserFollowingFkMainUser)
                    .HasForeignKey(d => d.FkMainUserId)
                    .HasConstraintName("FK_UserFollowing_User");
            });

            modelBuilder.Entity<UserLocations>(entity =>
            {
                entity.HasKey(e => e.UserLocationId)
                    .HasName("PRIMARY");

                entity.ToTable("User_Locations");

                entity.HasIndex(e => e.FkLocationId)
                    .HasName("FK_User_Locations_location");

                entity.HasIndex(e => e.FkUserId)
                    .HasName("FK_User_Locations_User");

                entity.Property(e => e.UserLocationId)
                    .HasColumnName("UserLocation_Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkLocationId)
                    .HasColumnName("FK_Location_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserId)
                    .HasColumnName("FK_User_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Tags)
                    .HasColumnName("tags")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Venue)
                    .HasColumnName("venue")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.FkLocation)
                    .WithMany(p => p.UserLocations)
                    .HasForeignKey(d => d.FkLocationId)
                    .HasConstraintName("FK_User_Locations_location");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.UserLocations)
                    .HasForeignKey(d => d.FkUserId)
                    .HasConstraintName("FK_User_Locations_User");
            });

            modelBuilder.Entity<UserMessages>(entity =>
            {
                entity.HasKey(e => e.UserMessages1)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.FkReciverUserId)
                    .HasName("FK_UserMessages_User1");

                entity.HasIndex(e => e.FkSenderUserId)
                    .HasName("FK_UserMessages_User");

                entity.Property(e => e.UserMessages1)
                    .HasColumnName("UserMessages")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkReciverUserId)
                    .HasColumnName("FK_Reciver_User_Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkSenderUserId)
                    .HasColumnName("FK_Sender_User_Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MessageContent)
                    .HasColumnType("varchar(5000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.FkReciverUser)
                    .WithMany(p => p.UserMessagesFkReciverUser)
                    .HasForeignKey(d => d.FkReciverUserId)
                    .HasConstraintName("FK_UserMessages_User1");

                entity.HasOne(d => d.FkSenderUser)
                    .WithMany(p => p.UserMessagesFkSenderUser)
                    .HasForeignKey(d => d.FkSenderUserId)
                    .HasConstraintName("FK_UserMessages_User");
            });

            modelBuilder.Entity<UserTags>(entity =>
            {
                entity.HasKey(e => e.UserTags1)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.FkLocationPostId)
                    .HasName("FK_location_Post_Id");

                entity.HasIndex(e => e.FkUserId)
                    .HasName("FK_UserTags_User");

                entity.Property(e => e.UserTags1)
                    .HasColumnName("UserTags")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkLocationPostId)
                    .HasColumnName("FK_location_Post_Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserId)
                    .HasColumnName("FK_UserId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IsPostLocation).HasColumnName("IsPost_Location");

                entity.HasOne(d => d.FkLocationPost)
                    .WithMany(p => p.UserTags)
                    .HasForeignKey(d => d.FkLocationPostId)
                    .HasConstraintName("UserTags_ibfk_1");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.UserTags)
                    .HasForeignKey(d => d.FkUserId)
                    .HasConstraintName("FK_UserTags_User");
            });

//            OnModelCreatingPartial(modelBuilder);
        }
    }
}
