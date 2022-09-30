using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthyMink.Class;
using HealthyMink.Models;

namespace HealthyMink.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftsController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public ShiftsController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Shifts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shift>>> GetShifts()
        {
            return await _context.Shifts.ToListAsync();
        }

        // GET: api/Shifts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shift>> GetShift(int id)
        {
            var shift = await _context.Shifts.FindAsync(id);

            if (shift == null)
            {
                return BadRequest();
            }

            return shift;
        }

        // PUT: api/Shifts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShift(int id, Shift shift)
        {
            if (id != shift.Id)
            {
                return BadRequest();
            }

            try
            {
                _context.Entry(shift).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShiftExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }            
        }

        // POST: api/Shifts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Shift>> PostShift(Shift shift)
        {
            _context.Shifts.Add(shift);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShift", new { id = shift.Id }, shift);
        }

        // DELETE: api/Shifts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShift(int id)
        {
            var shift = await _context.Shifts.FindAsync(id);
            if (shift == null)
            {
                return BadRequest();
            }

            _context.Shifts.Remove(shift);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Shifts/StartShift/1
        [HttpPost("StartShift/{employeeId}")]
        public async Task<ActionResult<Shift>> StartShift(int employeeId)
        {
            //Поиск сотрудника с таким ID
            Employee? empl = await _context.Employees.FirstOrDefaultAsync(c => c.Id == employeeId);
            if (empl == null)
            {
                return BadRequest();
            }

            //Поиск смены с EndTime в значении DateTime.MinValue, это будет означать что:
            //3. Если сотрудник зашел на завод и вышел не отметившись, он не может пройти кпп
            //на вход пока не закроет предыдущую смену – возвращается ошибка.
            Shift? shft = await _context.Shifts.FirstOrDefaultAsync(c => c.EndTime == DateTime.MinValue && c.Employee.Id == employeeId);
            if (shft != null)
            {
                return BadRequest();
            }

            //Если текущее время больше 9 утра, проставляем нарушение
            if (DateTime.Now.Hour >= 9) 
            {
                _context.Penalties.Add(new Penalty(empl, DateTime.Now));
                await _context.SaveChangesAsync();
            }

            Shift shf = new Shift(DateTime.Now, DateTime.MinValue, 0, empl);
            _context.Shifts.Add(shf);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/Shifts/EndShift/1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("EndShift/{employeeId}")]
        public async Task<IActionResult> EndShift(int employeeId)
        {
            //Поиск сотрудника с таким ID
            Employee? empl = await _context.Employees.FirstOrDefaultAsync(c => c.Id == employeeId);
            if (empl == null)
            {
                return BadRequest();
            }

            //Ищем запись о входе сотрудника(ID) в которой дата выхода DateTime.MinValue, если такой нет:
            //4. Если сотрудник каким-то образом попал на завод не пробив пропуск, он не может
            //выйти пока не отметит начало своей смены – возвращается ошибка.
            Shift? shft = await _context.Shifts.FirstOrDefaultAsync(c => c.EndTime == DateTime.MinValue && c.Employee.Id == empl.Id);
            if (shft == null)
            {
                return BadRequest();
            }

            //Если текущее время меньше 9 вечера и вы Тестер, проставляем нарушение
            if (DateTime.Now.Hour < 21 && empl.JobTitle == Class.Enum.JobTitle.Tester) 
            {
                _context.Penalties.Add(new Penalty(empl, DateTime.Now));
                await _context.SaveChangesAsync();
            }

            //Если текущее время меньше 6 вечера и вы не Тестер, проставляем нарушение
            if (DateTime.Now.Hour < 18 && empl.JobTitle != Class.Enum.JobTitle.Tester)
            {
                _context.Penalties.Add(new Penalty(empl, DateTime.Now));
                await _context.SaveChangesAsync();
            }

            shft.EndTime = DateTime.Now;
            shft.Hour = (int)(shft.EndTime - shft.StartTime).TotalHours;
            _context.Shifts.Update(shft);
            await _context.SaveChangesAsync();

            return Ok();
        }        

        private bool ShiftExists(int id)
        {
            return _context.Shifts.Any(e => e.Id == id);
        }
    }
}
