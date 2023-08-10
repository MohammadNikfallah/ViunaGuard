using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ViunaGuard.Dtos;
using ViunaGuard.Models;
using ViunaGuard.Models.Enums;
using ViunaGuard.Services;

namespace ViunaGuard.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class GuardController : ControllerBase
    {
        private readonly IGuardService guardService;
        private readonly DataContext context;
        private readonly IMapper mapper;

        public GuardController(IGuardService guardService, DataContext context, IMapper mapper)
        {
            this.guardService = guardService;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("GetEntrances")]
        public async Task<ActionResult<List<Entrance>>> GetEntrances(DateOnly date, [Required] int employeeId
            , int doorId, int personId, int organizationId, int carId, int guardId, int entranceTypeId, int enterOrExitId)
        {
            var employee = await context.Employees.FirstAsync(e => e.Id == employeeId);

            if (employee.PersonId.ToString() != HttpContext.User.Claims.First(c => c.Type == "ID").Value
                && employee.EmployeeTypeId != context.EmployeeTypes.First(e => e.Type == "Guard").Id)
                return BadRequest("Something wrong with EmployeeID");

            var entrances = context.Entrances.Where(e => e.OrganizationId == employee.OrganizationId);
            if (date != DateOnly.MinValue)
                entrances = entrances.Where(e => e.Time.Date == date.ToDateTime(TimeOnly.MinValue).Date);
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

            await entrances.Include(e => e.Person)
                .Include(e => e.Car)
                .Include(e => e.Guard)
                .Include(e => e.EntranceType)
                .Include(e => e.EnterOrExit)
                .Include(e => e.Door)
                .Select(e => mapper.Map<EntranceGetDto>(e)).ToListAsync();
            return Ok(entrances);
        }

        [HttpPost("PostEntrance")]
        public async Task<ActionResult<Entrance>> PostEntrance(EntrancePostDto entrancePostDto, int employeeId)
        {
            var entrance = mapper.Map<Entrance>(entrancePostDto);
            var person = await context.People
                .Include(s => s.Jobs)
                .FirstAsync(p => p.Id.ToString() == HttpContext.User.Claims.First(c => c.Type == "ID").Value);

            var org = person.Jobs
                .FirstOrDefault(j => j.OrganizationId == entrance.OrganizationId
                        && j.EmployeeTypeId == context.EmployeeTypes.First(e => e.Type == "Guard").Id);

            if(org == null)
            {
                return BadRequest("Something wrong with organizationId");
            }

            await context.Entrances.AddAsync(entrance);
            await context.SaveChangesAsync();
            return Ok(entrance);
        }
    }
}
