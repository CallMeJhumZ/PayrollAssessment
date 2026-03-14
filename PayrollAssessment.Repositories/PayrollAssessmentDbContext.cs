using Microsoft.EntityFrameworkCore;
using PayrollAssessment.DataModels.Employee;

namespace PayrollAssessment.Repositories
{
    public class PayrollAssessmentDbContext : DbContext
    {
        public PayrollAssessmentDbContext(DbContextOptions<PayrollAssessmentDbContext> options) : base(options) { }
        public DbSet<EmployeeInfo> Employees { get; set; }

    }
}
