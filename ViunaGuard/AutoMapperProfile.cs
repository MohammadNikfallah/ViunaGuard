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
            CreateMap<Person, PersonGetDto>()
                .ForMember(p => p.BirthPlaceCity, m => m.MapFrom(p => p.BirthPlaceCity.Name))
                .ForMember(p => p.Gender, m => m.MapFrom(p => p.Gender.Name))
                .ForMember(p => p.Nationality, m => m.MapFrom(p => p.Nationality.Name))
                .ForMember(p => p.Religion, m => m.MapFrom(p => p.Religion.Name))
                .ForMember(p => p.EducationalDegree, m => m.MapFrom(p => p.EducationalDegree.Degree))
                .ForMember(p => p.MaritalStatus, m => m.MapFrom(p => p.MaritalStatus.Stat))
                .ForMember(p => p.CityOfResidence, m => m.MapFrom(p => p.CityOfResidence.Name))
                .ForMember(p => p.MilitaryServiceStatus, m => m.MapFrom(p => p.MilitaryServiceStatus.Stat));
            CreateMap<EntrancePostDto, Entrance>();
            CreateMap<Entrance, EntranceGetDto>();
            CreateMap<SignatureNeedForEntrancePermission, SignatureNeedGetDto>();
            CreateMap<EmployeeShift, EmployeeShiftGetDto>();
            CreateMap<EntrancePermission, EntrancePermissionGetDto>();
            CreateMap<ShiftPostDto, EmployeeShift>();
            CreateMap<EmployeePostDto, Employee>();
            CreateMap<Employee, EmployeeGetDto>();
            CreateMap<EntranceGroupPostDto, EntranceGroup>();
            CreateMap<EntranceGroup, EntranceGroupGetDto>();
            CreateMap<Person, PersonForEntranceGetDto>();
            CreateMap<Door, DoorGetDto>();
            CreateMap<EmployeePeriodicShift, PeriodicShiftGetDto>();
            CreateMap<PeriodicShiftPostDto, EmployeePeriodicShift>();
            CreateMap<EmployeePeriodicShift, EmployeeShiftGetDto>();
        }
    }
}
