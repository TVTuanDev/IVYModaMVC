using AutoMapper;
using IVYModa.Areas.Identity.Models.Manage;
using IVYModa.EF;

namespace IVYModa.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile ()
        {
            CreateMap<AppUser, UserVM>();
            CreateMap<UserVM, AppUser>();
            CreateMap<LocationVM, Location>();
        }
    }
}
