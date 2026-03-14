using PayrollAssessment.DataModels.Employee;

namespace PayrollAssessment.Repositories.Employee.Interfaces
{
    public interface IEmployeeRepository  
    {
        Task<List<EmployeeInfo>> GetAllAsync();
        Task<EmployeeInfo?> GetByEmployeeInfoNumberAsync(string employeeNumber);
        Task<EmployeeInfo> CreateAsync(EmployeeInfo employee);
        Task<EmployeeInfo?> UpdateAsync(EmployeeInfo employee);
        Task<bool> DeleteAsync(string employeeNumber);
    }
}
