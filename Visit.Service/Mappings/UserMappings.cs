using AutoMapper;
using Visit.DataAccess.Models;
using Visit.Service.Models;
using Visit.Service.Models.Requests;
using Visit.Service.Models.Responses;

namespace Visit.Service.Mappings
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<RegisterRequest, User>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));
            CreateMap<RegisterRequest, User>().ForMember(au => au.Firstname, map => map.MapFrom(vm => vm.Firstname));
            CreateMap<RegisterRequest, User>().ForMember(au => au.Lastname, map => map.MapFrom(vm => vm.Lastname));
            CreateMap<RegisterRequest, User>().ForMember(au => au.Email, map => map.MapFrom(vm => vm.Email));
            CreateMap<RegisterRequest, User>().ForMember(au => au.Birthday, map => map.MapFrom(vm => vm.Birthday));
            CreateMap<RegisterRequest, User>().ForMember(au => au.FkBirthLocationId, map => map.Ignore());
            CreateMap<RegisterRequest, User>().ForMember(au => au.FkResidenceLocationId, map => map.Ignore());
            CreateMap<RegisterRequest, User>().ForMember(au => au.Avi, map => map.Ignore());
            CreateMap<RegisterRequest, User>().ForMember(au => au.Education, map => map.MapFrom(vm => vm.Education));
            CreateMap<RegisterRequest, User>().ForMember(au => au.Title, map => map.MapFrom(vm => vm.Title));
            CreateMap<RegisterRequest, User>().ForMember(au => au.FacebookId, map => map.MapFrom(vm => vm.FacebookId));
            CreateMap<RegisterRequest, User>().ForMember(au => au.BirthLocation, map => map.MapFrom(vm => vm.BirthLocation));
            CreateMap<RegisterRequest, User>().ForMember(au => au.ResidenceLocation, map => map.MapFrom(vm => vm.ResidenceLocation));


            CreateMap<User, UserResponse>().ForMember(au => au.UserId, map => map.MapFrom(vm => vm.Id));
            CreateMap<User, UserResponse>().ForMember(au => au.Firstname, map => map.MapFrom(vm => vm.Firstname));
            CreateMap<User, UserResponse>().ForMember(au => au.Lastname, map => map.MapFrom(vm => vm.Lastname));
            CreateMap<User, UserResponse>().ForMember(au => au.Email, map => map.MapFrom(vm => vm.Email));
            CreateMap<User, UserResponse>().ForMember(au => au.Birthday, map => map.MapFrom(vm => vm.Birthday));
            CreateMap<User, UserResponse>().ForMember(au => au.BirthLocation, map => map.MapFrom(vm => vm.BirthLocation));
            CreateMap<User, UserResponse>().ForMember(au => au.ResidenceLocation, map => map.MapFrom(vm => vm.ResidenceLocation));
            CreateMap<User, UserResponse>().ForMember(au => au.Avi, map => map.MapFrom(vm => vm.Avi));
            CreateMap<User, UserResponse>().ForMember(au => au.FacebookId, map => map.MapFrom(vm => vm.FacebookId));
            CreateMap<User, UserResponse>().ForMember(au => au.Posts, map => map.MapFrom(vm => vm.Post));
            CreateMap<User, UserResponse>().ForMember(au => au.FollowerCount, map => map.Ignore());
            CreateMap<User, UserResponse>().ForMember(au => au.FollowingCount, map => map.Ignore());
            CreateMap<User, UserResponse>().ForMember(au => au.UserLocations, map => map.MapFrom(vm => vm.UserLocation));
            CreateMap<User, UserResponse>().ForMember(au => au.Education, map => map.MapFrom(vm => vm.Education));
            CreateMap<User, UserResponse>().ForMember(au => au.Title, map => map.MapFrom(vm => vm.Title));
            
            CreateMap<User, SlimUserResponse>().ForMember(au => au.UserId, map => map.MapFrom(vm => vm.Id));
            CreateMap<User, SlimUserResponse>().ForMember(au => au.FirstName, map => map.MapFrom(vm => vm.Firstname));
            CreateMap<User, SlimUserResponse>().ForMember(au => au.LastName, map => map.MapFrom(vm => vm.Lastname));
            CreateMap<User, SlimUserResponse>().ForMember(au => au.Avi, map => map.MapFrom(vm => vm.Avi));

        }

    }
}