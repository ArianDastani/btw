﻿using SharghPc.DataLayer.Entites.Common;

namespace SharghPc.DataLayer.Repository
{
    public interface IGenericRepository<T> : IAsyncDisposable where T : BaseEntity
    {
        IQueryable<T> GetQuery();
        Task AddEntity(T entity);
        Task AddRangeEntity(List<T> entities);
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
