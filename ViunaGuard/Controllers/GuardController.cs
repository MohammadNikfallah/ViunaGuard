using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViunaGuard.Dtos;
using ViunaGuard.Models;
using ViunaGuard.Services;

namespace ViunaGuard.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [Authorize(Roles = "Guard", Policy = "RoleCookie")]
    public class GuardController : ControllerBase
    {
        private readonly IGuardService _guardService;
        private readonly DataContext _context;

        public GuardController(IGuardService guardService, DataContext context)
        {
            _guardService = guardService;
            _context = context;
        }

        [HttpGet("GetEntrances")]
        public async Task<ActionResult<List<EntranceGetDto>>> GetEntrances(DateOnly startDate, DateOnly endDate, int doorId
            , int personId, int organizationId, int carId, int guardId, int entranceTypeId, int enterOrExitId)
        {
            var response = await  _guardService.GetEntrances
                (startDate, endDate, doorId, personId, carId, guardId, enterOrExitId);

            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpPost("PostEntrance")]
        public async Task<ActionResult<Entrance>> PostEntrance(EntrancePostDto entrancePostDto)
        {
            var response = await _guardService.PostEntrance(entrancePostDto);
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }
        
        [HttpGet("GetCar")]
        public async Task<ActionResult<Car>> GetCar(int carId)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
                return NotFound("Car not Found!");
            return Ok(car);
        }
        
        [HttpPost("PostEntrances")]
        public async Task<ActionResult> PostSameGroupEntrances
            (EntranceGroupPostDto entranceGroupPost)
        {
            var response = await _guardService.PostSameGroupEntrances(entranceGroupPost);
            if (response.HttpResponseCode == 200)
                return Ok();
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }
    }
}
