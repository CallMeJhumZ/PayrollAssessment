using PayrollAssessment.BusinessModels.Employee.Request;
using PayrollAssessment.BusinessModels.Employee.Response;
using PayrollAssessment.DataModels.Employee;

namespace PayrollAssessment.BusinessModels.Employee.Extensions.Employee
{
    public static class EmployeeExtensions
    {
        public static EmployeeResponse MapToResponse(this EmployeeInfo employee) => new(employee);

        public static string GenerateEmployeeNumber(this CreateEmployeeRequest request)
        {
            string prefix = request.LastName.Length >= 3
                ? request.LastName.Substring(0, 3).ToUpper()
                : request.LastName.ToUpper().PadRight(3, '*');
            string random = new Random().Next(0, 99999).ToString("D5");
            string dobPart = request.DateOfBirth.ToString("ddMMMyyyy").ToUpper();
            return $"{prefix}-{random}-{dobPart}";
        }
        public static DayOfWeek[] GetScheduledDays(this EmployeeInfo e) =>
            e.WorkingDays.ToUpper() == "MWF"
        ? new[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday }
        : new[] { DayOfWeek.Tuesday, DayOfWeek.Thursday, DayOfWeek.Saturday };

        public static decimal ComputePay(this EmployeeInfo e, DateTime startingDate, DateTime endingDate)
        {
            DayOfWeek[] scheduledDays = e.GetScheduledDays();
            decimal totalPay = 0;

            for (DateTime current = startingDate; current <= endingDate; current = current.AddDays(1))
            {
                if (scheduledDays.Contains(current.DayOfWeek)) totalPay += e.DailyRate * 2;
                if (current.Month == e.DateOfBirth.Month && current.Day == e.DateOfBirth.Day) totalPay += e.DailyRate;
            }

            return totalPay;
        }

        public static TakeHomePayResponse MapToTakeHomePayResponse(this EmployeeInfo e, ComputePayRequest request) =>
            new(e, request.StartingDate, request.EndingDate);
    }
}
