using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Visit.DataAccess.EntityFramework;
using Visit.DataAccess.Models;
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
        private readonly UserManager<User> _userManager;
        private readonly IBlobStorageBusinessLogic _blobStorage;
        private readonly ILogger<PostService> _logger;
        private readonly IMapper _mapper;

        public PostService(VisitContext visitContext, UserManager<User> userManager, IBlobStorageBusinessLogic blobStorage, 
            ILogger<PostService> logger, IMapper mapper)
        {
            _visitContext = visitContext;
            _userManager = userManager;
            _blobStorage = blobStorage;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginatedList<PostApi>> GetPostsByPage(Claim claim, int? pageNumber, string filter = "", string userId = "")
        {
            var user = await _userManager.FindByNameAsync(claim.Value);

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

                bool likedByCurrentUser = _visitContext.Like.Any(l => l.FkPostId == post.PostId && l.FkUserId == user.Id);

                try
                {
                    postApiList.Add(new PostApi()
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
                    _logger.LogError($"Couldn't get post: {post.PostId} : {e}");
                }
            }
            
            return new PaginatedList<PostApi>()
            {
                HasNextPage = postPaginatedList.HasNextPage,
                HasPreviousPage = postPaginatedList.HasPreviousPage,
                Items = postApiList,
                PageIndex = postPaginatedList.PageIndex,
                TotalPages = postPaginatedList.TotalPages
            };
        }
        
        public async Task<NewPostResponse> CreatePost(Claim claim, CreatePostRequest postRequest)
        {
            var user = await _userManager.FindByNameAsync(claim.Value);
            
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
                        _logger.LogError("User " + user.UserName + " post image not updated");
                        return new NewPostResponse(false, new ImageErrors()
                        {
                            IdentityErrors = null,
                            UploadError = "User " + user.UserName + " post image could not be uploaded"
                        });
                    }
                }
                
                var userLocation = await _visitContext.UserLocation.SingleOrDefaultAsync(e =>
                    e.FkUser == user && e.FkLocation == location);

                if (userLocation == null)
                {
                    userLocation = new UserLocation
                    {
                        Status = "toVisit",
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

                return new NewPostResponse(true, null);
            }
            catch (Exception e)
            {
                _logger.LogError($"Could not create new post for user {user.Id}: {e}");
                return new NewPostResponse(false, null);
            }
        }

        public async Task<bool> LikePost(Claim claim, string postId)
        {
            // TODO only like a post once
            var user = await _userManager.FindByNameAsync(claim.Value);

            try
            {
                var post = await _visitContext.Post.FindAsync(int.Parse(postId));
                
                if (_visitContext.Like.Any(l => l.FkPostId == int.Parse(postId) && l.FkUserId == user.Id))
                {
                    return false;
                }
                
                await _visitContext.Like.AddAsync(new Like()
                {
                    FkPost = post,
                    FkUser = user
                });
                
                await _visitContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"{user.Id} could not like postId {postId}: {e}");
                return false;
            }
        }
        
        public async Task<bool> CommentOnPost(Claim claim, string postId, string comment)
        {
            var user = await _userManager.FindByNameAsync(claim.Value);

            try
            {
                var post = await _visitContext.Post.FindAsync(int.Parse(postId));

                await _visitContext.PostComment.AddAsync(new PostComment()
                {
                    FkPost = post,
                    FkUserIdOfCommentingNavigation = user,
                    DatetimeOfComments = DateTime.UtcNow,
                    CommentText = comment
                });
                
                await _visitContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"{user.Id} could not comment on postId {postId}: {e}");
                return false;
            }
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
                    likesForPosts.Add(new LikeForPost()
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
                    commentsForPost.Add(new CommentForPost()
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