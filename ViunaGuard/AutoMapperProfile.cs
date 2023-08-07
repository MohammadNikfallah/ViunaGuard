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
        }
    }
}
