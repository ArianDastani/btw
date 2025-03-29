using SharghPc.DataLayer.Entites.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharghPc.DataLayer.Repository2
{
    public interface IGenericRepository<T>:IAsyncDisposable where T : BaseEntity
    {
        IQueryable<T> GetQuery();
        Task AddEntity(T entity);
        Task AddRangeEntity(List<T>  entities);
        Task<T> GetEntityById(long entityId);
        void EditEntity(T entity);
        void DeleteEntity(T entity);
        Task DeleteEntity(long entityId);
        void DeletePermanent(T entity);
        void DeletePermanentEntities(List<T> entity);

        Task DeletePermanent(long entityId);
        Task SaveChanges();

    }
}
