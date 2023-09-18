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

        [HttpGet("GetEntrances")]
        public async Task<ActionResult<List<EntranceGroupGetDto>>> GetEntrances([Required] DateOnly startDate,[Required] DateOnly endDate
            , int doorId, int guardId, int enterOrExitId, int employeeId)
        {
            var response = await  _guardService.GetEntrances
                (startDate, endDate, doorId, guardId, enterOrExitId, employeeId);
        
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            return BadRequest(response.Message);
        }

        [HttpGet("GetCar")]
        public async Task<ActionResult<Car>> GetCar(int carId, int employeeId)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
                return NotFound("Car not Found!");
            return Ok(car);
        }
        
        [HttpGet("GetDoors")]
        public async Task<ActionResult<List<DoorGetDto>>> GetDoors(int employeeId)
        {
            var guardId = HttpContext.User.FindFirst("EmployeeId");
            var guard = await _context.Employees.FindAsync(int.Parse(guardId!.Value));
            var doors = await _context.Doors.Where(d => d.OrganizationId == guard!.OrganizationId)
                .Select(d => _mapper.Map<DoorGetDto>(d)).ToListAsync();
            return Ok(doors);
        }
        
        [HttpPost("PostEntrances")]
        public async Task<ActionResult> PostSameGroupEntrances
            (EntranceGroupPostDto entranceGroupPost, int employeeId)
        {
            var response = await _guardService.PostEntrances(entranceGroupPost, employeeId);
            if (response.HttpResponseCode == 200)
                return Ok();
            if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            return BadRequest(response.Message);
        }
        
        // [HttpGet("GetCar")]
        // public async Task<ActionResult<Car>> GetCar (string licenseNumber, int employeeId)
        // {
        //     var response = await _guardService.GetCar(entranceGroupPost, employeeId);
        //     if (response.HttpResponseCode == 200)
        //         return Ok();
        //     else if (response.HttpResponseCode == 404)
        //         return NotFound(response.Message);
        //     else
        //         return BadRequest(response.Message);
        // }
        
        [HttpGet("CheckEntrancePermission")]
        public async Task<ActionResult<EntrancePermissionCheckDto>> CheckEntrancePermission(string nationalId, int employeeId)
        {
            var response = await _guardService.CheckEntrancePermission(nationalId, employeeId);
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            return BadRequest(response.Message);
        }
        
        [HttpGet("CheckExitPermission")]
        public async Task<ActionResult<EntrancePermissionCheckDto>> CheckExitPermission(string nationalId, int employeeId)
        {
            var response = await _guardService.CheckExitPermission(nationalId, employeeId);
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            return BadRequest(response.Message);
        }
        
        [HttpGet("PostPerson")]
        public async Task<ActionResult> PostPerson(PersonPostDto personPostDto, int employeeId)
        {
            var response = await _guardService.PostPerson(personPostDto, employeeId);
            if (response.HttpResponseCode == 200)
                return Ok(response);
            if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            return BadRequest(response.Message);
        }

    }
}
