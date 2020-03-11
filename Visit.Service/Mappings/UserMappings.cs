using AutoMapper;
using Visit.DataAccess.Models;
using Visit.Service.Models;

namespace Visit.Service.Mappings
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<RegisterModelApi, User>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Username));
            CreateMap<RegisterModelApi, User>().ForMember(au => au.Firstname, map => map.MapFrom(vm => vm.Firstname));
            CreateMap<RegisterModelApi, User>().ForMember(au => au.Lastname, map => map.MapFrom(vm => vm.Lastname));
            CreateMap<RegisterModelApi, User>().ForMember(au => au.Email, map => map.MapFrom(vm => vm.Email));
            CreateMap<RegisterModelApi, User>().ForMember(au => au.Birthday, map => map.MapFrom(vm => vm.Birthday));
            CreateMap<RegisterModelApi, User>().ForMember(au => au.FkBirthLocationId, map => map.MapFrom(vm => vm.FkBirthLocationId));
            CreateMap<RegisterModelApi, User>().ForMember(au => au.FkResidenceLocationId, map => map.MapFrom(vm => vm.FkResidenceLocationId));
            CreateMap<RegisterModelApi, User>().ForMember(au => au.Avi, map => map.Ignore());
            CreateMap<RegisterModelApi, User>().ForMember(au => au.FacebookId, map => map.MapFrom(vm => vm.FacebookId));

            CreateMap<User, UserApi>().ForMember(au => au.Username, map => map.MapFrom(vm => vm.UserName));
            CreateMap<User, UserApi>().ForMember(au => au.Firstname, map => map.MapFrom(vm => vm.Firstname));
            CreateMap<User, UserApi>().ForMember(au => au.Lastname, map => map.MapFrom(vm => vm.Lastname));
            CreateMap<User, UserApi>().ForMember(au => au.Email, map => map.MapFrom(vm => vm.Email));
            CreateMap<User, UserApi>().ForMember(au => au.Birthday, map => map.MapFrom(vm => vm.Birthday));
            CreateMap<User, UserApi>().ForMember(au => au.BirthLocation, map => map.MapFrom(vm => vm.FkBirthLocation));
            CreateMap<User, UserApi>().ForMember(au => au.ResidenceLocation, map => map.MapFrom(vm => vm.FkResidenceLocation));
            CreateMap<User, UserApi>().ForMember(au => au.Avi, map => map.MapFrom(vm => vm.Avi));
            CreateMap<User, UserApi>().ForMember(au => au.FacebookId, map => map.MapFrom(vm => vm.FacebookId));
            CreateMap<User, UserApi>().ForMember(au => au.Likes, map => map.MapFrom(vm => vm.Like));
            CreateMap<User, UserApi>().ForMember(au => au.Posts, map => map.MapFrom(vm => vm.Post));
            CreateMap<User, UserApi>().ForMember(au => au.Followers, map => map.MapFrom(vm => vm.UserFollowingFkMainUser));
            CreateMap<User, UserApi>().ForMember(au => au.Following, map => map.MapFrom(vm => vm.UserFollowingFkFollowUser));
            CreateMap<User, UserApi>().ForMember(au => au.Followers, map => map.MapFrom(vm => vm.UserFollowingFkMainUser));
            CreateMap<User, UserApi>().ForMember(au => au.UserLocations, map => map.MapFrom(vm => vm.UserLocation));

        }

    }
}