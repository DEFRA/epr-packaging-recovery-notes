using EPRN.Common.Enums;
using EPRN.Portal.Services.Interfaces;

namespace EPRN.Portal.Services
{
    /// <summary>
    /// This is a temporary solution to allow the system to
    /// perform logic based operations based on a mocked 
    /// user role given that we do not have an authentication app
    /// at the current time
    /// </summary>
    public class UserRoleService : IUserRoleService
    {
        private readonly List<UserRole> _roles = new List<UserRole>();

        public bool HasRole(UserRole roleToTestFor) => _roles.Contains(roleToTestFor);

        public void RemoveRole(UserRole userRole)
        {
            _roles.Remove(userRole);
        }

        public void SetRole(UserRole userRole)
        {
            if (userRole == UserRole.None)
                return;

            if (!_roles.Contains(userRole))
            {
                _roles.Add(userRole);
            }
        }
    }
}
