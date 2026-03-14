namespace PayrollAssessment.BusinessModels.Employee.Request
{
    public class ComputePayRequest
    {
        public string EmployeeNumber { get; set; } = string.Empty;
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
    }
}
