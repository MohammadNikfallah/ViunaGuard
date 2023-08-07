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
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly DataContext context;
        private readonly IMapper mapper;

        public UserController(IUserService userService, DataContext context, IMapper mapper)
        {
            this.userService = userService;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpPost("postCar")]
        public async Task<ActionResult<Person>> PostCar(Car car, int id)
        {
            Person? p = await context.People.FirstOrDefaultAsync(p => p.Id == id);
            if (p == null)
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

        [HttpGet("GetOrganizationSignatureNeed")]
        public async Task<ActionResult<List<SignatureNeedForEntrancePermission>>> GetOrganizationSignatureNeed
            (int organizationId)
        {
            var response = await context.SignatureNeedForEntrancePermissions
                .Where(s => s.OrganizationId == organizationId)
                .Include(s => s.MinAuthority)
                .Select(s => mapper.Map<SignatureNeedGetDto>(s))
                .ToListAsync();
            return Ok(response);
        }

        [HttpGet("GetOrganizationAuthorities")]
        public async Task<ActionResult<List<Authority>>> GetOrganizationAuthorities(int organizationId)
        {
            var response = await context.Authorities
                .Where(s => s.OrganizationId == organizationId)
                .ToListAsync();
            return Ok(response);
        }
    }
}
