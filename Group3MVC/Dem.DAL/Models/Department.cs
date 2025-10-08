using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dem.DAL.Models
{
    public class Department
    {
        public Department()
        {
            Employees = new HashSet<Employee>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Name Is Required")]
        [MaxLength(50, ErrorMessage = "Name Max Length Is 50")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Code Is Required")]
        [MaxLength(10, ErrorMessage = "Code Max Length Is 10")]
        public string Code { get; set; }
        public DateTime DateOfCreation { get; set; }

        //RelationShip
        //Navigational Property
        [InverseProperty("Department")]
        public ICollection<Employee> Employees { get; set; }
    }
}
