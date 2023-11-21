using System.ComponentModel.DataAnnotations;

namespace EmsApi.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
