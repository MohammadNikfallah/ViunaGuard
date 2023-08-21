using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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
            , int personId, int organizationId, int carId, int guardId, int entranceTypeId, int enterOrExitId)
        {
            var response = new ServiceResponse<List<EntranceGetDto>>();

            var employee = await _context.Employees.FindAsync(_httpContextAccessor.HttpContext!.User.FindFirstValue("GuardId"));

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
            if (entranceTypeId > 0)
                entrances = entrances.Where(e => e.EntranceTypeId == entranceTypeId);
            if (enterOrExitId > 0)
                entrances = entrances.Where(e => e.EnterOrExitId == enterOrExitId);

            response.Data = await entrances.Include(e => e.Person)
                .Include(e => e.Car)
                .Include(e => e.Guard)
                .Include(e => e.EntranceType)
                .Include(e => e.EnterOrExit)
                .Include(e => e.Door)
                .Select(e => _mapper.Map<EntranceGetDto>(e)).ToListAsync();

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

            var entranceGroup = new EntranceGroup
            {
                OrganizationId = entranceGroupPost.Entrances[0].OrganizationId,
                CarId = entranceGroupPost.Entrances[0].CarId,
                DriverId = entranceGroupPost.DriverId
            };

            var entranceGroupContext = await _context.EntranceGroups.AddAsync(entranceGroup);
            var id = entranceGroupContext.Entity.Id;


            var guardId = _httpContextAccessor.HttpContext!.User.FindFirstValue("GuardId");
            var guardEmployee = await _context.Employees.FindAsync(guardId);

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
            return response;
        }
    }
}
