using CosStay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Core.Services.Impl
{
    public class AuthorizationService : IAuthorizationService
    {
        protected IEntityStore _es;
        protected IUserService _userService;
        public AuthorizationService(IEntityStore entityStore, IUserService userService)
        {
            _es = entityStore;
            _userService = userService;
        }

        public bool IsAuthorizedTo<TEntity>(ActionType actionType, TEntity entity) where TEntity : class
        {
            return IsAuthorizedTo(_userService.CurrentUser, actionType, entity);
        }

        public bool IsAuthorizedTo<TEntity>(User user, ActionType actionType, TEntity entity) where TEntity : class
        {
            if (user != null && user.IsDeleted)
                return false;
            
            // TODO: Custom auth tables for entities
            //var type = entity.GetType();
            Type type;
            if (entity == null)
                type = typeof(TEntity);
            else
                type = entity.GetType();
            
            if (type == typeof(IEntity))
                throw new InvalidOperationException("IsAuthorizedTo called but call time type forced to be IEntity which prevents runtime type discovery of 'null'");

            if (typeof(IOwnable).IsAssignableFrom(type))
            {
                if (entity != null)
                {
                    var ownedEntity = (IOwnable)entity;
                    // If the entity can be owned, return true if user is owner
                    if (ownedEntity.OwnerId != null && user != null && ownedEntity.OwnerId == user.Id)
                        return true;
                }
                // if trying to create something that can be owned, we're good
                if (actionType == ActionType.Create && user != null)
                    return true;
            }

            // Default for all entities is Read
            return actionType == ActionType.Read || IsAdmin(user);
        }

        public bool IsAdmin(User user)
        {
            return (from ur in user.Roles
                         join r in _es.GetAll<IdentityRole>()
                         on ur.RoleId equals r.Id
                         where r.Name == "Admin"
                              select ur.UserId).Any();
                         
        }
    }
}
