using System.ComponentModel.DataAnnotations;

namespace EmsApi.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        [Range(1000000000, 9999999999,
            ErrorMessage = "Mobile no should be 10 digits")]
        public long MobileNo { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Range(1, double.MaxValue)]
        [DataType(DataType.Currency)]
        public double Salary { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
