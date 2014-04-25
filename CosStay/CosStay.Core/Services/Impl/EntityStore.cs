using CosStay.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Core.Services.Impl
{
    public class EntityStore:IEntityStore,IDisposable
    {
        private CosStayContext _db;
        private Request _request;
        public EntityStore(Request request)
        {
            _db = new CosStayContext();
            _request = request;
        }
        public IQueryable<TEntity> Query<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        
        public async Task<TEntity> GetAsync<TEntity>(object pk) where TEntity : class
        {
            var query = GetByKeyExpression<TEntity, object>(pk);
            return await query.SingleOrDefaultAsync();
        }

        public async Task<TEntity> GetAsync<TEntity,TProperty>(object pk, params Expression<Func<TEntity, TProperty>>[] paths) where TEntity : class
        {
            var query = GetByKeyExpression<TEntity, TProperty>(pk, paths);
            return await query.SingleOrDefaultAsync();
        }

        /*public TEntity Get<TEntity>(object pk) where TEntity : class
        {
            var query = GetByKeyExpression<TEntity, object>(pk);
            return query.SingleOrDefault();
        }*/

        /*
        public TEntity> GetAsync<TEntity, TProperty>(object pk, params Expression<Func<TEntity, TProperty>>[] paths) where TEntity : class
        {
            var query = GetByKeyExpression<TEntity, object>(pk);
            return query.SingleOrDefault();
        }*/

        protected IQueryable<TEntity> GetByKeyExpression<TEntity,TProperty>(object key, params Expression<Func<TEntity, TProperty>>[] paths) where TEntity : class
        {
            var checkType = typeof(TEntity);
            var set = _db.Set<TEntity>();
            IQueryable<TEntity> query = set;
            foreach (var path in paths)
            {
                var memberExpression = path.Body as MemberExpression;
                var propInfo = memberExpression.Member as PropertyInfo;
                if (!propInfo.ReflectedType.IsAssignableFrom(checkType))
                    throw new ArgumentException(string.Format(
                        "Expresion '{0}' refers to a property that is not from type {1}.",
                        path.ToString(),
                        checkType));

                query = query.Include(propInfo.Name);
            }
            var keyProperty = _db.KeyForEntity(checkType);
            var paramIn = Expression.Parameter(checkType, "p");

            var memberExp = Expression.Property(paramIn, keyProperty);
            var body = Expression.Equal(
                memberExp, Expression.Constant(key));

            var expression = Expression.Lambda<Func<TEntity, bool>>(body, paramIn);

            query = query.Where(expression);

            return query;
        }

        public virtual TEntity GetByKey<TEntity>(params object[] keys)
        {
            
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            var mdw = objectContext.MetadataWorkspace;
            var items = mdw.GetItems<EntityType>(DataSpace.CSpace);
            var ourType = typeof(TEntity);
            var entity = items.First(e => e.BaseType.NamespaceName == ourType.Namespace);
            
            var entitySetName = objectContext.DefaultContainerName + "." + entity.Name;
            var keyNames = entity.KeyMembers.Select(k => k.Name).ToArray();

            if (keys.Length != keyNames.Length)
            {
                throw new ArgumentException("Invalid number of key members");
            }
            
            // Merge key names and values by its order in array
            var keyPairs = keyNames.Zip(keys, (keyName, keyValue) => 
                new KeyValuePair<string, object>(keyName, keyValue));

            // Build entity key
            var entityKey = new EntityKey(entitySetName, keyPairs);
            // Query first current state manager and if entity is not found query database!!!
            return (TEntity)objectContext.GetObjectByKey(entityKey);
        }

        /*public virtual TEntity GetByKey<TEntity,TKey>(TKey key) where TEntity:class
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;

            // Build entity key
            var entityKey = new EntityKey(_entitySetName, _keyName, key);
            // Query first current state manager and if entity is not found query database!!!
            return (TEntity)objectContext.GetObjectByKey(entityKey);
        }

        }*/

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

        public async Task SaveAsync()
        {
            try
            {
                await _db.SaveChangesAsync(_request);
                return;
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .Select(x => new {
                            Entity = x.Entry.Entity.GetType().Name,
                            Message = string.Join(", ",x.ValidationErrors.Select(e => e.ErrorMessage))
                        }
                );

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages.Select(e => e.Entity + ": " + e.Message));

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
