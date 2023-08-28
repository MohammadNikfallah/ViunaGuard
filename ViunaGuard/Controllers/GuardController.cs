using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        private readonly IMapper _mapper;

        public GuardController(IGuardService guardService, DataContext context, IMapper mapper)
        {
            _guardService = guardService;
            _context = context;
            _mapper = mapper;
        }

        // [HttpGet("GetEntrances")]
        // public async Task<ActionResult<List<EntranceGetDto>>> GetEntrances(DateOnly startDate, DateOnly endDate, int doorId
        //     , int personId, int organizationId, int carId, int guardId, int entranceTypeId, int enterOrExitId)
        // {
        //     var response = await  _guardService.GetEntrances
        //         (startDate, endDate, doorId, personId, carId, guardId, enterOrExitId);
        //
        //     if (response.HttpResponseCode == 200)
        //         return Ok(response.Data);
        //     else if (response.HttpResponseCode == 404)
        //         return NotFound(response.Message);
        //     else
        //         return BadRequest(response.Message);
        // }
        
        [HttpGet("GetEntrances")]
        public async Task<ActionResult<List<EntranceGroupGetDto>>> GetEntrances([Required] DateOnly startDate,[Required] DateOnly endDate
            , int doorId, int guardId, int enterOrExitId)
        {
            var response = await  _guardService.GetEntrances
                (startDate, endDate, doorId, guardId, enterOrExitId);
        
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }
        
        //
        // [HttpPost("PostEntrance")]
        // public async Task<ActionResult<Entrance>> PostEntrance(EntrancePostDto entrancePostDto)
        // {
        //     var response = await _guardService.PostEntrance(entrancePostDto);
        //     if (response.HttpResponseCode == 200)
        //         return Ok(response.Data);
        //     else if (response.HttpResponseCode == 404)
        //         return NotFound(response.Message);
        //     else
        //         return BadRequest(response.Message);
        // }
        
        [HttpGet("GetCar")]
        public async Task<ActionResult<Car>> GetCar(int carId)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
                return NotFound("Car not Found!");
            return Ok(car);
        }
        
        [HttpGet("GetDoors")]
        public async Task<ActionResult<List<DoorGetDto>>> GetDoors()
        {
            var guardId = HttpContext.User.FindFirst("EmployeeId");
            var guard = await _context.Employees.FindAsync(int.Parse(guardId!.Value));
            var doors = await _context.Doors.Where(d => d.OrganizationId == guard!.OrganizationId)
                .Select(d => _mapper.Map<DoorGetDto>(d)).ToListAsync();
            return Ok(doors);
        }
        
        [HttpPost("PostEntrances")]
        public async Task<ActionResult> PostSameGroupEntrances
            (EntranceGroupPostDto entranceGroupPost)
        {
            var response = await _guardService.PostEntrances(entranceGroupPost);
            if (response.HttpResponseCode == 200)
                return Ok();
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }
        
        [HttpGet("GetCars")]
        public async Task<ActionResult<List<Car>>> GetCars (string licenseNumber)
        {
            var response = new ServiceResponse<List<Car>>();
            response.Data = await _context.Cars.Where(car => car.LicenseNumber == licenseNumber).ToListAsync();
            return response.Data;
        }
    }
}
