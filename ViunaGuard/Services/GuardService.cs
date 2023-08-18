using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using ViunaGuard.Dtos;
using ViunaGuard.Models;

namespace ViunaGuard.Services
{
    public class GuardService : IGuardService
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GuardService(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<List<EntranceGetDto>>> GetEntrances(DateOnly startDate, DateOnly endDate, int doorId
            , int personId, int organizationId, int carId, int guardId, int entranceTypeId, int enterOrExitId)
        {
            var response = new ServiceResponse<List<EntranceGetDto>>();

            var employee = await context.Employees.FindAsync(httpContextAccessor.HttpContext.User.FindFirstValue("EmployeeId"));

            if (employee.PersonId.ToString() != httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "ID").Value)
            {
                response.HttpResponseCode = 400;
                response.Message = "Something wrong with EmployeeID";
                return response;
            }

            var entrances = context.Entrances.Where(e => e.OrganizationId == employee.OrganizationId);
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
                .Select(e => mapper.Map<EntranceGetDto>(e)).ToListAsync();

            response.HttpResponseCode = 200;
            return response;
        }


        public async Task<ServiceResponse<Entrance>> PostEntrance(EntrancePostDto entrancePostDto, int employeeId)
        {
            var response = new ServiceResponse<Entrance>();

            var entrance = mapper.Map<Entrance>(entrancePostDto);
            var person = await context.People
                .Include(s => s.Jobs)
                .FirstAsync(p => p.Id.ToString() == httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "ID").Value);

            var org = person.Jobs
                .FirstOrDefault(j => j.OrganizationId == entrance.OrganizationId
                        && j.EmployeeTypeId == context.EmployeeTypes.First(e => e.Type == "Guard").Id);

            if (org == null)
            {
                response.HttpResponseCode = 400;
                response.Message = "Something wrong with organizationId";
                return response;
            }

            await context.Entrances.AddAsync(entrance);
            await context.SaveChangesAsync();

            response.HttpResponseCode = 200;
            response.Data = entrance;
            return response;
        }

        public async Task<ServiceResponse<object>> PostSameGroupEntrances
            (List<EntrancePostDto> entrancePostDtos, int driverID, [Required] int employeeId)
        {
            var response = new ServiceResponse<object>();

            var person = await context.People
                .Include(s => s.Jobs)
                .FirstAsync(p => p.Id.ToString() == httpContextAccessor.HttpContext!.User.FindFirst("ID")!.Value);

            var entranceGroup = new EntranceGroupPostDto
            {
                OrganizationId = entrancePostDtos[0].OrganizationId,
                CarId = entrancePostDtos[0].CarId,
                DriverId = driverID
            };

            var entranceGroupContext = await context.EntranceGroups.AddAsync(mapper.Map<EntranceGroup>(entranceGroup));
            var id = entranceGroupContext.Entity.Id;

            

            foreach (var entranceDto in entrancePostDtos)
            {
                var entrance = mapper.Map<Entrance>(entranceDto);
                var org = person.Jobs
                    .FirstOrDefault(j => j.OrganizationId == entrance.OrganizationId
                            && j.EmployeeTypeId == context.EmployeeTypes.First(e => e.Type == "Guard").Id);

                entrance.GuardId = employeeId;
                entrance.EntranceGroupId = id;

                if (org == null)
                {
                    response.HttpResponseCode = 400;
                    response.Message = "Something wrong with organizationId";
                    return response;
                }

                await context.Entrances.AddAsync(entrance);
            }

            await context.SaveChangesAsync();
            response.HttpResponseCode = 200;
            return response;
        }
    }
}
