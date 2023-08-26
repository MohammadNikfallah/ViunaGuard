using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViunaGuard.Dtos;
using ViunaGuard.Models;

namespace ViunaGuard.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EmployeeService(IMapper mapper, DataContext context,IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<object>> PostPeriodicShift(PeriodicShiftPostDto shift)
    {
        var response = new ServiceResponse<object>();

        var employeeClaim = _httpContextAccessor.HttpContext.User.FindFirstValue("EmployeeId");
        if (employeeClaim == null)
        {
            response.HttpResponseCode = 400;
            response.Message = "Something is wrong with EmployeeId";
            return response;
        }
        var employeeId = int.Parse(employeeClaim);
        
        var employeePeriodicShift = _mapper.Map<EmployeePeriodicShift>(shift);
        employeePeriodicShift.ShiftMakerEmployeeId = employeeId;
        
        await _context.EmployeePeriodicShifts.AddAsync(employeePeriodicShift);
        await _context.SaveChangesAsync();
            
        response.HttpResponseCode=200;
        return response;
    }

    public async Task<ServiceResponse<List<EmployeeShift>>> PostShift(ShiftPostDto shift)
    {
        var response = new ServiceResponse<List<EmployeeShift>>();
        
        var employeeClaim = _httpContextAccessor.HttpContext.User.FindFirstValue("EmployeeId");
        if (employeeClaim == null)
        {
            response.HttpResponseCode = 400;
            response.Message = "Something is wrong with EmployeeId";
            return response;
        }
        var employeeId = int.Parse(employeeClaim);

        var employeeShift = _mapper.Map<EmployeeShift>(shift);
        employeeShift.ShiftMakerEmployeeId = employeeId;
        
        await _context.EmployeeShifts.AddAsync(employeeShift);
        await _context.SaveChangesAsync();
        var shifts = await _context.EmployeeShifts.ToListAsync();

        response.Data = shifts;
        response.HttpResponseCode=200;
        return response;
    }

    public async Task<ServiceResponse<TwoShiftGetDto>> GetEmployeeShifts()
    {
        var response = new ServiceResponse<TwoShiftGetDto>();
        
        var employeeClaim = _httpContextAccessor.HttpContext.User.FindFirstValue("EmployeeId");
        if (employeeClaim == null)
        {
            response.HttpResponseCode = 400;
            response.Message = "Something is wrong with EmployeeId";
            return response;
        }
        var employeeId = int.Parse(employeeClaim);
        
        response.Data = new TwoShiftGetDto();
        var time = DateTime.Now;
        var id = _httpContextAccessor.HttpContext!.User.FindFirstValue("ID");
        var person = await _context.People.FindAsync(int.Parse(id));
        if(person!.Jobs.Any(j => j.Id == employeeId))
        {
            response.HttpResponseCode = 400;
            response.Message = "Something wrong with EmployeeId";
            return response;
        }

        response.Data.TodayShifts.AddRange(await GetDayShifts(time, employeeId));
        response.Data.TomorrowShifts.AddRange(await GetDayShifts(time.AddDays(1), employeeId));
        response.HttpResponseCode = 200;
        return response;
    }
    
    public async Task<List<EmployeeShiftGetDto>> GetDayShifts(DateTime time, int employeeId)
    {
        var Result = new List<EmployeeShiftGetDto>();
        var periodicShift = _context.EmployeePeriodicShifts
            .Include(e => e.GuardDoor)
            .Where(e => e.EmployeeId == employeeId);
        var todayPeriodicShifts = periodicShift
            .Where(e => ((time.Date.DayOfYear - e.StartTime.Date.DayOfYear) % e.PeriodDayRange) == 0);
            
        var tempResult = await todayPeriodicShifts.Select(e => _mapper.Map<EmployeeShiftGetDto>(e)).ToListAsync();
        for (int i = 0; i < tempResult.Count; i++)
        {
            tempResult[i].StartTime = tempResult[i].StartTime.AddDays(time.DayOfYear - tempResult[i].StartTime.DayOfYear);
            tempResult[i].FinishTime = tempResult[i].FinishTime.AddDays(time.DayOfYear - tempResult[i].FinishTime.DayOfYear);
        }
        Result.AddRange(tempResult);

        var shift = _context.EmployeeShifts
            .Include(e => e.GuardDoor)
            .Where(e => e.EmployeeId == employeeId);
        var nowShift = shift.Where(e => time.Date <= e.FinishTime.Date && time.Date >= e.StartTime.Date);
        tempResult = await nowShift.Select(e => _mapper.Map<EmployeeShiftGetDto>(e)).ToListAsync();
        for (int i = 0; i < tempResult.Count; i++)
        {
            if (tempResult[i].StartTime.AddDays(1).Date <= time.Date)
                tempResult[i].StartTime = DateOnly.FromDateTime(time).ToDateTime(TimeOnly.MinValue);
            if (tempResult[i].FinishTime.AddDays(-1).Date >= time.Date)
                tempResult[i].FinishTime = DateOnly.FromDateTime(time).ToDateTime(TimeOnly.MaxValue);
        }
        Result.AddRange(tempResult);

        return Result;
    }

    public async Task<ServiceResponse<EmployeeShiftGetDto>> GetCurrentShift()
    {
        var response = new ServiceResponse<EmployeeShiftGetDto>();
        
        var employeeClaim = _httpContextAccessor.HttpContext.User.FindFirstValue("EmployeeId");
        if (employeeClaim == null)
        {
            response.HttpResponseCode = 400;
            response.Message = "Something is wrong with EmployeeId";
            return response;
        }
        var employeeId = int.Parse(employeeClaim);
        
        var time = DateTime.Now;
        var id = _httpContextAccessor.HttpContext!.User.FindFirstValue("ID");
        var person = await _context.People.FindAsync(int.Parse(id));
        if(person!.Jobs.Any(j => j.Id == employeeId))
        {
            response.HttpResponseCode = 400;
            response.Message = "Something wrong with EmployeeId";
            return response;
        }
            
        var periodicShift = _context.EmployeePeriodicShifts
            .Include(e => e.GuardDoor)
            .Where(e => e.EmployeeId == employeeId);
        var todayPeriodicShifts = periodicShift
            .Where(e => ((time.Date.DayOfYear - e.StartTime.Date.DayOfYear) % e.PeriodDayRange) == 0);
        var nowPeriodicShift = await todayPeriodicShifts
            .FirstOrDefaultAsync(p => time.TimeOfDay > p.StartTime.TimeOfDay && time.TimeOfDay < p.FinishTime.TimeOfDay);
        if (nowPeriodicShift != null)
        {
            response.HttpResponseCode = 200;
            response.Data = _mapper.Map<EmployeeShiftGetDto>(nowPeriodicShift);
            return response;
        }
            
        var shift = _context.EmployeeShifts
            .Include(e => e.GuardDoor)
            .Where(e => e.EmployeeId == employeeId);
        var nowShift = await shift
            .FirstOrDefaultAsync(e => time.Date < e.FinishTime.Date && time.Date > e.StartTime.Date);
        if (nowShift != null)
        {
            response.HttpResponseCode = 200;
            response.Data = _mapper.Map<EmployeeShiftGetDto>(nowShift);
            return response;
        }
            
        response.HttpResponseCode = 404;
        response.Message = "There is no current shift for this employee";
        return response;
    }

    public async Task<ActionResult> PostEmployee(EmployeePostDto employeePostDto)
    {
        var employee = _mapper.Map<Employee>(employeePostDto);
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
        
        throw new NotImplementedException();
    }
}