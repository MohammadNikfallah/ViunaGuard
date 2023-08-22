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

        public GuardService(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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
            , int guardId, int enterOrExitId)
        {
            var response = new ServiceResponse<List<EntranceGroupGetDto>>();

            var employeeId = _httpContextAccessor.HttpContext!.User.FindFirstValue("EmployeeId");
            var employee = await _context.Employees.FindAsync(int.Parse(employeeId));

            if (employee!.PersonId.ToString() != _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "ID").Value)
            {
                response.HttpResponseCode = 400;
                response.Message = "Something wrong with EmployeeID";
                return response;
            }

            var entranceGroup = _context.EntranceGroups
                .Include(e => e.Entrances)
                .ThenInclude(e => e.Person)
                .Where(e => e.Entrances.Count>0 && e.OrganizationId == employee.OrganizationId);

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
            (EntranceGroupPostDto entranceGroupPost)
        {

            var response = new ServiceResponse<object>();

            var entranceGroup = _mapper.Map<EntranceGroup>(entranceGroupPost);
            
            var guardId = _httpContextAccessor.HttpContext!.User.FindFirstValue("EmployeeId");
            var guardEmployee = await _context.Employees.FindAsync(int.Parse(guardId));
            File.AppendAllText("log","guardId :");
            File.AppendAllText("log",guardId + "\n");   
            
            if (guardEmployee == null)
            {
                response.HttpResponseCode = 400;
                response.Message = "Something is wrong with EmployeeId";
                return response;
            }
            entranceGroup.GuardId = int.Parse(guardId!);
            entranceGroup.OrganizationId = guardEmployee.OrganizationId;

            var entranceGroupContext = await _context.EntranceGroups.AddAsync(entranceGroup);
            await _context.SaveChangesAsync();

            await _context.SaveChangesAsync();
            response.HttpResponseCode = 200;
            response.Data = new();
            return response;
        }
    }
}
