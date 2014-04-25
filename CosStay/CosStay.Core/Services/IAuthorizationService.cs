using CosStay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Core.Services
{
    public interface IAuthorizationService
    {
        bool IsAuthorizedTo<TEntity>(User user, ActionType actionType, TEntity entity) where TEntity : IEntity;
        bool IsAuthorizedTo<TEntity>(ActionType actionType, TEntity entity) where TEntity : IEntity;
    }

    public enum ActionType
    {
        Create,
        Read,
        Update,
        Delete
    }
}
