using CosStay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Core.Services
{
    public interface IEntityStore:IDisposable
    {
        IQueryable<TEntity> Query<TEntity>() where TEntity : class;
        Task<TEntity> GetAsync<TEntity>(object pk) where TEntity : class;
        Task<TEntity> GetAsync<TEntity, TProperty>(object pk, params Expression<Func<TEntity, TProperty>>[] paths) where TEntity : class;
        
        IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;
        void Add<TEntity>(TEntity entity) where TEntity : class, IAddable;
        Task SaveAsync();

        void Update<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[] paths) where TEntity : class;
        void AddOrUpdate<TEntity>(TEntity entity) where TEntity : class, IEntity;

        void AttachStub<TEntity>(TEntity entity) where TEntity : class;
        void Delete<TEntity>(TEntity entity) where TEntity : class, IDeletable;
        void Destroy<TEntity>(TEntity entity) where TEntity : class, IDestoyable;


    }
}
