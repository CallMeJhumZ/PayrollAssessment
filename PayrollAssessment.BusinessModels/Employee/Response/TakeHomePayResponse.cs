using PayrollAssessment.DataModels.Employee;

namespace PayrollAssessment.BusinessModels.Employee.Response
{
    public class TakeHomePayResponse
    {
        private readonly string _employeeNumber;
        private readonly string _employeeName;
        private readonly DateTime _startingDate;
        private readonly DateTime _endingDate;
        private readonly decimal _takeHomePay;

        public string EmployeeNumber => _employeeNumber;
        public string EmployeeName => _employeeName;
        public DateTime StartingDate => _startingDate;
        public DateTime EndingDate => _endingDate;
        public decimal TakeHomePay => _takeHomePay;

        public TakeHomePayResponse(EmployeeInfo employee, DateTime startingDate, DateTime endingDate)
        {
            _employeeNumber = employee.EmployeeNumber;
            _employeeName = $"{employee.LastName}, {employee.FirstName}";
            _startingDate = startingDate;
            _endingDate = endingDate;
            _takeHomePay = Compute(employee, startingDate, endingDate);
        }

        private static DayOfWeek[] GetScheduledDays(EmployeeInfo employee) =>
            employee.WorkingDays.ToUpper() == "MWF"
                ? new[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday }
                : new[] { DayOfWeek.Tuesday, DayOfWeek.Thursday, DayOfWeek.Saturday };

        private static decimal Compute(EmployeeInfo employee, DateTime startingDate, DateTime endingDate)
        {
            DayOfWeek[] scheduledDays = GetScheduledDays(employee);
            decimal totalPay = 0;

            for (DateTime current = startingDate; current <= endingDate; current = current.AddDays(1))
            {
                if (scheduledDays.Contains(current.DayOfWeek)) totalPay += employee.DailyRate * 2;
                if (current.Month == employee.DateOfBirth.Month &&
                    current.Day == employee.DateOfBirth.Day) totalPay += employee.DailyRate;
            }

            return totalPay;
        }
    }
}
