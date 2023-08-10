using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ViunaGuard.Dtos;
using ViunaGuard.Models;

namespace ViunaGuard.Services
{
    public interface IUserService
    {
        public Task<ServiceResponse<Person>> PostCar(Car car, int id);

        public Task<ServiceResponse<List<Person>>> PostPerson(PersonPostDto personDto);

        public Task<ServiceResponse<List<EmployeeShiftPeriodicMonthly>>> PostShiftMonthly(EmployeeShiftPeriodicMonthly shift);
        public Task<ServiceResponse<List<SignatureNeedGetDto>>> GetOrganizationSignatureNeed (int organizationId);
        public Task<ServiceResponse<List<Authority>>> GetOrganizationAuthorities(int organizationId);
        public Task<ServiceResponse<PersonGetDto>> GetPersonDetails();
    }
}
