using Dem.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dem.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; } //Pk
        [Required(ErrorMessage = "Name Is Required")] //Frontend Validataion
        [MaxLength(50, ErrorMessage = "Max Length is 50 Chars ")]
        [MinLength(5, ErrorMessage = "Min Length is 5 Chars")]
        public string Name { get; set; }
        [Range(22, 35, ErrorMessage = "Age Must be In Range From 22 To 35")]
        public int? Age { get; set; }
        //123-Cairo-Cairo-Egypt
        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]+-[a-zA-Z]+-[a-zA-Z]+$", ErrorMessage = "Address must be in the format 123-Street-City-Country (e.g., 123-Cairo-Cairo-Egypt)")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhonwNumber { get; set; }
        public DateTime HireDate { get; set; }

        public IFormFile? Image { get; set; }
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
