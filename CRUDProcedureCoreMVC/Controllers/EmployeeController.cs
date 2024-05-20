using CRUDProcedureCoreMVC.Data;
using CRUDProcedureCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CRUDProcedureCoreMVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        //List
        public IActionResult Index()
        {
            var employeeList = _context.Employee.FromSqlRaw("EXEC EmployeeList").ToList();
            return View(employeeList);
        }

        //Create
        [HttpGet]
        public IActionResult Create()
        {
            var newEmployee = new Employee();
            return View(newEmployee);
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var existEmployee = _context.Employee.FirstOrDefault(e => e.EmployeeEmail == employee.EmployeeEmail);
                if (existEmployee != null)
                {
                    ModelState.AddModelError("EmployeeEmail", "User already registered");
                    return View(employee);
                }

                var parameters = new[]
                {
                    new SqlParameter("@EmployeeName", employee.EmployeeName),
                    new SqlParameter("@EmployeeEmail", employee.EmployeeEmail),
                    new SqlParameter("@Password", employee.Password),
                    new SqlParameter("@Address", employee.Address),
                    new SqlParameter("@LastUpdate", employee.LastUpdate)
                };

                _context.Database.ExecuteSqlRaw("EXEC CreateEmployee @EmployeeName, @EmployeeEmail, @Password, @Address, @LastUpdate", parameters);

                return RedirectToAction("Index");
            }
            return View(employee);
        }

        //Update
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _context.Employee.Find(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(int id, Employee employee)
        {
            if (ModelState.IsValid)
            {
                var parameters = new[]
                {
                    new SqlParameter("EmployeeId", employee.EmployeeId),
                    new SqlParameter("@EmployeeName", employee.EmployeeName),
                    new SqlParameter("@EmployeeEmail", employee.EmployeeEmail),
                    new SqlParameter("@Password", employee.Password),
                    new SqlParameter("@Address", employee.Address),
                    new SqlParameter("@LastUpdate", employee.LastUpdate)
                };

                _context.Database.ExecuteSqlRaw("EXEC UpdateEmployee @EmployeeId, @EmployeeName, @EmployeeEmail, @Password, @Address, @LastUpdate", parameters);

                return RedirectToAction("Index");
            }
            return View(employee);
        }

        //Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var employee = _context.Employee.Find(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@EmployeeId", id)
            };

            _context.Database.ExecuteSqlRaw("EXEC DeleteEmployee @EmployeeId", parameters);

            return RedirectToAction("Index");
        }
    }
}
