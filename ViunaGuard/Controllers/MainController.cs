using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ViunaGuard.Dtos;
using ViunaGuard.Models;

namespace ViunaGuard.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class MainController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public MainController(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
            
        }

        [HttpPost("postCar")]
        public async Task<ActionResult<Person>> PostCar(Car car, int id)
        {
            Person? p = await context.People.FirstOrDefaultAsync(p => p.Id == id);
            if(p == null)
            {
                return BadRequest();
            }
            p.Cars.Add(car);
            var cars = await context.Cars.ToListAsync();
            var carInList = cars.FirstOrDefault(c => c.Id == car.Id);
            if (carInList != null)
                carInList.People.Add(p);
            else
            {
                car.People.Add(p);
                context.Cars.Add(car);
            }
            await context.SaveChangesAsync();
            return Ok(p);
        }

        [HttpPost("postPerson")]
        public async Task<ActionResult<List<Person>>> PostPerson(PersonPostDto personDto)
        {
            var person = mapper.Map<Person>(personDto);
            var personAdditionalInfo = new PersonAdditionalInfo();
            person.PersonAdditionalInfo = personAdditionalInfo;
            await context.PersonAdditionalInfos.AddAsync(personAdditionalInfo);
            await context.People.AddAsync(person);
            await context.SaveChangesAsync();
            var people = await context.People.ToListAsync();
            return Ok(people);
        }

        [HttpPost("PostShift")]
        public async Task<ActionResult<List<EmployeeShift>>> PostShift(EmployeeShiftPeriodicMonthly shift)
        {
            shift.Id = await context.EmployeeShiftsMonthly.MaxAsync(p => p.Id) + 1;
            await context.EmployeeShiftsMonthly.AddAsync(shift);
            await context.SaveChangesAsync();
            var shifts = await context.EmployeeShifts.ToListAsync();
            return Ok(shifts);
        }

        [HttpGet("GetPeople")]
        public async Task<ActionResult<List<Person>>> GetPeople()
        {
            var people = await context.People
                .Include(p => p.Cars)
                .Include(p => p.Jobs)
                .Select(p => mapper.Map<PersonGetDto>(p)).ToListAsync();
            return Ok(people);
        }

        [HttpGet("GetPerson")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var people = await context.People.FirstOrDefaultAsync(p => p.Id == id);
            return Ok(people);
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
            var entrances = context.Entrances.Where( e => true);
            if (date != DateOnly.MinValue)
                entrances = entrances.Where(e => e.Time.Date == date.ToDateTime(TimeOnly.MinValue).Date);
            if(doorId > 0)
                entrances = entrances.Where(e => e.DoorId == doorId);
            if(personId > 0)
                entrances = entrances.Where(e => e.PersonId == personId);
            if(organizationId > 0)
                entrances = entrances.Where(e => e.OrganizationId == organizationId);
            if(carId > 0)
                entrances = entrances.Where(e => e.CarId == carId);
            if(guardId > 0)
                entrances = entrances.Where(e => e.GuardId == guardId);
            if(entranceTypeId > 0)
                entrances = entrances.Where(e => e.EntranceTypeId == entranceTypeId);
            if(enterOrExitId > 0)
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

        [HttpGet("GetOrganizationSignatureNeed")]
        public async Task<ActionResult<List<SignatureNeedForEntrancePermission>>> GetOrganizationSignatureNeed
            (int organizationId)
        {
            var response = await context.SignatureNeedForEntrancePermissions
                .Where(s => s.OrganizationId == organizationId).ToListAsync();
            return Ok(response);
        }


    }
}
