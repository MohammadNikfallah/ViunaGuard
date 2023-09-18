namespace ViunaGuard.Services
{
    public interface IUserService
    {
        public Task<ServiceResponse<Person>> PostCar(Car car, string id);
        public Task<ServiceResponse<List<SignatureNeedGetDto>>> GetOrganizationSignatureNeed (int organizationId);
        public Task<ServiceResponse> PostEntrancePermission(EntrancePermissionPostDto entrancePermissionPostDto);
        public Task<ServiceResponse<PersonGetDto>> GetPersonDetails();
        public Task<ServiceResponse<List<EntrancePermissionGetDto>>> GetEntrancePermissions();
        public Task<ServiceResponse<List<EmployeeGetDto>>> GetPersonJobs();
    }
}
