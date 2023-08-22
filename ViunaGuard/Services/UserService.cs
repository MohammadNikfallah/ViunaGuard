using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using ViunaGuard.Dtos;
using ViunaGuard.Models;

namespace ViunaGuard.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("postCar")]
        public async Task<ServiceResponse<Person>> PostCar(Car car, int id)
        {
            var response = new ServiceResponse<Person>();

            Person? p = await _context.People.FirstOrDefaultAsync(p => p.Id == id);
            if (p == null)
            {
                response.Message = "Person Not Found";
                response.HttpResponseCode = 404;
                return response;
            }
            p.Cars.Add(car);
            var carInList = await _context.Cars.FirstOrDefaultAsync(c => c.Id == car.Id);
            if (carInList != null)  
                carInList.People.Add(p);
            else
            {
                car.People.Add(p);
                _context.Cars.Add(car);
            }
            await _context.SaveChangesAsync();
            response.HttpResponseCode = 200;
            response.Data = p;
            return response;
        }

        [HttpPost("postPerson")]
        public async Task<ServiceResponse<List<Person>>> PostPerson(PersonPostDto personDto)
        {
            var response = new ServiceResponse<List<Person>>();

            var person = _mapper.Map<Person>(personDto);
            var personAdditionalInfo = new PersonAdditionalInfo();
            person.PersonAdditionalInfo = personAdditionalInfo;
            await _context.PersonAdditionalInfos.AddAsync(personAdditionalInfo);
            await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();
            var people = await _context.People.ToListAsync();

            response.Data = people;
            response.HttpResponseCode=200;
            return response;
        }

        public async Task<ServiceResponse<List<EmployeeShiftPeriodicMonthly>>> PostShiftMonthly(MonthlyShiftPostDto shift)
        {
            var response = new ServiceResponse<List<EmployeeShiftPeriodicMonthly>>();

            await _context.EmployeeShiftsMonthly.AddAsync(_mapper.Map<EmployeeShiftPeriodicMonthly>(shift));
            await _context.SaveChangesAsync();
            var shifts = await _context.EmployeeShiftsMonthly.ToListAsync();

            response.Data = shifts;
            response.HttpResponseCode=200;
            return response;
        }
        public async Task<ServiceResponse<List<EmployeeShiftPeriodicWeekly>>> PostShiftWeekly(WeeklyShiftPostDto shift)
        {
            var response = new ServiceResponse<List<EmployeeShiftPeriodicWeekly>>();

            await _context.EmployeeShiftsWeekly.AddAsync(_mapper.Map<EmployeeShiftPeriodicWeekly>(shift));
            await _context.SaveChangesAsync();
            var shifts = await _context.EmployeeShiftsWeekly.ToListAsync();

            response.Data = shifts;
            response.HttpResponseCode=200;
            return response;
        }
        public async Task<ServiceResponse<List<EmployeeShift>>> PostShift(ShiftPostDto shift)
        {
            var response = new ServiceResponse<List<EmployeeShift>>();

            await _context.EmployeeShifts.AddAsync(_mapper.Map<EmployeeShift>(shift));
            await _context.SaveChangesAsync();
            var shifts = await _context.EmployeeShifts.ToListAsync();

            response.Data = shifts;
            response.HttpResponseCode=200;
            return response;
        }

        public async Task<ServiceResponse<List<SignatureNeedGetDto>>> GetOrganizationSignatureNeed
            (int organizationId)
        {
            var response = new ServiceResponse<List<SignatureNeedGetDto>>();

            var data = await _context.SignatureNeedForEntrancePermissions
                .Where(s => s.OrganizationId == organizationId)
                .Include(s => s.MinAuthority)
                .Select(s => _mapper.Map<SignatureNeedGetDto>(s))
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

            var data = await _context.Authorities
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

            var id = _httpContextAccessor.HttpContext!.User.FindFirstValue("ID");
            if(id == null)
            {
                response.HttpResponseCode = 404;
                response.Message = "Id claim Not Found!";
                return response;
            }

            var data = await _context.People
                .Include(p => p.Nationality)
                .Include(p => p.Religion)
                .Include(p => p.EducationalDegree)
                .Include(p => p.Gender)
                .Include(p => p.MaritalStatus)
                .Include(p => p.CityOfResidence)
                .Include(p => p.BirthPlaceCity)
                .Include(p => p.MilitaryServiceStatus)
                .FirstOrDefaultAsync
                (p => p.Id.ToString() == id);

            if(data == null)
            {
                response.HttpResponseCode = 404;
                response.Message = "User Not Found!";
                return response;
            }

            var dataDto = _mapper.Map<PersonGetDto>(data);

            response.Data = dataDto;
            response.HttpResponseCode = 200;
            return response;
        }

        public async Task<ServiceResponse<EmployeeShift>> GetCurrentShift(int employeeId)
        {
            var response = new ServiceResponse<EmployeeShift>();
            var time = DateTime.Now;
            var person = await _context.People.FindAsync(_httpContextAccessor.HttpContext!.User.FindFirstValue("ID"));
            if(person!.Jobs.Any(j => j.Id == employeeId))
            {
                response.HttpResponseCode = 400;
                response.Message = "Something wrong with EmployeeId";
                return response;
            }

            var weeklyShift = _context.EmployeeShiftsWeekly
                .Include(e => e.GuardDoor)
                .FirstOrDefault(e => e.EmployeeId == employeeId && e.DayOfWeek == ((int)time.DayOfWeek) 
                    && time < e.FinishTime && time > e.StartTime);
            if(weeklyShift != null)
            {
                response.Data = _mapper.Map<EmployeeShift>(weeklyShift);
                response.HttpResponseCode = 200;
                return response;
            }

            var monthlyShift = _context.EmployeeShiftsMonthly
                .Include(e => e.GuardDoor)
                .FirstOrDefault(e => e.EmployeeId == employeeId && e.DayOfMonth == time.Month 
                    && time < e.FinishTime && time > e.StartTime);
            if(monthlyShift != null)
            {
                response.Data = _mapper.Map<EmployeeShift>(monthlyShift);
                response.HttpResponseCode = 200;
                return response;
            }

            var shift = _context.EmployeeShifts
                .Include(e => e.GuardDoor)
                .FirstOrDefault(e => e.EmployeeId == employeeId && time < e.FinishTime && time > e.StartTime);
            if(shift != null)
            {
                response.Data = _mapper.Map<EmployeeShift>(monthlyShift);
                response.HttpResponseCode = 200;
                return response;
            }

            response.HttpResponseCode = 404;
            response.Message = "There is no Current Shift";
            return response;
        }

        public async Task<ServiceResponse<List<EntrancePermissionGetDto>>> GetEntrancePermissions()
        {
            var response = new ServiceResponse<List<EntrancePermissionGetDto>> ();

            var id = _httpContextAccessor.HttpContext!.User.FindFirstValue("ID");
            if (id == null)
            {
                response.HttpResponseCode = 404;
                response.Message = "Id claim Not Found!";
                return response;
            }

            var data = await _context.People
                .Include(p => p.EntrancePermissions)
                .FirstOrDefaultAsync
                (p => p.Id.ToString() == id);

            if (data == null)
            {
                response.HttpResponseCode = 404;
                response.Message = "User Not Found!";
                return response;
            }

            var permissions = data.EntrancePermissions;

            var dataDto = permissions.Select(p => _mapper.Map<EntrancePermissionGetDto>(p)).ToList();

            response.Data = dataDto;
            response.HttpResponseCode = 200;
            return response;
        }

        public async Task<ServiceResponse<List<EmployeeGetDto>>> GetPersonJobs()
        {
            var response = new ServiceResponse<List<EmployeeGetDto>>();

            var id = _httpContextAccessor.HttpContext!.User.FindFirstValue("ID");
            if(id == null)
            {
                response.HttpResponseCode = 404;
                response.Message = "Id claim Not Found!";
                return response;
            }

            var person = await _context.People
                .FirstOrDefaultAsync
                    (p => p.Id.ToString() == id);

            if(person == null)
            {
                response.HttpResponseCode = 404;
                response.Message = "User Not Found!";
                return response;
            }
            
            var data = await _context.Employees
                .Include(e => e.Authority)
                .Include(e => e.Organization)
                .Include(e => e.EmployeeType)
                .Where(e => e.PersonId.ToString() == id)
                .Select(e => _mapper.Map<EmployeeGetDto>(e))
                .ToListAsync();


            response.Data = data;
            response.HttpResponseCode = 200;
            return response;
        }
    }
}
