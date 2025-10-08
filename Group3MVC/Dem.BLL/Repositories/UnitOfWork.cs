using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dem.BLL.Interfaces;
using Dem.DAL.Context;

namespace Dem.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly MVCProjectDbContext _dbContext;

        public IEmployeeRepository EmployeeRepository { get ; set; } //Automatic Property
        public IDepartmentRepository DepartmentRepository { get; set; } //Automatic Property

        public UnitOfWork(MVCProjectDbContext dbContext) //ASk CLR For Object From DbContext
        {
            EmployeeRepository = new EmployeeRepository(dbContext);
            DepartmentRepository =new DepartmentRepository(dbContext);
            _dbContext = dbContext;
        }

        public async Task<int> CompleteAsync()
        {
          return await _dbContext.SaveChangesAsync();
        }
        public void Dispose() 
        {
            _dbContext.Dispose();
        }

    }
}
