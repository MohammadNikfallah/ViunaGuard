using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ViunaGuard.Dtos;
using ViunaGuard.Models;

namespace ViunaGuard.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserService(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("postCar")]
        public async Task<ServiceResponse<Person>> PostCar(Car car, int id)
        {
            var response = new ServiceResponse<Person>();

            Person? p = await context.People.FirstOrDefaultAsync(p => p.Id == id);
            if (p == null)
            {
                response.Message = "Person Not Found";
                response.HttpResponseCode = 404;
                return response;
            }
            p.Cars.Add(car);
            var carInList = await context.Cars.FirstOrDefaultAsync(c => c.Id == car.Id);
            if (carInList != null)
                carInList.People.Add(p);
            else
            {
                car.People.Add(p);
                context.Cars.Add(car);
            }
            await context.SaveChangesAsync();
            response.HttpResponseCode = 200;
            response.Data = p;
            return response;
        }

        [HttpPost("postPerson")]
        public async Task<ServiceResponse<List<Person>>> PostPerson(PersonPostDto personDto)
        {
            var response = new ServiceResponse<List<Person>>();

            var person = mapper.Map<Person>(personDto);
            var personAdditionalInfo = new PersonAdditionalInfo();
            person.PersonAdditionalInfo = personAdditionalInfo;
            await context.PersonAdditionalInfos.AddAsync(personAdditionalInfo);
            await context.People.AddAsync(person);
            await context.SaveChangesAsync();
            var people = await context.People.ToListAsync();

            response.Data = people;
            response.HttpResponseCode=200;
            return response;
        }

        public async Task<ServiceResponse<List<EmployeeShiftPeriodicMonthly>>> PostShiftMonthly(EmployeeShiftPeriodicMonthly shift)
        {
            var response = new ServiceResponse<List<EmployeeShiftPeriodicMonthly>>();

            shift.Id = await context.EmployeeShiftsMonthly.MaxAsync(p => p.Id) + 1;
            await context.EmployeeShiftsMonthly.AddAsync(shift);
            await context.SaveChangesAsync();
            var shifts = await context.EmployeeShiftsMonthly.ToListAsync();

            response.Data = shifts;
            response.HttpResponseCode=200;
            return response;
        }

        public async Task<ServiceResponse<List<SignatureNeedGetDto>>> GetOrganizationSignatureNeed
            (int organizationId)
        {
            var response = new ServiceResponse<List<SignatureNeedGetDto>>();

            var data = await context.SignatureNeedForEntrancePermissions
                .Where(s => s.OrganizationId == organizationId)
                .Include(s => s.MinAuthority)
                .Select(s => mapper.Map<SignatureNeedGetDto>(s))
                .ToListAsync();

            if (data.Count == 0)
            {
                response.HttpResponseCode = 404;
                response.Message = "no SignatureNeed found related to this Organization";
                return response;
            }

            response.Data = data;
            response.HttpResponseCode=200;
            return response;
        }

        public async Task<ServiceResponse<List<Authority>>> GetOrganizationAuthorities(int organizationId)
        {
            var response = new ServiceResponse<List<Authority>>();

            var data = await context.Authorities
                .Where(s => s.OrganizationId == organizationId)
                .OrderBy(s => s.AuthorityLevel)
                .ToListAsync();

            if(data.Count == 0)
            {
                response.HttpResponseCode = 404;
                response.Message = "no authority found related to this Organization";
                return response;
            }

            response.Data = data;
            response.HttpResponseCode = 200;
            return response;
        }

        public async Task<ServiceResponse<PersonGetDto>> GetPersonDetails()
        {
            var response = new ServiceResponse<PersonGetDto>();

            var id = httpContextAccessor.HttpContext.User.FindFirstValue("ID");
            if(id == null)
            {
                response.HttpResponseCode = 404;
                response.Message = "Id claim Not Found!";
                return response;
            }

            var data = await context.People
                .FirstOrDefaultAsync
                (p => p.Id.ToString() == id);

            if(data == null)
            {
                response.HttpResponseCode = 404;
                response.Message = "User Not Found!";
                return response;
            }

            var dataDto = mapper.Map<PersonGetDto>(data);

            response.Data = dataDto;
            response.HttpResponseCode = 200;
            return response;
        }
    }
}
