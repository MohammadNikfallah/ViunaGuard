using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Humanizer;
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
        
        public async Task<ServiceResponse<object>> PostPeriodicShift(PeriodicShiftPostDto shift)
        {
            var response = new ServiceResponse<object>();

            await _context.EmployeePeriodicShifts.AddAsync(_mapper.Map<EmployeePeriodicShift>(shift));
            await _context.SaveChangesAsync();
            
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

        public async Task<ServiceResponse<EmployeeShiftGetDto>> GetCurrentShift(int employeeId)
        {
            var response = new ServiceResponse<EmployeeShiftGetDto>();
            var time = DateTime.Now;
            var id = _httpContextAccessor.HttpContext!.User.FindFirstValue("ID");
            var person = await _context.People.FindAsync(int.Parse(id));
            if(person!.Jobs.Any(j => j.Id == employeeId))
            {
                response.HttpResponseCode = 400;
                response.Message = "Something wrong with EmployeeId";
                return response;
            }
            
            var periodicShift = _context.EmployeePeriodicShifts
                .Include(e => e.GuardDoor)
                .Where(e => e.EmployeeId == employeeId);
            var todayPeriodicShifts = periodicShift
                .Where(e => ((time.Date.DayOfYear - e.StartTime.Date.DayOfYear) % e.PeriodDayRange) == 0);
            var nowPeriodicShift = await todayPeriodicShifts
                .FirstOrDefaultAsync(p => time.TimeOfDay > p.StartTime.TimeOfDay && time.TimeOfDay < p.FinishTime.TimeOfDay);
            if (nowPeriodicShift != null)
            {
                response.HttpResponseCode = 200;
                response.Data = _mapper.Map<EmployeeShiftGetDto>(nowPeriodicShift);
                return response;
            }
            
            var shift = _context.EmployeeShifts
                .Include(e => e.GuardDoor)
                .Where(e => e.EmployeeId == employeeId);
            var nowShift = await shift
                .FirstOrDefaultAsync(e => time.Date < e.FinishTime.Date && time.Date > e.StartTime.Date);
            if (nowShift != null)
            {
                response.HttpResponseCode = 200;
                response.Data = _mapper.Map<EmployeeShiftGetDto>(nowShift);
                return response;
            }
            
            response.HttpResponseCode = 404;
            response.Message = "There is no current shift for this employee";
            return response;
        }
        
        public async Task<ServiceResponse<TwoShiftGetDto>> GetPersonShifts(int employeeId)
        {
            var response = new ServiceResponse<TwoShiftGetDto>();
            response.Data = new TwoShiftGetDto();
            var time = DateTime.Now;
            var id = _httpContextAccessor.HttpContext!.User.FindFirstValue("ID");
            var person = await _context.People.FindAsync(int.Parse(id));
            if(person!.Jobs.Any(j => j.Id == employeeId))
            {
                response.HttpResponseCode = 400;
                response.Message = "Something wrong with EmployeeId";
                return response;
            }

            response.Data.TodayShifts.AddRange(await GetDayShifts(time, employeeId));
            response.Data.TomorrowShifts.AddRange(await GetDayShifts(time.AddDays(1), employeeId));
            response.HttpResponseCode = 200;
            return response;
        }

        public async Task<List<EmployeeShiftGetDto>> GetDayShifts(DateTime time, int employeeId)
        {
            var Result = new List<EmployeeShiftGetDto>();
            var periodicShift = _context.EmployeePeriodicShifts
                .Include(e => e.GuardDoor)
                .Where(e => e.EmployeeId == employeeId);
            var todayPeriodicShifts = periodicShift
                .Where(e => ((time.Date.DayOfYear - e.StartTime.Date.DayOfYear) % e.PeriodDayRange) == 0);
            
            var tempResult = await todayPeriodicShifts.Select(e => _mapper.Map<EmployeeShiftGetDto>(e)).ToListAsync();
            for (int i = 0; i < tempResult.Count; i++)
            {
                tempResult[i].StartTime = tempResult[i].StartTime.AddDays(time.DayOfYear - tempResult[i].StartTime.DayOfYear);
                tempResult[i].FinishTime = tempResult[i].FinishTime.AddDays(time.DayOfYear - tempResult[i].FinishTime.DayOfYear);
            }
            Result.AddRange(tempResult);

            var shift = _context.EmployeeShifts
                .Include(e => e.GuardDoor)
                .Where(e => e.EmployeeId == employeeId);
            var nowShift = shift.Where(e => time.Date <= e.FinishTime.Date && time.Date >= e.StartTime.Date);
            tempResult = await nowShift.Select(e => _mapper.Map<EmployeeShiftGetDto>(e)).ToListAsync();
            for (int i = 0; i < tempResult.Count; i++)
            {
                if (tempResult[i].StartTime.AddDays(1).Date <= time.Date)
                    tempResult[i].StartTime = DateOnly.FromDateTime(time).ToDateTime(TimeOnly.MinValue);
                if (tempResult[i].FinishTime.AddDays(-1).Date >= time.Date)
                    tempResult[i].FinishTime = DateOnly.FromDateTime(time).ToDateTime(TimeOnly.MaxValue);
            }
            Result.AddRange(tempResult);

            return Result;
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
