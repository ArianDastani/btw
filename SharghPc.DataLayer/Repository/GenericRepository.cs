using Microsoft.EntityFrameworkCore;
using SharghPc.DataLayer.Context;
using SharghPc.DataLayer.Entites.Common;

namespace SharghPc.DataLayer.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly SharghPcDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(SharghPcDbContext context)
        {
            _context = context;
            this._dbSet = _context.Set<T>();
        }

        public IQueryable<T> GetQuery()
        {
            return _dbSet.AsQueryable();
        }

        public async Task AddEntity(T entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.LastUpdateDate = entity.CreateDate;
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeEntity(List<T> entities)
        {
            foreach (var entity in entities)
            {
                await AddEntity(entity);
            }
        }


        public async Task<T> GetEntityById(long entityId)
        {
            return await _dbSet.SingleOrDefaultAsync(s => s.Id == entityId);
        }

        public void EditEntity(T entity)
        {
            entity.LastUpdateDate = DateTime.Now;
            _dbSet.Update(entity);
        }

        public void DeleteEntity(T entity)
        {
            entity.IsDelete = true;
            EditEntity(entity);
        }

        public async Task DeleteEntity(long entityId)
        {
            T entity = await GetEntityById(entityId);
            if (entity != null) DeleteEntity(entity);
        }

        public void DeletePermanent(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeletePermanentEntities(List<T> entity)
        {
            _context.RemoveRange(entity);
        }

        public async Task DeletePermanent(long entityId)
        {
            T entity = await GetEntityById(entityId);
            if (entity != null) DeletePermanent(entity);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }
        }



    }

}
