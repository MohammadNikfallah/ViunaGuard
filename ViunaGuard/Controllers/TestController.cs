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
    public class TestController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public TestController(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
            
        }

        [HttpGet("GetCar")]
        public async Task<ActionResult<List<Car>>> GetCar()
        {
            var cars = await context.Cars.ToListAsync();
            return Ok(cars);
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
    }
}