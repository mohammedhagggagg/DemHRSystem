using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dem.DAL.Models
{
    public class Employee
    {
        public int Id { get; set; } //Pk
        [Required] //Frontend Validataion
        [MaxLength(50, ErrorMessage = "Max Length is 50 Chars ")]
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string PhonwNumber { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public string? ImageName { get; set; }

        //RelationShip

        public int? DepartmentId { get; set; } //FK
        //FK Optional => OnDelete : Restrict ====> If Department Has Employees You Can't Delete It
        //FK Required => OnDelete : Cascade ====> If Department Has Employees You Can Delete It And All Employees Will Be Deleted
        //If You Don't Specify OnDelete Behavior It Will Be Cascade By Default

        //Navigational Property  
        [InverseProperty("Employees")]
        public Department? Department { get; set; }

    }
}
