using Dem.BLL.Interfaces;
using Dem.DAL.Context;
using Dem.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Dem.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MVCProjectDbContext _context;

        public GenericRepository(MVCProjectDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T entity)
        {
            await _context.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        //public IEnumerable<T> GetAll()
        //=> _context.Set<T>().ToList();

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>)await _context.Employees.Include(e => e.Department).ToListAsync();
            }
            return await _context.Set<T>().ToListAsync();
        }


        public async Task<T> GetByIdAsync(int id)
        => await _context.Set<T>().FindAsync(id);


        public void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}
