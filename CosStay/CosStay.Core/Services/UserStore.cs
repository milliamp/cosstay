using CosStay.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Core.Services
{
    public class UserStore:IUserStore<User>,IUserLoginStore<User, string>, IUserClaimStore<User, string>, IUserRoleStore<User, string>, IUserPasswordStore<User, string>, IUserSecurityStampStore<User, string>, IQueryableUserStore<User, string>, IUserEmailStore<User, string>, IUserPhoneNumberStore<User, string>, IUserTwoFactorStore<User, string>, IUserLockoutStore<User, string>, IUserStore<User, string>
    {
        private IEntityStore _es;
        /// <summary>
        ///     Context for the store
        /// </summary>
        public CosStayContext Context
        {
            get;
            private set;
        }
        /// <summary>
        ///     If true will call dispose on the DbContext during Dipose
        /// </summary>
        public bool DisposeContext
        {
            get;
            set;
        }
        /// <summary>
        ///     If true will call SaveChanges after Create/Update/Delete
        /// </summary>
        public bool AutoSaveChanges
        {
            get;
            set;
        }
        /// <summary>
        ///     Returns an IQueryable of users
        /// </summary>
        public IQueryable<User> Users
        {
            get
            {
                return _es.GetAll<User>();
            }
        }
        /// <summary>
        ///     Constructor which takes a db context and wires up the stores with default instances using the context
        /// </summary>
        /// <param name="context"></param>
        public UserStore(IEntityStore es)
        {
            if (es == null)
            {
                throw new ArgumentNullException("es");
            }
            _es = es;
            this.AutoSaveChanges = true;
        }
        /// <summary>
        ///     Return the claims for a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual Task<IList<Claim>> GetClaimsAsync(User user)
        {

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            IList<Claim> result = (
                from c in user.Claims
                select new Claim(c.ClaimType, c.ClaimValue)).ToList<Claim>();
            return Task.FromResult<IList<Claim>>(result);
        }
        /// <summary>
        ///     Add a claim to a user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        public virtual Task AddClaimAsync(User user, Claim claim)
        {

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }
            ICollection<IdentityUserClaim> arg_7B_0 = user.Claims;
            IdentityUserClaim item = Activator.CreateInstance<IdentityUserClaim>();
            item.UserId = user.Id;
            item.ClaimType = claim.Type;
            item.ClaimValue = claim.Value;
            arg_7B_0.Add(item);
            return Task.FromResult<int>(0);
        }
        /// <summary>
        ///     Remove a claim from a user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        public virtual Task RemoveClaimAsync(User user, Claim claim)
        {

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }
            List<IdentityUserClaim> list = (
                from uc in user.Claims
                where uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type
                select uc).ToList<IdentityUserClaim>();
            foreach (IdentityUserClaim current in list)
            {
                user.Claims.Remove(current);
            }
            IQueryable<IdentityUserClaim> queryable =
                from uc in _es.GetAll<IdentityUserClaim>()
                where uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type
                select uc;
            foreach (IdentityUserClaim current2 in queryable)
            {
                _es.Destroy<IdentityUserClaim>(current2);
            }
            return Task.FromResult<int>(0);
        }
        /// <summary>
        ///     Returns whether the user email is confirmed
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetEmailConfirmedAsync(User user)
        {

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<bool>(user.EmailConfirmed);
        }
        /// <summary>
        ///     Set IsConfirmed on the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.EmailConfirmed = confirmed;
            return Task.FromResult<int>(0);
        }
        /// <summary>
        ///     Set the user email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task SetEmailAsync(User user, string email)
        {

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.Email = email;
            return Task.FromResult<int>(0);
        }
        /// <summary>
        ///     Get the user's email
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetEmailAsync(User user)
        {

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<string>(user.Email);
        }
        /// <summary>
        ///     Find a user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<User> FindByEmailAsync(string email)
        {

            return this.GetUserAggregateAsync((User u) => u.Email.ToUpper() == email.ToUpper());
        }
        /// <summary>
        ///     Returns the DateTimeOffset that represents the end of a user's lockout, any time in the past should be considered
        ///     not locked out.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<DateTimeOffset>(user.LockoutEndDateUtc.HasValue ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc)) : default(DateTimeOffset));
        }
        /// <summary>
        ///     Locks a user out until the specified end date (set to a past date, to unlock a user)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="lockoutEnd"></param>
        /// <returns></returns>
        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.LockoutEndDateUtc = ((lockoutEnd == DateTimeOffset.MinValue) ? null : new DateTime?(lockoutEnd.UtcDateTime));
            return Task.FromResult<int>(0);
        }
        /// <summary>
        ///     Used to record when an attempt to access the user has failed
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<int> IncrementAccessFailedCountAsync(User user)
        {

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.AccessFailedCount++;
            return Task.FromResult<int>(user.AccessFailedCount);
        }
        /// <summary>
        ///     Used to reset the account access count, typically after the account is successfully accessed
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task ResetAccessFailedCountAsync(User user)
        {

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.AccessFailedCount = 0;
            return Task.FromResult<int>(0);
        }
        /// <summary>
        ///     Returns the current number of failed access attempts.  This number usually will be reset whenever the password is
        ///     verified or the account is locked out.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<int> GetAccessFailedCountAsync(User user)
        {

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<int>(user.AccessFailedCount);
        }
        /// <summary>
        ///     Returns whether the user can be locked out.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<bool>(user.LockoutEnabled);
        }
        /// <summary>
        ///     Sets whether the user can be locked out.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.LockoutEnabled = enabled;
            return Task.FromResult<int>(0);
        }
        /// <summary>
        ///     Find a user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual Task<User> FindByIdAsync(string userId)
        {
            return this.GetUserAggregateAsync((User u) => u.Id.Equals(userId));
        }
        /// <summary>
        ///     Find a user by name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual Task<User> FindByNameAsync(string userName)
        {
            return this.GetUserAggregateAsync((User u) => u.UserName.ToUpper() == userName.ToUpper());
        }
        /// <summary>
        ///     Insert an entity
        /// </summary>
        /// <param name="user"></param>
        public virtual async Task CreateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            _es.Add(user);
            await this.SaveChanges().ConfigureAwait(false);
        }
        /// <summary>
        ///     Mark an entity for deletion
        /// </summary>
        /// <param name="user"></param>
        public virtual async Task DeleteAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            _es.Delete(user);
            await this.SaveChanges().ConfigureAwait(false);
        }
        /// <summary>
        ///     Update an entity
        /// </summary>
        /// <param name="user"></param>
        public virtual async Task UpdateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            _es.Update(user);
            await this.SaveChanges().ConfigureAwait(false);
        }
        /// <summary>
        ///     Returns the user associated with this login
        /// </summary>
        /// <returns></returns>
        public virtual async Task<User> FindAsync(UserLoginInfo login)
		{
			if (login == null)
			{
				throw new ArgumentNullException("login");
			}
            var userLogin = _es.GetAll<IdentityUserLogin>().SingleOrDefault(ul => ul.LoginProvider == login.LoginProvider && ul.ProviderKey == login.ProviderKey);
            if (userLogin == null)
                return null;
            
			return await _es.GetAll<User>().SingleOrDefaultAsync(u => u.Id == userLogin.UserId);
		}
        /// <summary>
        ///     Add a login to the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        public virtual Task AddLoginAsync(User user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            ICollection<IdentityUserLogin> arg_7B_0 = user.Logins;
            IdentityUserLogin item = Activator.CreateInstance<IdentityUserLogin>();
            item.UserId = user.Id;
            item.ProviderKey = login.ProviderKey;
            item.LoginProvider = login.LoginProvider;
            arg_7B_0.Add(item);
            return Task.FromResult<int>(0);
        }
        /// <summary>
        ///     Remove a login from a user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        public virtual Task RemoveLoginAsync(User user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            string provider = login.LoginProvider;
            string key = login.ProviderKey;
            IdentityUserLogin tUserLogin = user.Logins.SingleOrDefault((IdentityUserLogin l) => l.LoginProvider == provider && l.ProviderKey == key);
            if (tUserLogin != null)
            {
                user.Logins.Remove(tUserLogin);
            }
            return Task.FromResult<int>(0);
        }
        /// <summary>
        ///     Get the logins for a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            IList<UserLoginInfo> result = (
                from l in user.Logins
                select new UserLoginInfo(l.LoginProvider, l.ProviderKey)).ToList<UserLoginInfo>();
            return Task.FromResult<IList<UserLoginInfo>>(result);
        }
        /// <summary>
        ///     Set the password hash for a user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PasswordHash = passwordHash;
            return Task.FromResult<int>(0);
        }
        /// <summary>
        ///     Get the password hash for a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetPasswordHashAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<string>(user.PasswordHash);
        }
        /// <summary>
        ///     Returns true if the user has a password set
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult<bool>(user.PasswordHash != null);
        }
        /// <summary>
        ///     Set the user's phone number
        /// </summary>
        /// <param name="user"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public Task SetPhoneNumberAsync(User user, string phoneNumber)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PhoneNumber = phoneNumber;
            return Task.FromResult<int>(0);
        }
        /// <summary>
        ///     Get a user's phone number
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetPhoneNumberAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<string>(user.PhoneNumber);
        }
        /// <summary>
        ///     Returns whether the user phoneNumber is confirmed
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetPhoneNumberConfirmedAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<bool>(user.PhoneNumberConfirmed);
        }
        /// <summary>
        ///     Set PhoneNumberConfirmed on the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult<int>(0);
        }
        /// <summary>
        ///     Add a user to a role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public virtual Task AddToRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("ValueCannotBeNullOrEmpty", "roleName");
            }
            IdentityRole tRole = _es.GetAll<IdentityRole>().SingleOrDefault((r) => r.Name.ToUpper() == roleName.ToUpper());
            if (tRole == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "RoleNotFound", new object[]
				{
					roleName
				}));
            }
            IdentityUserRole tUserRole = Activator.CreateInstance<IdentityUserRole>();
            tUserRole.UserId = user.Id;
            tUserRole.RoleId = tRole.Id;
            IdentityUserRole item = tUserRole;
            user.Roles.Add(item);
            tRole.Users.Add(item);
            return Task.FromResult<int>(0);
        }
        /// <summary>
        ///     Remove a user from a role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public virtual Task RemoveFromRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("ValueCannotBeNullOrEmpty", "roleName");
            }
            IdentityRole roleEntity = _es.GetAll<IdentityRole>().SingleOrDefault((r) => r.Name.ToUpper() == roleName.ToUpper());
            if (roleEntity != null)
            {
                IdentityUserRole tUserRole = user.Roles.FirstOrDefault((IdentityUserRole r) => roleEntity.Name.ToUpper() == roleName.ToUpper());
                if (tUserRole != null)
                {
                    user.Roles.Remove(tUserRole);
                    roleEntity.Users.Remove(tUserRole);
                }
            }
            return Task.FromResult<int>(0);
        }
        /// <summary>
        ///     Get the names of the roles a user is a member of
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual Task<IList<string>> GetRolesAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            IEnumerable<string> source =
                from userRoles in user.Roles
                join roles in _es.GetAll<IdentityRole>() on userRoles.RoleId equals roles.Id
                select roles.Name;
            return Task.FromResult<IList<string>>(source.ToList<string>());
        }
        /// <summary>
        ///     Returns true if the user is in the named role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public virtual Task<bool> IsInRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("ValueCannotBeNullOrEmpty", "roleName");
            }
            bool result = (
                from r in _es.GetAll<IdentityRole>()
                where r.Name.ToUpper() == roleName.ToUpper()
                where r.Users.Any((IdentityUserRole ur) => ur.UserId.Equals(user.Id))
                select r).Count<IdentityRole>() > 0;
            return Task.FromResult<bool>(result);
        }
        /// <summary>
        ///     Set the security stamp for the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="stamp"></param>
        /// <returns></returns>
        public Task SetSecurityStampAsync(User user, string stamp)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.SecurityStamp = stamp;
            return Task.FromResult<int>(0);
        }
        /// <summary>
        ///     Get the security stamp for a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetSecurityStampAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<string>(user.SecurityStamp);
        }
        /// <summary>
        ///     Set the Two Factor provider for the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.TwoFactorEnabled = enabled;
            return Task.FromResult<int>(0);
        }
        /// <summary>
        ///     Get the two factor provider for the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<bool>(user.TwoFactorEnabled);
        }
        private async Task SaveChanges()
        {
            if (this.AutoSaveChanges)
            {
                await _es.SaveAsync();
                //await this.Context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
        private Task<User> GetUserAggregateAsync(Expression<Func<User, bool>> filter)
        {
            return QueryableExtensions.FirstOrDefaultAsync<User>(QueryableExtensions.Include<User, ICollection<IdentityUserLogin>>(QueryableExtensions.Include<User, ICollection<IdentityUserClaim>>(QueryableExtensions.Include<User, ICollection<IdentityUserRole>>(this.Users, (User u) => u.Roles), (User u) => u.Claims), (User u) => u.Logins), filter);
        }

        public void Dispose()
        {
        }
    }
}
