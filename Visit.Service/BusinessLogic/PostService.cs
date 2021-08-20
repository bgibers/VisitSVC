using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Visit.DataAccess.EntityFramework;
using Visit.DataAccess.Models;
using Visit.Service.ApiControllers.Models;
using Visit.Service.BusinessLogic.BlobStorage;
using Visit.Service.BusinessLogic.Interfaces;
using Visit.Service.Models;
using Visit.Service.Models.Extenstions;
using Visit.Service.Models.Requests;
using Visit.Service.Models.Responses;

namespace Visit.Service.BusinessLogic
{
    public class PostService : IPostService
    {
        private readonly VisitContext _visitContext;
        private readonly IFirebaseService _firebaseService;
        private readonly IBlobStorageBusinessLogic _blobStorage;
        private readonly ILogger<PostService> _logger;
        private readonly IMapper _mapper;

        public PostService(VisitContext visitContext, IFirebaseService firebaseService, IBlobStorageBusinessLogic blobStorage, 
            ILogger<PostService> logger, IMapper mapper)
        {
            _visitContext = visitContext;
            _firebaseService = firebaseService;
            _blobStorage = blobStorage;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginatedList<PostApi>> GetPostsByPage(string claim, int? pageNumber, string filter = "",
            string userId = "")
        {
            var user = await _firebaseService.GetUserFromToken(claim);

            int pageSize = 50;
            var postApiList = new List<PostApi>();
            IIncludableQueryable<Post, Location> postList;
            
            if (string.IsNullOrEmpty(filter) && string.IsNullOrEmpty(userId))
            {
               postList = _visitContext.Post.OrderByDynamic("PostTime", "OrderByDescending")
                    .Include(p => p.FkUser)
                    .Include(p => p.FkPostType)
                    .Include(p => p.PostComment)
                    .Include(p => p.Like)
                    .Include(p => p.PostUserLocation)
                    .ThenInclude(p => p.FkLocation.FkLocation);
            }
            else if (string.IsNullOrEmpty(userId))
            {
                postList = _visitContext.Post
                    .Where(p => p.PostUserLocation.First().FkLocation.FkLocation.LocationCode == filter)
                    .OrderByDynamic("PostTime", "OrderByDescending")
                        .Include(p => p.FkUser)
                        .Include(p => p.FkPostType)
                        .Include(p => p.PostComment)
                        .Include(p => p.Like)
                        .Include(p => p.PostUserLocation)
                        .ThenInclude(p => p.FkLocation.FkLocation);
            } else if (string.IsNullOrEmpty(filter))
            {
                postList = _visitContext.Post
                    .Where(p => p.FkUserId == userId)
                    .OrderByDynamic("PostTime", "OrderByDescending")
                    .Include(p => p.FkUser)
                    .Include(p => p.FkPostType)
                    .Include(p => p.PostComment)
                    .Include(p => p.Like)
                    .Include(p => p.PostUserLocation)
                    .ThenInclude(p => p.FkLocation.FkLocation);
            }
            else
            {
                postList = _visitContext.Post
                    .Where(p => p.FkUserId == userId && p.PostUserLocation.First().FkLocation.FkLocation.LocationCode == filter)
                    .OrderByDynamic("PostTime", "OrderByDescending")
                    .Include(p => p.FkUser)
                    .Include(p => p.FkPostType)
                    .Include(p => p.PostComment)
                    .Include(p => p.Like)
                    .Include(p => p.PostUserLocation)
                    .ThenInclude(p => p.FkLocation.FkLocation);
            }
    

            var postPaginatedList = await PaginatedList<Post>.CreateAsync(postList.AsNoTracking(), pageNumber ?? 1, pageSize);
            
            // We should only loop through the items we need to loop through. All of this context shit needs to be cleaned up
            // don't think this is the cleanest
            foreach (var post in postPaginatedList.Items)
            {

                var commentCount = post.PostComment.Count;
                var likeCount = post.Like.Count;

                bool likedByCurrentUser = await _visitContext.Like.AnyAsync(l => l.FkPostId == post.PostId && l.FkUserId == user.Uid);

                try
                {
                    postApiList.Add(new PostApi
                    {
                        PostId = post.PostId,
                        FkPostTypeId = post.FkPostTypeId,
                        FkUserId = post.FkUserId,
                        PostContentLink = post.PostContentLink ?? "",
                        PostCaption = post.PostCaption,
                        PostTime = post.PostTime,
                        ReviewRating = post.ReviewRating,
                        FkPostType = post.FkPostType,
                        FkUser = _mapper.Map<UserResponse>(post.FkUser),
                        LikedByCurrentUser = likedByCurrentUser,
                        Location = post.PostUserLocation.SingleOrDefault()?.FkLocation.FkLocation,
                        CommentCount = commentCount,
                        LikeCount = likeCount
                    });
                }
                catch(Exception e)
                {
                    _logger.LogError($"Couldn't get post: {post.PostId} - {JsonConvert.SerializeObject(post)}: {e}");
                }
            }
            
            return new PaginatedList<PostApi>
            {
                HasNextPage = postPaginatedList.HasNextPage,
                HasPreviousPage = postPaginatedList.HasPreviousPage,
                Items = postApiList,
                PageIndex = postPaginatedList.PageIndex,
                TotalPages = postPaginatedList.TotalPages
            };
        }

        public async Task<PostApi> GetPostById(string claim, int postId)
        {
            var user = await _firebaseService.GetUserFromToken(claim);

            var post = await _visitContext.Post.Where(p => p.PostId == postId)
                .Include(p => p.FkUser)
                .Include(p => p.FkPostType)
                .Include(p => p.PostComment)
                .Include(p => p.Like)
                .Include(p => p.PostUserLocation)
                .ThenInclude(p => p.FkLocation.FkLocation)
                .FirstOrDefaultAsync();
            
            var commentCount = post.PostComment.Count;
            var likeCount = post.Like.Count;

            bool likedByCurrentUser = await _visitContext.Like.AnyAsync(l => l.FkPostId == post.PostId && l.FkUserId == user.Uid);
            
            return new PostApi
            {
                PostId = post.PostId,
                FkPostTypeId = post.FkPostTypeId,
                FkUserId = post.FkUserId,
                PostContentLink = post.PostContentLink ?? "",
                PostCaption = post.PostCaption,
                PostTime = post.PostTime,
                ReviewRating = post.ReviewRating,
                FkPostType = post.FkPostType,
                FkUser = _mapper.Map<UserResponse>(post.FkUser),
                LikedByCurrentUser = likedByCurrentUser,
                Location = post.PostUserLocation.SingleOrDefault()?.FkLocation.FkLocation,
                CommentCount = commentCount,
                LikeCount = likeCount
            };
        } 
        
        public async Task<NewPostResponse> CreatePost(string claim, CreatePostRequest postRequest)
        {
            var userId = (await _firebaseService.GetUserFromToken(claim)).Uid;
            var user = await _visitContext.User.FindAsync(userId);
            
            try
            {
                var location = await _visitContext.Location.SingleAsync(f => f.LocationCode == postRequest.LocationCode);
                var postType = await _visitContext.PostType.SingleOrDefaultAsync(t => t.Type == postRequest.PostType);

                Uri res = null;
                if (postRequest.Image != null)
                {
                    var fileName = Guid.NewGuid();
                    res = await _blobStorage.UploadBlob($"{user.Id}/posts/{location.LocationName}", postRequest.Image, fileName);
                    if (string.IsNullOrEmpty(res.ToString()))
                    {
                        _logger.LogError("User " + user.Email + " post image not updated");
                        return new NewPostResponse(false, new ImageErrors
                        {
                            IdentityErrors = null,
                            UploadError = "User " + user.Email + " post image could not be uploaded"
                        });
                    }
                }
                
                var userLocation = await _visitContext.UserLocation.SingleOrDefaultAsync(e =>
                    e.FkUser == user && e.FkLocation == location);

                if (userLocation == null)
                {
                    userLocation = new UserLocation
                    {
                        Status = "visited",
                        Venue = "",
                        FkLocation = location,
                        FkUser = user
                    };
                    await _visitContext.UserLocation.AddAsync(userLocation);
                }
                
                var post = new Post
                {
                    PostContentLink = res?.ToString() ?? "",
                    FkPostType = postType,
                    PostCaption = postRequest.Caption,
                    PostTime = DateTime.UtcNow,
                    FkUser = user
                };

                await _visitContext.Post.AddAsync(post);
                
                await _visitContext.PostUserLocation.AddAsync(new PostUserLocation
                {
                    FkLocation = userLocation,
                    FkPost = post
                });

                await _visitContext.SaveChangesAsync();

                return new NewPostResponse(true);
            }
            catch (Exception e)
            {
                _logger.LogError($"Could not create new post for user {user.Id}: {e}");
                return new NewPostResponse();
            }
        }

        public async Task<NewPostResponse> EditPost(string claim, int postId, CreatePostRequest postRequest)
        {
            try
            {
                var userId = (await _firebaseService.GetUserFromToken(claim)).Uid;
                var user = await _visitContext.User.FindAsync(userId);

                var post = await _visitContext.Post.Where(p => p.PostId == postId && p.FkUserId == userId)
                    .FirstOrDefaultAsync();

                post.PostCaption = postRequest.Caption;
                
                _visitContext.Post.Update(post);
                await _visitContext.SaveChangesAsync();
                return new NewPostResponse(true);
            }
            catch (Exception e)
            {
                _logger.LogError($"Could not edit post {postId}: {e}");
                return new NewPostResponse();
            }
        }
        
        public async Task<NewPostResponse> DeletePost(string claim, int postId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> LikePost(string claim, string postId)
        { 
            var userLikingId = (await _firebaseService.GetUserFromToken(claim)).Uid;
            var userLiking = await _visitContext.User.FindAsync(userLikingId);

            try
            {
                var post = (_visitContext.Post.Where(p => p.PostId == int.Parse(postId))
                    .Include(p => p.FkUser)).ToList().SingleOrDefault();
                
                if (_visitContext.Like.Any(l => l.FkPostId == int.Parse(postId) && l.FkUserId == userLiking.Id) || post == null)
                {
                    return false;
                }

                var like = new Like
                {
                    FkPost = post,
                    FkUser = userLiking
                };
                await _visitContext.Like.AddAsync(like);
                await _visitContext.SaveChangesAsync();

                if (post.FkUser.Id != userLikingId)
                {
                    await _visitContext.UserNotification.AddAsync(new UserNotification()
                    {
                        FkUser = post.FkUser,
                        FkUserWhoNotifiedNavigation = userLiking,
                        FkPost = post,
                        DatetimeOfNot = DateTime.UtcNow,
                        LikeId = like.LikeId
                    });
                
                    await _visitContext.SaveChangesAsync(); 
                
                    var message = new Message()
                    {
                        Token = post.FkUser.FcmToken,
                        Notification = new Notification()
                        {
                            Body = $"{userLiking.Firstname} {userLiking.Lastname} liked your post."
                        },
                        Data = new Dictionary<string, string>()
                        {
                            {"postId", postId}
                        }
                    };

                    await _firebaseService.SendPushNotification(message);
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLiking.Id} could not like postId {postId}: {e}");
                return false;
            }
        }
        
        public async Task<bool> CommentOnPost(string claim, string postId, string comment)
        {
            var userCommentingId = (await _firebaseService.GetUserFromToken(claim)).Uid;
            var userCommenting = await _visitContext.User.FindAsync(userCommentingId);

            try
            {
                var post = (_visitContext.Post.Where(p => p.PostId == int.Parse(postId))
                    .Include(p => p.FkUser)).ToList().SingleOrDefault();

                if (post == null) return false;
                
                var commentObj = new PostComment
                {
                    FkPost = post,
                    FkUserIdOfCommentingNavigation = userCommenting,
                    DatetimeOfComments = DateTime.UtcNow,
                    CommentText = comment
                };
                await _visitContext.PostComment.AddAsync(commentObj);
                await _visitContext.SaveChangesAsync();

                if (post.FkUser.Id != userCommentingId)
                {
                    await SendUserNotification(post.FkUser, userCommenting, post, commentObj);
                }
                else
                {
                    var allUsersWhoCommented = (_visitContext.PostComment.Where(p => p.FkPostId == int.Parse(postId))
                        .Include(p => p.FkUserIdOfCommentingNavigation))
                        .Select(p => p.FkUserIdOfCommentingNavigation);

                    if (!allUsersWhoCommented.Any()) return true;
                    
                    foreach (var user in allUsersWhoCommented)
                    {
                        if (user.Id == userCommentingId) continue;
                        await SendUserNotification(user, userCommenting, post, commentObj);
                    }
                }
                
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userCommenting.Id} could not comment on postId {postId}: {e}");
                return false;
            }
        }

        public async Task<bool> EditComment(string claim, int commentId, string commentText)
        {
            try
            {
                var userId = (await _firebaseService.GetUserFromToken(claim)).Uid;
                var user = await _visitContext.User.FindAsync(userId);

                var comment = await _visitContext.PostComment.Where(p => p.PostCommentId == commentId && p.FkUserIdOfCommenting == userId)
                    .FirstOrDefaultAsync();

                comment.CommentText = commentText;
                
                _visitContext.PostComment.Update(comment);
                await _visitContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"Could not edit comment {commentId}: {e}");
                return false;
            }
        }
        

