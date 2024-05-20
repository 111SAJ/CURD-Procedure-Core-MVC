using System.ComponentModel.DataAnnotations;

namespace CRUDProcedureCoreMVC.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string EmployeeEmail { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }
        public string Address { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.Now;
    }
}
