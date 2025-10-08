using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dem.BLL.Interfaces
{
    public interface IUnitOfWork
    {
        //Signature For Property For Each and Every Repository Interface
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }

        Task<int> CompleteAsync(); //Signature For Complete Method
    }
}
