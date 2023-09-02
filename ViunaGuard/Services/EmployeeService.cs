using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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

    public async Task<ServiceResponse> PostPeriodicShift(PeriodicShiftPostDto shift, int employeeId)
    {
        var response = new ServiceResponse();

        var employeeClaim = _httpContextAccessor.HttpContext!.User.FindFirstValue("EmployeeId");
        if (employeeClaim == null)
        {
            response.HttpResponseCode = 400;
            response.Message = "Something is wrong with EmployeeId Claim";
            return response;
        }
        var employee = await _context.Employees.FindAsync(employeeId);
        
        var employeePeriodicShift = _mapper.Map<EmployeePeriodicShift>(shift);
        employeePeriodicShift.OrganizationId = employee.OrganizationId;
        employeePeriodicShift.ShiftMakerEmployeeId = employeeId;
        
        await _context.EmployeePeriodicShifts.AddAsync(employeePeriodicShift);
        await _context.SaveChangesAsync();
            
        response.HttpResponseCode=200;
        return response;
    }

    public async Task<ServiceResponse<List<EmployeeShift>>> PostShift(ShiftPostDto shift, int employeeId)
    {
        var response = new ServiceResponse<List<EmployeeShift>>();

        var employee = await _context.Employees.FindAsync(employeeId);

        var shiftEmployee = await _context.Employees.FindAsync(shift.EmployeeId);
        if (shiftEmployee == null)
        {
            response.HttpResponseCode = 404;
            response.Message = "Employee Not Found";
            return response;
        }

        if (shiftEmployee.OrganizationId != employee.OrganizationId)
        {
            response.HttpResponseCode = 400;
            response.Message = "You cant add shift for this employee";
            return response;
        }


        var employeeShift = _mapper.Map<EmployeeShift>(shift);
        employeeShift.ShiftMakerEmployeeId = employeeId;
        employeeShift.OrganizationId = employee.OrganizationId;
        
        await _context.EmployeeShifts.AddAsync(employeeShift);
        await _context.SaveChangesAsync();
        var shifts = await _context.EmployeeShifts.ToListAsync();

        response.Data = shifts;
        response.HttpResponseCode=200;
        return response;
    }

    public async Task<ServiceResponse<TwoShiftGetDto>> GetEmployeeShifts(int employeeId)
    {
        var response = new ServiceResponse<TwoShiftGetDto>();

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
        var pc = new PersianCalendar();
            
        var MonthlyShift = _context.EmployeeShiftsPeriodicMonthly
            .Include(e => e.GuardDoor)
            .Where(e => e.EmployeeId == employeeId);
        var todayMonthlyShifts = MonthlyShift
            .Where(e => e.DayOfMonth == pc.GetDayOfMonth(time));
            
        var tempResult = await todayMonthlyShifts.Select(e => _mapper.Map<EmployeeShiftGetDto>(e)).ToListAsync();
        for (int i = 0; i < tempResult.Count; i++)
        {
            tempResult[i].StartTime = tempResult[i].StartTime.AddDays(time.DayOfYear - tempResult[i].StartTime.DayOfYear);
            tempResult[i].StartTime = tempResult[i].StartTime.AddYears(time.Year - tempResult[i].StartTime.Year);
            tempResult[i].FinishTime = tempResult[i].FinishTime.AddDays(time.DayOfYear - tempResult[i].FinishTime.DayOfYear);
            tempResult[i].FinishTime = tempResult[i].FinishTime.AddYears(time.Year - tempResult[i].FinishTime.Year);
        }
        Result.AddRange(tempResult);
        
        var periodicShift = _context.EmployeePeriodicShifts
            .Include(e => e.GuardDoor)
            .Where(e => e.EmployeeId == employeeId);
        var todayPeriodicShifts = periodicShift
            .Where(e => ((time.Date.DayOfYear - e.StartTime.Date.DayOfYear) % e.PeriodDayRange) == 0);
            
        tempResult = await todayPeriodicShifts.Select(e => _mapper.Map<EmployeeShiftGetDto>(e)).ToListAsync();
        for (int i = 0; i < tempResult.Count; i++)
        {
            tempResult[i].StartTime = tempResult[i].StartTime.AddDays(time.DayOfYear - tempResult[i].StartTime.DayOfYear);
            tempResult[i].StartTime = tempResult[i].StartTime.AddYears(time.Year - tempResult[i].StartTime.Year);
            tempResult[i].FinishTime = tempResult[i].FinishTime.AddDays(time.DayOfYear - tempResult[i].FinishTime.DayOfYear);
            tempResult[i].FinishTime = tempResult[i].FinishTime.AddYears(time.Year - tempResult[i].FinishTime.Year);
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

    public async Task<ServiceResponse<EmployeeShiftGetDto>> GetCurrentShift(int employeeId)
    {
        var response = new ServiceResponse<EmployeeShiftGetDto>();

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
            var dayDifference = time.DayOfYear - nowPeriodicShift.StartTime.DayOfYear;
            var yearDifference = time.Year - nowPeriodicShift.StartTime.Year;
            nowPeriodicShift.StartTime = nowPeriodicShift.StartTime.AddDays(dayDifference);
            nowPeriodicShift.StartTime = nowPeriodicShift.StartTime.AddYears(yearDifference);
            nowPeriodicShift.FinishTime = nowPeriodicShift.FinishTime.AddDays(dayDifference);
            nowPeriodicShift.FinishTime = nowPeriodicShift.FinishTime.AddYears(yearDifference);
            response.HttpResponseCode = 200;
            response.Data = _mapper.Map<EmployeeShiftGetDto>(nowPeriodicShift);
            return response;
        }

        var pc = new PersianCalendar();
            
        var MonthlyShift = _context.EmployeeShiftsPeriodicMonthly
            .Include(e => e.GuardDoor)
            .Where(e => e.EmployeeId == employeeId);
        var todayMonthlyShifts = MonthlyShift
            .Where(e => e.DayOfMonth == pc.GetDayOfMonth(time));
        var nowMonthlyShift = await todayMonthlyShifts
            .FirstOrDefaultAsync(p => time.TimeOfDay > p.StartTime.TimeOfDay && time.TimeOfDay < p.FinishTime.TimeOfDay);
        if (nowMonthlyShift != null)
        {
            var dayDifference = time.DayOfYear - nowMonthlyShift.StartTime.DayOfYear;
            var yearDifference = time.Year - nowMonthlyShift.StartTime.Year;
            nowMonthlyShift.StartTime = nowMonthlyShift.StartTime.AddDays(dayDifference);
            nowMonthlyShift.StartTime = nowMonthlyShift.StartTime.AddYears(yearDifference);
            nowMonthlyShift.FinishTime = nowMonthlyShift.FinishTime.AddDays(dayDifference);
            nowMonthlyShift.FinishTime = nowMonthlyShift.FinishTime.AddYears(yearDifference);
            response.HttpResponseCode = 200;
            response.Data = _mapper.Map<EmployeeShiftGetDto>(nowMonthlyShift);
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

    public async Task<ServiceResponse> PostEmployee(EmployeePostDto employeePostDto, int employeeId)
    {
        var employee = _mapper.Map<Employee>(employeePostDto);
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
        
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse> PostEmployeeWeeklyShift(WeeklyShiftPostDto weeklyShiftPostDto, int employeeId)
    {
        var response = new ServiceResponse();
        
        var employee = await _context.Employees.FindAsync(employeeId);

        var shiftEmployee = await _context.Employees.FindAsync(weeklyShiftPostDto.EmployeeId);
        if (shiftEmployee == null)
        {
            response.HttpResponseCode = 404;
            response.Message = "Employee Not Found";
            return response;
        }

        if (shiftEmployee.OrganizationId != employee.OrganizationId)
        {
            response.HttpResponseCode = 400;
            response.Message = "You cant add shift for this employee";
            return response;
        }


        var periodicShift = _mapper.Map<EmployeePeriodicShift>(weeklyShiftPostDto);
        periodicShift.PeriodDayRange = 7;
        periodicShift.OrganizationId = employee.OrganizationId;
        periodicShift.ShiftMakerEmployeeId = employeeId;
        
        periodicShift.StartTime = periodicShift.StartTime.AddYears(DateTime.Now.Year - periodicShift.StartTime.Year);
        periodicShift.StartTime = periodicShift.StartTime.AddMonths(DateTime.Now.Month - periodicShift.StartTime.Month);
        periodicShift.StartTime = periodicShift.StartTime.AddDays
            ((weeklyShiftPostDto.DayOfWeek - ((int) DateTime.Now.DayOfWeek + 1) % 7) + (DateTime.Now.DayOfYear - periodicShift.StartTime.DayOfYear));
        
        periodicShift.FinishTime = periodicShift.FinishTime.AddYears(DateTime.Now.Year - periodicShift.FinishTime.Year);
        periodicShift.FinishTime = periodicShift.FinishTime.AddMonths(DateTime.Now.Month - periodicShift.FinishTime.Month);
        periodicShift.FinishTime = periodicShift.FinishTime.AddDays
            ((weeklyShiftPostDto.DayOfWeek - ((int) DateTime.Now.DayOfWeek + 1) % 7) + (DateTime.Now.DayOfYear - periodicShift.FinishTime.DayOfYear));

        await _context.EmployeePeriodicShifts.AddAsync(periodicShift);
        await _context.SaveChangesAsync();

        response.HttpResponseCode = 200;
        return response;
    }

    public async Task<ServiceResponse> PostEmployeeMonthlyShift(MonthlyShiftPostDto monthlyShiftPostDto, int employeeId)
    {
        var response = new ServiceResponse();
        
        var employee = await _context.Employees.FindAsync(employeeId);

        var shiftEmployee = await _context.Employees.FindAsync(monthlyShiftPostDto.EmployeeId);
        if (shiftEmployee == null)
        {
            response.HttpResponseCode = 404;
            response.Message = "Employee Not Found";
            return response;
        }

        if (shiftEmployee.OrganizationId != employee.OrganizationId)
        {
            response.HttpResponseCode = 400;
            response.Message = "You cant add shift for this employee";
            return response;
        }

        var monthlyShift = _mapper.Map<EmployeeShiftPeriodicMonthly>(monthlyShiftPostDto);
        monthlyShift.ShiftMakerEmployeeId = employeeId;
        monthlyShift.OrganizationId = employee.OrganizationId;

        await _context.EmployeeShiftsPeriodicMonthly.AddAsync(monthlyShift);
        await _context.SaveChangesAsync();

        response.HttpResponseCode = 200;
        return response;
    }
}