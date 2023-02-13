using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.EF
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbSet<T> _dataSet;

        public GenericRepository(EFContext context)
        {
            _dataSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            _dataSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dataSet.Remove(entity);
        }

        public IAsyncEnumerable<T> GetAll()
        {
            return _dataSet.AsAsyncEnumerable();
        }

        public async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken)
        {
            return await _dataSet.FindAsync(id, cancellationToken);
        }

        public void Update(T entity)
        {
            _dataSet.Update(entity);
        }
    }
}
