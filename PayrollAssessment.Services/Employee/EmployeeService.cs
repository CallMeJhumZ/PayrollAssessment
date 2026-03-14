using PayrollAssessment.BusinessModels.Employee.Extensions.Employee;
using PayrollAssessment.BusinessModels.Employee.Request;
using PayrollAssessment.BusinessModels.Employee.Response;
using PayrollAssessment.DataModels.Employee;
using PayrollAssessment.Repositories.Employee.Interfaces;
using PayrollAssessment.Services.Employee.Interfaces;

namespace PayrollAssessment.Services.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;

        public EmployeeService(IEmployeeRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<EmployeeResponse>> GetAllAsync() =>
            (await _repo.GetAllAsync()).Select(employe => employe.MapToResponse()).ToList();

        public async Task<EmployeeResponse?> GetByEmployeeNumberAsync(string employeeNumber)
        {
            EmployeeInfo? employee = await _repo.GetByEmployeeInfoNumberAsync(employeeNumber);
            return employee?.MapToResponse();
        }

        public async Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request)
        {
            EmployeeInfo employee = new()
            {
                EmployeeNumber = request.GenerateEmployeeNumber(),
                LastName = request.LastName,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                DateOfBirth = request.DateOfBirth,
                DailyRate = request.DailyRate,
                WorkingDays = request.WorkingDays.ToUpper()
            };
            EmployeeInfo created = await _repo.CreateAsync(employee);
            return created.MapToResponse();
        }

        public async Task<EmployeeResponse?> UpdateAsync(string employeeNumber, UpdateEmployeeRequest request)
        {
            EmployeeInfo employee = new()
            {
                EmployeeNumber = employeeNumber,
                LastName = request.LastName,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                DateOfBirth = request.DateOfBirth,
                DailyRate = request.DailyRate,
                WorkingDays = request.WorkingDays.ToUpper()
            };
            EmployeeInfo? updated = await _repo.UpdateAsync(employee);
            return updated?.MapToResponse();
        }

        public async Task<bool> DeleteAsync(string employeeNumber) =>
            await _repo.DeleteAsync(employeeNumber);

        public async Task<TakeHomePayResponse?> ComputeTakeHomePayAsync(ComputePayRequest request)
        {
            EmployeeInfo? employee = await _repo.GetByEmployeeInfoNumberAsync(request.EmployeeNumber);
            return employee?.MapToTakeHomePayResponse(request);
        }
    }
}
