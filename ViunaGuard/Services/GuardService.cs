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

        public GuardService(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
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
            if (employee.PersonId != int.Parse(userId!))
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
        
        // public async Task<ServiceResponse<List<EntranceGetDto>>> GetEntrances(DateOnly startDate, DateOnly endDate, int doorId
        //     , int personId, int carId, int guardId, int enterOrExitId)
        // {
        //     var response = new ServiceResponse<List<EntranceGetDto>>();
        //
        //     var employeeId = _httpContextAccessor.HttpContext!.User.FindFirstValue("EmployeeId");
        //     var employee = await _context.Employees.FindAsync(int.Parse(employeeId));
        //
        //     if (employee!.PersonId.ToString() != _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "ID").Value)
        //     {
        //         response.HttpResponseCode = 400;
        //         response.Message = "Something wrong with EmployeeID";
        //         return response;
        //     }
        //
        //     var entrances = _context.Entrances.Where(e => e.OrganizationId == employee.OrganizationId);
        //     if (startDate != DateOnly.MinValue && endDate != DateOnly.MinValue)
        //         entrances = entrances.Where(e => e.Time.Date >= startDate.ToDateTime(TimeOnly.MinValue).Date 
        //             && e.Time.Date < endDate.ToDateTime(TimeOnly.MaxValue).Date);
        //     if (doorId > 0)
        //         entrances = entrances.Where(e => e.DoorId == doorId);
        //     if (personId > 0)
        //         entrances = entrances.Where(e => e.PersonId == personId);
        //     if (carId > 0)
        //         entrances = entrances.Where(e => e.CarId == carId);
        //     if (guardId > 0)
        //         entrances = entrances.Where(e => e.GuardId == guardId);
        //     if (enterOrExitId > 0)
        //         entrances = entrances.Where(e => e.EnterOrExitId == enterOrExitId);
        //
        //     response.Data = await entrances.Include(e => e.Person)
        //         .Include(e => e.Car)
        //         .Include(e => e.EnterOrExit)
        //         .Include(e => e.Door)
        //         .Select(e => _mapper.Map<EntranceGetDto>(e)).ToListAsync();
        //
        //     response.HttpResponseCode = 200;
        //     return response;
        // }
        public async Task<ServiceResponse<List<EntranceGroupGetDto>>> GetEntrances(DateOnly startDate, DateOnly endDate, int doorId
            , int guardId, int enterOrExitId, int employeeId)
        {
            var response = new ServiceResponse<List<EntranceGroupGetDto>>();
            var employee = await _context.Employees.FindAsync(employeeId);
            if (CheckGuardId(employee, response, out var serviceResponse))
            {
                response.HttpResponseCode = serviceResponse.HttpResponseCode;
                response.Message = serviceResponse.Message;
                return response;
            }

            var entranceGroup = _context.EntranceGroups
                .Include(e => e.Entrances)
                .ThenInclude(e => e.Person)
                .Where(e => e.Entrances.Count>0 && e.OrganizationId == employee!.OrganizationId);

            if (startDate != DateOnly.MinValue && endDate != DateOnly.MinValue)
                entranceGroup = entranceGroup.Where(e => e.Time >= startDate.ToDateTime(TimeOnly.MinValue) 
                    && e.Time < endDate.ToDateTime(TimeOnly.MaxValue));
            if (doorId > 0)
                entranceGroup = entranceGroup.Where(e => e.DoorId == doorId);
            if (guardId > 0)
                entranceGroup = entranceGroup.Where(e => e.GuardId == guardId);
            if (enterOrExitId > 0)
                entranceGroup = entranceGroup.Where(e => e.EnterOrExitId == enterOrExitId);

            response.Data = await entranceGroup
                .Select(e => _mapper.Map<EntranceGroupGetDto>(e)).ToListAsync();

            response.HttpResponseCode = 200;
            return response;
        }
        

        public async Task<ServiceResponse<object>> PostEntrances
            (EntranceGroupPostDto entranceGroupPost, int employeeId)
        {

            var response = new ServiceResponse<object>();
            var guardEmployee = await _context.Employees.FindAsync(employeeId);
            if (CheckGuardId(guardEmployee, response, out var serviceResponse))
            {
                response.HttpResponseCode = serviceResponse.HttpResponseCode;
                response.Message = serviceResponse.Message;
                return response;
            }

            var entranceGroup = _mapper.Map<EntranceGroup>(entranceGroupPost);

            entranceGroup.GuardId = employeeId;
            entranceGroup.OrganizationId = guardEmployee!.OrganizationId;

            await _context.EntranceGroups.AddAsync(entranceGroup);
            await _context.SaveChangesAsync();

            response.HttpResponseCode = 200;
            response.Data = new();
            return response;
        }
    }
}
