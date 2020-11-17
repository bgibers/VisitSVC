using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public PostService(VisitContext visitContext, UserManager<User> userManager, IBlobStorageBusinessLogic blobStorage, ILogger<PostService> logger, IMapper mapper)
        {
            _visitContext = visitContext;
            _userManager = userManager;
            _blobStorage = blobStorage;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<List<Post>> GetPostsByUser(string userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PaginatedList<Post>> GetPostsByPage(int? pageNumber)
        {
            int pageSize = 50;
            var postList = _visitContext.Post.OrderByDynamic("PostTime", "OrderByDescending")
                .Include(p => p.FkUser)
                .Include(p => p.FkPostType)
                .Include(p => p.PostUserLocation)
                .ThenInclude(p => p.FkLocation.FkLocation);

            foreach (var post in postList)
            {
                // todo properly return userResponse here instead of scrubbing the data
                var temp = _mapper.Map<UserResponse>(post.FkUser);
                post.FkUser = _mapper.Map<User>(temp);
            }

            return await PaginatedList<Post>.CreateAsync(postList.AsNoTracking(), pageNumber ?? 1, pageSize);
        }

        public async Task<NewPostResponse> CreatePost(Claim claim, CreatePostRequest postRequest)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(claim.Value);
                var location = _visitContext.Location.Single(f => f.LocationCode == postRequest.LocationCode);
                var postType = _visitContext.PostType.SingleOrDefault(t => t.Type == postRequest.PostType);

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
                
                var post = new Post
                {
                    PostContentLink = res?.ToString() ?? "",
                    FkPostType = postType,
                    PostCaption = postRequest.Caption,
                    PostTime = DateTime.UtcNow,
                    FkUser = user
                };

                _visitContext.Post.Add(post);

                var userLocation = _visitContext.UserLocation.SingleOrDefault(e =>
                    e.FkUser == user && e.FkLocation == location);

                if (userLocation == null)
                {
                    _visitContext.UserLocation.Add(new UserLocation
                    {
                        Status = "toVisit",
                        Venue = "",
                        FkLocation = location,
                        FkUser = user
                    });
                }

                _visitContext.PostUserLocation.Add(new PostUserLocation
                {
                    FkLocation = userLocation,
                    FkPost = post
                });

                await _visitContext.SaveChangesAsync();

                return new NewPostResponse(true, null);
            }
            catch (Exception e)
            {
                return new NewPostResponse(false, null);
            }
        }
    }
}