        private async Task<bool> SendUserNotification(User userWhoIsBeingNotified, User userNotifying, Post post,
            PostComment commentObj)
        {
            await _visitContext.UserNotification.AddAsync(new UserNotification()
            {
                FkUser = userWhoIsBeingNotified,
                FkUserWhoNotifiedNavigation = userNotifying,
                FkPost = post,
                DatetimeOfNot = DateTime.UtcNow,
                PostCommentId = commentObj.PostCommentId
            });
                
            await _visitContext.SaveChangesAsync(); 
                
            var message = new Message()
            {
                Token = post.FkUser.FcmToken,
                Notification = new Notification()
                {
                    Body = $"{userNotifying.Firstname} {userNotifying.Lastname} commented {commentObj.CommentText}"
                },
                Data = new Dictionary<string, string>()
                {
                    {"postId", post.PostId.ToString()}
                }
            };

            await _firebaseService.SendPushNotification(message);

            return true;
        }

        public List<LikeForPost> GetLikesForPost(string postId)
        {
            try
            {
                var likes = _visitContext.Like.Where(l => l.FkPostId == int.Parse(postId))
                    .Include(l => l.FkUser);

                if (likes == null) return new List<LikeForPost>();
                
                List<LikeForPost> likesForPosts = new List<LikeForPost>();
                
                foreach (var like in likes)
                {
                    likesForPosts.Add(new LikeForPost
                    {
                        FkPostId = like.FkPostId,
                        LikeId = like.LikeId,
                        User = _mapper.Map<User, SlimUserResponse>(like.FkUser)
                    });
                }

                return likesForPosts;
            }
            catch (Exception e)
            {
                _logger.LogError($"Could not get likes for post {postId}");
                return new List<LikeForPost>();
            }
        }
        
        public List<CommentForPost> GetCommentsForPost(string postId)
        {
            try
            {
                var comments = _visitContext.PostComment.Where(l => l.FkPostId == int.Parse(postId))
                    .Include(l => l.FkUserIdOfCommentingNavigation);

                if (comments == null) return new List<CommentForPost>();
                
                List<CommentForPost> commentsForPost = new List<CommentForPost>();
                
                foreach (var comment in comments)
                {
                    commentsForPost.Add(new CommentForPost
                    {
                        FkPostId = comment.FkPostId,
                        CommentId = comment.PostCommentId,
                        Comment = comment.CommentText,
                        Date = comment.DatetimeOfComments,
                        User = _mapper.Map<User, SlimUserResponse>(comment.FkUserIdOfCommentingNavigation)
                    });
                }

                return commentsForPost;
            }
            catch (Exception e)
            {
                _logger.LogError($"Could not get comments for post {postId}");
                return new List<CommentForPost>();
            }
        }
    }
}