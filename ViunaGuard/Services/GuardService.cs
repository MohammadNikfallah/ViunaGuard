using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ViunaGuard.Services
{
    public class GuardService : IGuardService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IEmployeeService _employeeService;

        public GuardService(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration
            ,IEmployeeService employeeService)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _employeeService = employeeService;
        }

        private bool CheckGuardId(Employee? employee, ServiceResponse response, out ServiceResponse serviceResponse)
        {
            if (employee == null)
            {
                response.HttpResponseCode = 400;
                response.Message = "There is no Employee with this id";
                {
                    serviceResponse = response;
                    return true;
                }
            }

            var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue("ID");
            if (employee.PersonId != userId)
            {
                response.HttpResponseCode = 400;
                response.Message = "You cant access this employee";
                {
                    serviceResponse = response;
                    return true;
                }
            }
            
            if (employee.EmployeeTypeId != _configuration.GetValue<int>("Constants:GuardEmployeeTypeId"))
            {
                response.HttpResponseCode = 400;
                response.Message = "You need to be Guard to access this Section";
                {
                    serviceResponse = response;
                    return true;
                }
            }

            serviceResponse = new ServiceResponse();
            return false;
        }
        
        public async Task<ServiceResponse<List<EntranceGroupGetDto>>> GetEntrances(DateOnly startDate, DateOnly endDate, int doorId
            , int guardId, int enterOrExitId, int employeeId, int entranceCount)
        {
            var response = new ServiceResponse<List<EntranceGroupGetDto>>();
            var employee = await _context.Employees
                .Include(e => e.UserAccessRole)
                .ThenInclude(e => e.UserAccess)
                .FirstOrDefaultAsync(e => e.Id == employeeId);
            if (CheckGuardId(employee, response, out var serviceResponse))
            {
                response.HttpResponseCode = serviceResponse.HttpResponseCode;
                response.Message = serviceResponse.Message;
                return response;
            }

            var entranceGroup = _context.EntranceGroups
                .Include(e => e.Entrances)
                .ThenInclude(e => e.Person)
                .Include(e => e.EnterOrExit)
                .Where(e => e.Entrances.Count>0 && e.OrganizationId == employee!.OrganizationId);

            if (startDate != DateOnly.MinValue && endDate != DateOnly.MinValue)
                entranceGroup = entranceGroup.Where(e => e.Time >= startDate.ToDateTime(TimeOnly.MinValue) 
                    && e.Time < endDate.ToDateTime(TimeOnly.MaxValue));
            if (!employee!.UserAccessRole.UserAccess.CanSeeOtherDoorsEntrances)
                entranceGroup = entranceGroup.Where(e => e.GuardId == employeeId);
            if (doorId > 0)
                entranceGroup = entranceGroup.Where(e => e.DoorId == doorId);
            if (enterOrExitId > 0)
                entranceGroup = entranceGroup.Where(e => e.EnterOrExitId == enterOrExitId);
            if (!employee.UserAccessRole.UserAccess.CanSeeOtherGuardsEntrances)
                entranceGroup = entranceGroup.Where(e => e.GuardId == employeeId);
            else if (guardId > 0)
                entranceGroup = entranceGroup.Where(e => e.GuardId == guardId);

            response.Data = await entranceGroup
                .OrderBy(e => e.Time).Take(entranceCount).Select(e => _mapper.Map<EntranceGroupGetDto>(e)).ToListAsync();

            response.HttpResponseCode = 200;
            return response;
        }

        public async Task<ServiceResponse<EntrancePermissionCheckDto>> CheckEntrancePermission(string nationalId, int employeeId)
        {
            var response = new ServiceResponse<EntrancePermissionCheckDto>()
            {
                Data = new EntrancePermissionCheckDto()
            };
            var employee = await _context.Employees.FindAsync(employeeId);
            if (CheckGuardId(employee, response, out var serviceResponse))
            {
                response.HttpResponseCode = serviceResponse.HttpResponseCode;
                response.Message = serviceResponse.Message;
                return response;
            }

            var person = await _context.People
                .Include(p => p.Cars)
                .Include(p => p.Jobs)
                .ThenInclude(j => j.EmployeeType)
                .Include(p => p.Jobs)
                .ThenInclude(j => j.UserAccessRole)
                .ThenInclude(u => u.UserAccess)
                .Include(p => p.EntrancePermissions)
                .ThenInclude(e => e.Signatures)
                .Include(e => e.BannedFrom)
                .FirstOrDefaultAsync(p => p.NationalId == nationalId);
            if (person == null)
            {
                response.HttpResponseCode = 404;
                response.Message = "Person Not Found";
                return response;
            }
            var job = person.Jobs
                .FirstOrDefault(j => j.OrganizationId == employee!.OrganizationId);
            response.Data.Job = _mapper.Map<EmployeeGetDto>(job);
            person.Jobs = new();
            response.Data.Person = _mapper.Map<PersonGetDto>(person);
            response.Data.EntrancePermissions =
                person.EntrancePermissions
                    .Where(e => e.OrganizationId == employee!.OrganizationId && e.PermissionGranted)
                    .Select(e => _mapper.Map<EntrancePermissionGetDto>(e)).ToList();
            if (response.Data.Job != null)
            {
                var shiftResponse = await _employeeService.GetEmployeeShifts(response.Data.Job.Id);
                if (shiftResponse.HttpResponseCode == 200)
                    response.Data.Shifts = shiftResponse.Data;
            }

            response.Data.DoesHavePermission = false;

            if (job != null && job.UserAccessRole.UserAccess.AlwaysHaveEntrancePermission)
                response.Data.DoesHavePermission = true;
            else if(response.Data.Shifts != null)
                foreach (var shift in response.Data.Shifts.TodayShifts)
                {
                    if (shift.FinishTime > DateTime.Now)
                        if (shift.StartTime < DateTime.Now)
                            response.Data.DoesHavePermission = true;
                }

            if (response.Data.DoesHavePermission == false)
                foreach (var ep in response.Data.EntrancePermissions)
                {
                    if(ep.EndValidityTime > DateTime.Now)
                        if (ep.StartValidityTime < DateTime.Now)
                            response.Data.DoesHavePermission = true;
                }
            
            if (person.BannedFrom.Exists(o => o.Id == employee!.OrganizationId))
            {
                response.Data.IsInBlackList = true;
                response.Data.DoesHavePermission = false;
            }
            response.HttpResponseCode = 200;
            return response;
        }

        public async Task<ServiceResponse<CheckExitPermissionDto>> CheckExitPermission(string nationalId, int employeeId)
        {
            var response = new ServiceResponse<CheckExitPermissionDto>()
            {
                Data = new CheckExitPermissionDto()
            };
            var employee = await _context.Employees.FindAsync(employeeId);
            if (CheckGuardId(employee, response, out var serviceResponse))
            {
                response.HttpResponseCode = serviceResponse.HttpResponseCode;
                response.Message = serviceResponse.Message;
                return response;
            }
            
            var person = await _context.People
                .Include(p => p.Cars)
                .Include(p => p.VisitedPlaces)
                .ThenInclude(v => v.OrganizationPlace)
                .Include(p => p.Jobs)
                .ThenInclude(j => j.EmployeeType)
                .Include(p => p.Jobs)
                .ThenInclude(j => j.UserAccessRole)
                .ThenInclude(u => u.UserAccess)
                .Include(p => p.EntrancePermissions)
                .ThenInclude(e => e.Signatures)
                .Include(e => e.BannedFrom)
                .FirstOrDefaultAsync(p => p.NationalId == nationalId);
            if (person == null)
            {
                response.HttpResponseCode = 404;
                response.Message = "Person Not Found";
                return response;
            }
            var job = person.Jobs
                .FirstOrDefault(j => j.OrganizationId == employee!.OrganizationId);
            response.Data!.Job = _mapper.Map<EmployeeGetDto>(job);
            person.Jobs = new();
            response.Data.Person = _mapper.Map<PersonGetDto>(person);
            response.Data.EntrancePermissions =
                person.EntrancePermissions
                    .Where(e => e.OrganizationId == employee!.OrganizationId && e.PermissionGranted)
                    .Select(e => _mapper.Map<EntrancePermissionGetDto>(e))
                    .OrderByDescending(e => e.StartValidityTime)
                    .Take(3).ToList();
            
            response.Data.VisitedPlaces = person.VisitedPlaces
                .Where(e => e.OrganizationPlace!.OrganizationId == employee!.OrganizationId && e.VisitTime.Date == DateTime.Now.Date)
                .OrderByDescending(e => e.VisitTime)
                .Take(3).ToList();

            if (response.Data.Job != null)
            {
                response.Data.DoesHavePermission = true;
            }

            response.HttpResponseCode = 200;
            return response;
        }

        public async Task<ServiceResponse> PostPerson(PersonPostDto personDto, int employeeId)
        {
            var response = new ServiceResponse();
            var employee = await _context.Employees.FindAsync(employeeId);
            if (CheckGuardId(employee, response, out var serviceResponse)) return serviceResponse;
            
            var person = _mapper.Map<Person>(personDto);
            var personTest = _context.People.FirstOrDefault(p => p.NationalId == person.NationalId);
            if (personTest != null)
            {
                response.HttpResponseCode = 400;
                response.Message = "Person With this nationalId Exists";
                return response;
            }
            var personAdditionalInfo = new PersonAdditionalInfo();
            person.PersonAdditionalInfo = personAdditionalInfo;
            await _context.PersonAdditionalInfos.AddAsync(personAdditionalInfo);
            await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();

            response.HttpResponseCode=200;
            return response;
        }


        public async Task<ServiceResponse> PostEntrances
            (EntranceGroupPostDto entranceGroupPost, int employeeId)
        {
            var response = new ServiceResponse();
            var employee = await _context.Employees.FindAsync(employeeId);
            if (CheckGuardId(employee, response, out var serviceResponse)) return serviceResponse;

            var entranceGroup = _mapper.Map<EntranceGroup>(entranceGroupPost);

            entranceGroup.GuardId = employeeId;
            entranceGroup.OrganizationId = employee!.OrganizationId;

            await _context.EntranceGroups.AddAsync(entranceGroup);
            await _context.SaveChangesAsync();

            response.HttpResponseCode = 200;
            return response;
        }
    }
}
