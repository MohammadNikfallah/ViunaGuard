using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViunaGuard.Dtos;
using ViunaGuard.Models;
using ViunaGuard.Services;

namespace ViunaGuard.Controllers
{
    [Route("[controller]")]
    [ApiController]
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

        [HttpGet("GetCar")]
        public async Task<ActionResult<List<Car>>> GetCar()
        {
            var cars = await context.Cars.ToListAsync();
            return Ok(cars);
        }


        [HttpGet("GetEntrances")]
        public async Task<ActionResult<List<Entrance>>> GetEntrances(DateOnly date
            , int doorId, int personId, int organizationId, int carId, int guardId, int entranceTypeId, int enterOrExitId)
        {
            var entrances = context.Entrances.Where(e => true);
            if (date != DateOnly.MinValue)
                entrances = entrances.Where(e => e.Time.Date == date.ToDateTime(TimeOnly.MinValue).Date);
            if (doorId > 0)
                entrances = entrances.Where(e => e.DoorId == doorId);
            if (personId > 0)
                entrances = entrances.Where(e => e.PersonId == personId);
            if (organizationId > 0)
                entrances = entrances.Where(e => e.OrganizationId == organizationId);
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
        public async Task<ActionResult<Entrance>> PostEntrance(EntrancePostDto entrancePostDto)
        {
            var entrance = mapper.Map<Entrance>(entrancePostDto);
            await context.Entrances.AddAsync(entrance);
            await context.SaveChangesAsync();
            return Ok(entrance);
        }
    }
}
