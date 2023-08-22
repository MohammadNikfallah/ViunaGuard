using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using ViunaGuard.Dtos;
using ViunaGuard.Models;

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

        public async Task<ServiceResponse<List<EntranceGetDto>>> GetEntrances(DateOnly startDate, DateOnly endDate, int doorId
            , int personId, int carId, int guardId, int enterOrExitId)
        {
            var response = new ServiceResponse<List<EntranceGetDto>>();

            var employeeId = _httpContextAccessor.HttpContext!.User.FindFirstValue("EmployeeId");
            var employee = await _context.Employees.FindAsync(int.Parse(employeeId));

            if (employee!.PersonId.ToString() != _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "ID").Value)
            {
                response.HttpResponseCode = 400;
                response.Message = "Something wrong with EmployeeID";
                return response;
            }

            var entrances = _context.Entrances.Where(e => e.OrganizationId == employee.OrganizationId);
            if (startDate != DateOnly.MinValue && endDate != DateOnly.MinValue)
                entrances = entrances.Where(e => e.Time.Date >= startDate.ToDateTime(TimeOnly.MinValue).Date 
                    && e.Time.Date < endDate.ToDateTime(TimeOnly.MaxValue).Date);
            if (doorId > 0)
                entrances = entrances.Where(e => e.DoorId == doorId);
            if (personId > 0)
                entrances = entrances.Where(e => e.PersonId == personId);
            if (carId > 0)
                entrances = entrances.Where(e => e.CarId == carId);
            if (guardId > 0)
                entrances = entrances.Where(e => e.GuardId == guardId);
            if (enterOrExitId > 0)
                entrances = entrances.Where(e => e.EnterOrExitId == enterOrExitId);

            response.Data = await entrances.Include(e => e.Person)
                .Include(e => e.Car)
                .Include(e => e.EnterOrExit)
                .Include(e => e.Door)
                .Select(e => _mapper.Map<EntranceGetDto>(e)).ToListAsync();

            response.HttpResponseCode = 200;
            return response;
        }
        public async Task<ServiceResponse<List<EntranceGroupGetDto>>> GetGroupEntrances(DateOnly startDate, DateOnly endDate, int doorId
            , int personId, int carId, int guardId, int enterOrExitId)
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
                .Where(e => !e.Entrances.IsNullOrEmpty() && e.Entrances[0].OrganizationId == employee.OrganizationId);

            if (startDate != DateOnly.MinValue && endDate != DateOnly.MinValue)
                entranceGroup = entranceGroup.Where(e => e.Entrances[0].Time.Date >= startDate.ToDateTime(TimeOnly.MinValue).Date 
                    && e.Entrances[0].Time.Date < endDate.ToDateTime(TimeOnly.MaxValue).Date);
            if (doorId > 0)
                entranceGroup = entranceGroup.Where(e => e.Entrances[0].DoorId == doorId);
            if (personId > 0)
                entranceGroup = entranceGroup.Where(e => e.Entrances[0].PersonId == personId);
            if (carId > 0)
                entranceGroup = entranceGroup.Where(e => e.Entrances[0].CarId == carId);
            if (guardId > 0)
                entranceGroup = entranceGroup.Where(e => e.Entrances[0].GuardId == guardId);
            if (enterOrExitId > 0)
                entranceGroup = entranceGroup.Where(e => e.Entrances[0].EnterOrExitId == enterOrExitId);

            response.Data = await entranceGroup
                .Select(e => _mapper.Map<EntranceGroupGetDto>(e)).ToListAsync();

            response.HttpResponseCode = 200;
            return response;
        }


        public async Task<ServiceResponse<Entrance>> PostEntrance(EntrancePostDto entrancePostDto)
        {
            var response = new ServiceResponse<Entrance>();

            var entrance = _mapper.Map<Entrance>(entrancePostDto);

            var guardId = _httpContextAccessor.HttpContext!.User.FindFirstValue("GuardId");
            var guardEmployee = await _context.Employees.FindAsync(guardId);

            if (guardEmployee == null)
            {
                response.HttpResponseCode = 400;
                response.Message = "Something is wrong with EmployeeId";
                return response;
            }

            if (guardEmployee.OrganizationId != entrance.OrganizationId)
            {
                response.HttpResponseCode = 400;
                response.Message = "You cant add entrance on this organization!";
                return response;
            }

            entrance.GuardId = int.Parse(guardId!);

            await _context.Entrances.AddAsync(entrance);
            await _context.SaveChangesAsync();

            response.HttpResponseCode = 200;
            response.Data = entrance;
            return response;
        }

        public async Task<ServiceResponse<object>> PostSameGroupEntrances
            (EntranceGroupPostDto entranceGroupPost)
        {

            var response = new ServiceResponse<object>();

            var entranceGroup = new EntranceGroup();

            var entranceGroupContext = await _context.EntranceGroups.AddAsync(entranceGroup);
            await _context.SaveChangesAsync();
            var id = entranceGroupContext.Entity.Id;


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

            foreach (var entranceDto in entranceGroupPost.Entrances)
            {
                var entrance = _mapper.Map<Entrance>(entranceDto);

                if (guardEmployee.OrganizationId != entrance.OrganizationId)
                {
                    _context.EntranceGroups.Remove(await _context.EntranceGroups.FindAsync(id));
                    response.HttpResponseCode = 400;
                    response.Message = "You cant add entrance on this organization!";
                    return response;
                }
                entrance.GuardId = int.Parse(guardId!);
                entrance.EntranceGroupId = id;

                await _context.Entrances.AddAsync(entrance);
            }

            await _context.SaveChangesAsync();
            response.HttpResponseCode = 200;
            response.Data = new();
            return response;
        }
    }
}
