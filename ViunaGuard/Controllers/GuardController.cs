using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ViunaGuard.Dtos;
using ViunaGuard.Models;
using ViunaGuard.Models.Enums;
using ViunaGuard.Services;

namespace ViunaGuard.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpGet("GetEntrances")]
        public async Task<ActionResult<List<Entrance>>> GetEntrances(DateOnly startDate,DateOnly endDate
            , [Required] int employeeId, int doorId, int personId
            , int organizationId, int carId, int guardId, int entranceTypeId, int enterOrExitId)
        {
            var response = await  guardService.GetEntrances
                (startDate, endDate, employeeId, doorId, personId, organizationId, carId, guardId, entranceTypeId, enterOrExitId);

            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }

        [HttpPost("PostEntrance")]
        public async Task<ActionResult<Entrance>> PostEntrance(EntrancePostDto entrancePostDto,[Required] int employeeId)
        {
            var response = await guardService.PostEntrance(entrancePostDto, employeeId);
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
            var car = await context.Cars.FindAsync(carId);
            if (car == null)
                return NotFound("Car not Found!");
            return Ok(car);
        }
        
        [HttpGet("PostSameGroupEntrances")]
        public async Task<ActionResult> PostSameGroupEntrances
            (List<EntrancePostDto> entrancePostDtos, int driverID, [Required] int employeeId)
        {
            var response = await guardService.PostSameGroupEntrances(entrancePostDtos, driverID, employeeId);
            if (response.HttpResponseCode == 200)
                return Ok(response.Data);
            else if (response.HttpResponseCode == 404)
                return NotFound(response.Message);
            else
                return BadRequest(response.Message);
        }
    }
}
