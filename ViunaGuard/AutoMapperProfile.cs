using AutoMapper;
using ViunaGuard.Dtos;
using ViunaGuard.Models;

namespace ViunaGuard
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PersonPostDto, Person>();
            CreateMap<Person, PersonGetDto>();
            CreateMap<EntrancePostDto, Entrance>();
            CreateMap<Entrance, EntranceGetDto>();
            CreateMap<SignatureNeedForEntrancePermission, SignatureNeedGetDto>();
        }
    }
}
