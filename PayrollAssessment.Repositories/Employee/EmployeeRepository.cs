using Microsoft.EntityFrameworkCore;  // ← ADD THIS

using PayrollAssessment.DataModels.Employee;
using PayrollAssessment.Repositories.Employee.Interfaces;

namespace PayrollAssessment.Repositories.Employee
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly PayrollAssessmentDbContext _context;

        public EmployeeRepository(PayrollAssessmentDbContext context)
        {
            _context = context;
        }
        public async Task<List<EmployeeInfo>> GetAllAsync() =>
          await _context.Employees.ToListAsync();

        public async Task<EmployeeInfo?> GetByEmployeeInfoNumberAsync(string employeeNumber) =>
            await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeNumber == employeeNumber);

        public async Task<EmployeeInfo> CreateAsync(EmployeeInfo employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<EmployeeInfo?> UpdateAsync(EmployeeInfo employee)
        {
            EmployeeInfo? existing = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeNumber == employee.EmployeeNumber);
            if (existing == null) return null;

            existing.LastName = employee.LastName;
            existing.FirstName = employee.FirstName;
            existing.MiddleName = employee.MiddleName;
            existing.DateOfBirth = employee.DateOfBirth;
            existing.DailyRate = employee.DailyRate;
            existing.WorkingDays = employee.WorkingDays;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(string employeeNumber)
        {
            EmployeeInfo? employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeNumber == employeeNumber);
            if (employee == null) return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
