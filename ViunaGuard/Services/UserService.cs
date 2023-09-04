using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

        public async Task<ServiceResponse> PostEntrancePermission(EntrancePermissionPostDto entrancePermissionPostDto)
        {
            var response = new ServiceResponse<PersonGetDto>();

            var entrancePermission = _mapper.Map<EntrancePermission>(entrancePermissionPostDto);
            var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue("ID");
            entrancePermission.PersonId = int.Parse(userId!);
            await _context.EntrancePermissions.AddAsync(entrancePermission);
            await _context.SaveChangesAsync();

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
            var person = await _context.People.FindAsync(int.Parse(id!));
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
            var response = new ServiceResponse<TwoShiftGetDto>
            {
                Data = new TwoShiftGetDto()
            };
            var time = DateTime.Now;
            var id = _httpContextAccessor.HttpContext!.User.FindFirstValue("ID");
            var person = await _context.People.FindAsync(int.Parse(id!));
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

        private async Task<List<EmployeeShiftGetDto>> GetDayShifts(DateTime time, int employeeId)
        {
            var result = new List<EmployeeShiftGetDto>();
            var periodicShift = _context.EmployeePeriodicShifts
                .Include(e => e.GuardDoor)
                .Where(e => e.EmployeeId == employeeId);
            var todayPeriodicShifts = periodicShift
                .Where(e => ((time.Date.DayOfYear - e.StartTime.Date.DayOfYear) % e.PeriodDayRange) == 0);
            
            var tempResult = await todayPeriodicShifts.Select(e => _mapper.Map<EmployeeShiftGetDto>(e)).ToListAsync();
            foreach (var t in tempResult)
            {
                t.StartTime = t.StartTime.AddDays(time.DayOfYear - t.StartTime.DayOfYear);
                t.FinishTime = t.FinishTime.AddDays(time.DayOfYear - t.FinishTime.DayOfYear);
            }
            result.AddRange(tempResult);

            var shift = _context.EmployeeShifts
                .Include(e => e.GuardDoor)
                .Where(e => e.EmployeeId == employeeId);
            var nowShift = shift.Where(e => time.Date <= e.FinishTime.Date && time.Date >= e.StartTime.Date);
            tempResult = await nowShift.Select(e => _mapper.Map<EmployeeShiftGetDto>(e)).ToListAsync();
            foreach (var t in tempResult)
            {
                if (t.StartTime.AddDays(1).Date <= time.Date)
                    t.StartTime = DateOnly.FromDateTime(time).ToDateTime(TimeOnly.MinValue);
                if (t.FinishTime.AddDays(-1).Date >= time.Date)
                    t.FinishTime = DateOnly.FromDateTime(time).ToDateTime(TimeOnly.MaxValue);
            }
            result.AddRange(tempResult);

            return result;
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
                .Include(e => e.EmployeeType)
                .Include(e => e.Organization)
                .Where(e => e.PersonId.ToString() == id)
                .Select(e => _mapper.Map<EmployeeGetDto>(e))
                .ToListAsync();


            response.Data = data;
            response.HttpResponseCode = 200;
            return response;
        }
    }
}
