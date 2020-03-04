using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Visit.DataAccess.EntityFramework;
using Visit.DataAccess.Models;

namespace Visit.Service.BusinessLogic
{
    public class PostTestDataService
    { 
        private readonly VisitContext _visitContext;

        public PostTestDataService(VisitContext visitContext)
        {
            _visitContext = visitContext;
        }

        public async Task<List<string>> CreateUsers()
        {
            try
            {
                User testUser1 = new User()
                {
                    UserId = "1",
                    Avi = "sometesturl",
                    Birthday = DateTime.Today,
                    Email = "testuser1@gmail.com",
                    Username = "TestUser1",
                    Firstname = "TestUser1",
                    FkBirthLocation = _visitContext.Location.First(l => l.LocationCode == "US-MA"),
                    FkResidenceLocation = _visitContext.Location.First(l => l.LocationCode == "US-SC"),
                    Lastname = "TestUser1"

                };
                User testUser2 = new User()
                {
                    UserId = "2",
                    Avi = "sometesturl",
                    Birthday = DateTime.Today,
                    Email = "testuser2@gmail.com",
                    Username = "TestUser2",
                    Firstname = "TestUser2",
                    FkBirthLocation = _visitContext.Location.First(l => l.LocationCode == "US-NC"),
                    FkResidenceLocation = _visitContext.Location.First(l => l.LocationCode == "US-FL"),
                    Lastname = "TestUser2"
                };

                PopulateUserLocations(testUser1);
                PopulateUserLocations(testUser2);

                _visitContext.User.Add(testUser1);
                _visitContext.User.Add(testUser2);


                await _visitContext.SaveChangesAsync();
                return new List<string>() {testUser1.UserId, testUser2.UserId};
            }
            catch (Exception e)
            {
                return new List<string>();
            }
        }

        private void PopulateUserLocations(User user)
        {
            Random random = new Random();
            
            for (int i = 0; i < 20; i++)
            {
                int id = random.Next(1, 405);

                Location location = _visitContext.Location.Find(id);

                UserLocation userLocation = new UserLocation
                {
                    Status = id % 2 != 0 ? "toVisit" : "visited",
                    Venue = location.LocationType,
                    FkLocation = location,
                    FkUser = user
                };

                _visitContext.UserLocation.Add(userLocation);
                
                CreatePosts(userLocation);
            }
        }

        private void CreatePosts(UserLocation location)
        {
            Post post = new Post
            {
                PostContentLink = "testlinktest",
                PostCaption = location.FkLocation.LocationCode,
                ReviewRating = 5,
                FkUser = location.FkUser
                
            };
            _visitContext.Post.Add(post);

            _visitContext.PostUserLocation.Add(new PostUserLocation()
            {
                FkLocation = location,
                FkPost = post
            });
        }

        public async Task<List<User>> GetUsers()
        {
            return await _visitContext.User
                .Include(u => u.UserLocation)
                .Include(u => u.FkBirthLocation)
                .Include(u => u.FkResidenceLocation)
                .Include(u => u.Post)
                
                .ToListAsync();
        }
        
        public async Task<User> GetUser(string id)
        {
            return await _visitContext.User
                .Include(u => u.UserLocation)
                .SingleOrDefaultAsync(u => u.UserId == id);
        }
    }
}