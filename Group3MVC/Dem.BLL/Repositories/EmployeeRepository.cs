using Dem.BLL.Interfaces;
using Dem.DAL.Context;
using Dem.DAL.Models;

namespace Dem.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MVCProjectDbContext _context;

        public EmployeeRepository(MVCProjectDbContext context) : base(context)
        {
            _context = context;
        }

        /*
           private readonly MVCProjectDbContext _context;

        public EmployeeRepository(MVCProjectDbContext context) //Ask CLR For Object From DbContext
        {
            _context = context;
        }
        public int Add(Employee employee)
        {
            _context.Employees.Add(employee);
            return _context.SaveChanges();
        }

        public int Delete(Employee employee)
        {
            _context.Employees.Remove(employee);
            return _context.SaveChanges();
        }

        public IEnumerable<Employee> GetAll()
        => _context.Employees.ToList();

        public Employee GetById(int id)
        => _context.Employees.Find(id);


        public int Update(Employee employee)
        {
            _context.Employees.Update(employee);
            return _context.SaveChanges();
        }
         */
        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
            //var employees = _context.Employees.Where(e => e.Address.Contains(address));
            var employees = _context.Employees.Where(e => e.Address == address);
            return employees;
        }

        public IQueryable<Employee> GetEmployeesByName(string name)

           => _context.Employees.Where(e => e.Name.ToLower().Contains(name.ToLower()));

    }
}
