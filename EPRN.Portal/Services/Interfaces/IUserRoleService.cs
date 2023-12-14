using EPRN.Common.Enums;

namespace EPRN.Portal.Services.Interfaces
{
    public interface IUserRoleService
    {
        bool HasRole(UserRole roleToTestFor);

        void SetRole(UserRole userRole);

        void RemoveRole(UserRole userRole);
    }
}
