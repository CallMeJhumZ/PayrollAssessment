using Microsoft.AspNetCore.Mvc;
using PayrollAssessment.BusinessModels.Employee.Request;
using PayrollAssessment.Services.Employee.Interfaces;

namespace PayrollAssessment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{employeeNumber}")]
        public async Task<IActionResult> Get(string employeeNumber)
        {
            var result = await _service.GetByEmployeeNumberAsync(employeeNumber);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request)
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(Get),
                new { employeeNumber = result.EmployeeNumber }, result);
        }

        [HttpPut("update/{employeeNumber}")]
        public async Task<IActionResult> Update(string employeeNumber,
            [FromBody] UpdateEmployeeRequest request)
        {
            var result = await _service.UpdateAsync(employeeNumber, request);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("delete/{employeeNumber}")]
        public async Task<IActionResult> Delete(string employeeNumber)
        {
            var success = await _service.DeleteAsync(employeeNumber);
            return success ? NoContent() : NotFound();
        }

        [HttpPost("compute")]
        public async Task<IActionResult> Compute([FromBody] ComputePayRequest request)
        {
            var result = await _service.ComputeTakeHomePayAsync(request);
            return result == null ? NotFound() : Ok(result);
        }
    }
}
