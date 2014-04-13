using CosStay.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Core.Services.Impl
{
    public class EntityStore:IEntityStore,IDisposable
    {
        private CosStayContext _db;
        public EntityStore()
        {
            _db = new CosStayContext();
        }
        public IQueryable<TEntity> Query<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public TEntity Get<TEntity>(object pk) where TEntity : class
        {
            return _db.Set<TEntity>().Find(pk);
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            var set = _db.Set<TEntity>();
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletable)))
                return set.Where(e => !((IDeletable)e).IsDeleted);
            return set;
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class, IAddable
        {
            _db.Set(entity.GetType()).Add(entity);
        }

        public void Update<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[] paths) where TEntity : class
        {
            if (_db.Entry<TEntity>(entity).State == System.Data.Entity.EntityState.Unchanged)
                _db.Entry<TEntity>(entity).State = System.Data.Entity.EntityState.Modified;

            foreach (var path in paths)
            {
                var property = path.Compile().Invoke(entity);
                AddOrUpdate(property);
            }
        }

        public void AddOrUpdate<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            if (entity.Id == 0)
            {
                if (entity is IAddable)
                    Add((IAddable)entity);
            }
            else
            {
                Update(entity);
            }
        }

        public void AddOrUpdate<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity
        {
            foreach (var entity in entities)
                AddOrUpdate(entity);
        }

        protected void AddOrUpdate(object property)
        {
            if (property is IEnumerable)
                AddOrUpdate((IEnumerable<IEntity>) property);
            else
                AddOrUpdate((IEntity) property);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IDeletable
        {
            entity.IsDeleted = true;
            //_db.Set<TEntity>().Remove(entity);
        }

        public void Destroy<TEntity>(TEntity entity) where TEntity : class, IDestoyable
        {
            _db.Set<TEntity>().Remove(entity);
        }

        public void Save()
        {
            try
            {
                _db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void AttachStub<TEntity>(TEntity entity) where TEntity:class
        {
            _db.Set<TEntity>().Attach(entity);
        }
    }
}
