using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dem.BLL.Interfaces;
using Dem.DAL.Context;
using Dem.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Dem.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department> ,IDepartmentRepository
    {
        //private readonly MVCProjectDbContext _context;

        public DepartmentRepository(MVCProjectDbContext context):base(context)
        {
            //_context = context;
        }

        /*
           private readonly MVCProjectDbContext _context ;
        public DepartmentRepository(MVCProjectDbContext context) //Ask CLR to inject the dependency of DbContext
        {
            _context = context;
        }
        public int Add(Department department)
        {
           _context.Departments.Add(department);
            return _context.SaveChanges();
        }

        public int Delete(Department department)
        {
            _context.Departments.Remove(department);
            return _context.SaveChanges();
        }

        public IEnumerable<Department> GetAll()
        => _context.Departments.ToList();
        

        public Department GetById(int id)
        {
            return _context.Departments.Find(id); //Find method works only with primary key Locally
            ///var department = _context.Departments.Local.Where(d => d.Id == id).FirstOrDefault();
            ///if (department is null)
            ///    department = _context.Departments.Where(d=> d.Id == id).FirstOrDefault();
            ///return department;
        }

        public int Update(Department department)
        {
            _context.Departments.Update(department);
            return _context.SaveChanges();
        }
         */
    }
}
