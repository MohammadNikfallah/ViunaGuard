using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViunaGuard.Dtos;
using ViunaGuard.Models;

namespace ViunaGuard.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [Authorize("RoleCookie")]
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

        [HttpGet("GetPerson")]
        public async Task<ActionResult<Person>> GetPerson(int id)
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