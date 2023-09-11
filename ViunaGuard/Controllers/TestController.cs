using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ViunaGuard.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TestController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            
        }

        [HttpGet("GetCar")]
        public async Task<ActionResult<List<Car>>> GetCar()
        {
            var cars = await _context.Cars.ToListAsync();
            return Ok(cars);
        }


        [HttpGet("GetPeople")]
        public async Task<ActionResult<List<Person>>> GetPeople()
        {
            var people = await _context.People
                .Include(p => p.Cars)
                .Include(p => p.Jobs)
                .Select(p => _mapper.Map<PersonGetDto>(p)).ToListAsync();
            return Ok(people);
        }


        [HttpPost("PostPerson")]
        public async Task<ActionResult> PostPerson(Person person)
        {
            var ai = new PersonAdditionalInfo();
            var aic = await _context.PersonAdditionalInfos.AddAsync(ai);
            await _context.SaveChangesAsync();
            person.PersonAdditionalInfoId = aic.Entity.Id;
            await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("GetPerson")]
        public async Task<ActionResult<Person>> GetPerson(string id)
        {
            var people = await _context.People.FirstOrDefaultAsync(p => p.Id == id);
            return Ok(people);
        }

        [HttpGet("GetClaims")]
        public ActionResult GetClaims()
        {
            return Ok(HttpContext.User.Claims.Select(x => new { x.Type, x.Value }).ToList());
        }
    }
}