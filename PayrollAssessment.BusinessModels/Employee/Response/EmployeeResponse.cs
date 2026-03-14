using PayrollAssessment.DataModels.Employee;

namespace PayrollAssessment.BusinessModels.Employee.Response
{
    public class EmployeeResponse
    {
        private readonly int _id;
        private readonly string _employeeNumber;
        private readonly string _employeeName;
        private readonly DateTime _dateOfBirth;
        private readonly decimal _dailyRate;
        private readonly string _workingDays;

        public int Id => _id;
        public string EmployeeNumber => _employeeNumber;
        public string EmployeeName => _employeeName;
        public DateTime DateOfBirth => _dateOfBirth;
        public decimal DailyRate => _dailyRate;
        public string WorkingDays => _workingDays;

        public EmployeeResponse(EmployeeInfo employee)
        {
            _id = employee.Id;
            _employeeNumber = employee.EmployeeNumber;
            _employeeName = $"{employee.LastName}, {employee.FirstName}" +
                            (string.IsNullOrEmpty(employee.MiddleName) ? "" : $" {employee.MiddleName}");
            _dateOfBirth = employee.DateOfBirth;
            _dailyRate = employee.DailyRate;
            _workingDays = employee.WorkingDays;
        }
    }
}
