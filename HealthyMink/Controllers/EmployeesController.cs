using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthyMink.Class;
using HealthyMink.Models;
using HealthyMink.Class.Enum;

namespace HealthyMink.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public EmployeesController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetEmployees()
        {
            List<Employee> employees = _context.Employees.ToList();
            //Дата начала месяца
            DateTime startMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1).AddDays(-1);

            for (int i = 0; i < employees.Count; i++)
            {
                employees[i].Shift = new List<Shift>();
                employees[i].Shift = _context.Shifts.Where(c => c.Employee.Id == employees[i].Id).Select(x => new Shift
                {
                    Id = x.Id,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    Hour = x.Hour
                }).ToList();
                employees[i].Penalty = _context.Penalties.Where(c => c.Employee.Id == employees[i].Id && c.Time > startMonth && c.Time < endMonth).Count();
            }
            return employees;
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id, int jobTitle = -1)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return BadRequest();
            }

            //Опциональный аргумент – должность, если он передан, возвращаются только сотрудники с указанной должностью
            //Если передается несуществующая должность – вернуть ошибку
            if (jobTitle != -1) 
            {
                employee = await _context.Employees.FirstOrDefaultAsync(c => c.Id == id && (int)c.JobTitle == jobTitle);
                if (employee == null)
                {
                    return BadRequest();
                }
            }

            return employee;
        }

        // GET api/Employees/jobtitle
        [HttpGet("jobtitle")]
        public List<string> Get()
        {
            return Enum.GetNames(typeof(JobTitle)).ToList();
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return BadRequest();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
