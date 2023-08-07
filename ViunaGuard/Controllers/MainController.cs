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

        [HttpGet("GetTodayEntrances")]
        public async Task<ActionResult<List<Entrance>>> GetTodayEntrances(int doorId)
        {
            var entrances = await context.Entrances.Where(e => e.Time.Date == DateTime.Now.Date && e.DoorId == doorId)
                .Include(e => e.Person)
                .Include(e => e.Car)
                .Include(e => e.Guard)
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
