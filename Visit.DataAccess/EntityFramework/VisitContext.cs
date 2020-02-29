using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Visit.DataAccess.Models;

namespace Visit.DataAccess.EntityFramework
{
    public partial class VisitContext : DbContext
    {
        private readonly IConfiguration _configuration;
        
        public VisitContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public VisitContext(DbContextOptions<VisitContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(_configuration.GetConnectionString("MySql"), x => x.ServerVersion("8.0.17-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.LocationId).HasColumnType("int(11)");

                entity.Property(e => e.LocationCode)
                    .HasColumnType("varchar(8)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.LocationCountry)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.LocationName)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.LocationType)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
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

                entity.Property(e => e.FkPostTypeId)
                    .HasColumnName("FK_Post_TypeId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserId)
                    .HasColumnName("FK_UserId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PostCaption)
                    .HasColumnType("varchar(5000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.PostContentLink)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

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
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FkPostId)
                    .HasColumnName("FK_PostId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserIdOfCommenting)
                    .HasColumnName("FK_UserId_Of_commenting")
                    .HasColumnType("int(11)");

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
                    .HasCollation("utf8mb4_0900_ai_ci");
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
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.FkBirthLocationId)
                    .HasName("FK_BirthLocationId");

                entity.HasIndex(e => e.FkResidenceLocationId)
                    .HasName("FK_ResidenceLocationId");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.Avi)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Email)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Firstname)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FkBirthLocationId)
                    .HasColumnName("FK_BirthLocationId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkResidenceLocationId)
                    .HasColumnName("FK_ResidenceLocationId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Lastname)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Password)
                    .HasColumnType("varchar(350)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Username)
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
                entity.HasIndex(e => e.FkFollowUserId)
                    .HasName("FK_UserFollowing_User1");

                entity.HasIndex(e => e.FkMainUserId)
                    .HasName("FK_UserFollowing_User");

                entity.Property(e => e.UserFollowingId).HasColumnType("int(11)");

                entity.Property(e => e.FkFollowUserId)
                    .HasColumnName("FK_Follow_UserId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkMainUserId)
                    .HasColumnName("FK_Main_UserId")
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

                entity.Property(e => e.FkLocationId)
                    .HasColumnName("FK_LocationId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserId)
                    .HasColumnName("FK_UserId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Venue)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

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
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkSenderUserId)
                    .HasColumnName("FK_Sender_UserId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MessageContent)
                    .HasColumnType("varchar(5000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.FkRecieverUser)
                    .WithMany(p => p.UserMessageFkRecieverUser)
                    .HasForeignKey(d => d.FkRecieverUserId)
                    .HasConstraintName("FK_UserMessage_User1");

                entity.HasOne(d => d.FkSenderUser)
                    .WithMany(p => p.UserMessageFkSenderUser)
                    .HasForeignKey(d => d.FkSenderUserId)
                    .HasConstraintName("FK_UserMessage_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
