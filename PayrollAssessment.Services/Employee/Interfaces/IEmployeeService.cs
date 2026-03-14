using PayrollAssessment.BusinessModels.Employee.Request;
using PayrollAssessment.BusinessModels.Employee.Response;

namespace PayrollAssessment.Services.Employee.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeResponse>> GetAllAsync();
        Task<EmployeeResponse?> GetByEmployeeNumberAsync(string employeeNumber);
        Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request);
        Task<EmployeeResponse?> UpdateAsync(string employeeNumber, UpdateEmployeeRequest request);
        Task<bool> DeleteAsync(string employeeNumber);
        Task<TakeHomePayResponse?> ComputeTakeHomePayAsync(ComputePayRequest request);
    }
}
