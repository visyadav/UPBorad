namespace Application.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    int? RoleId { get; }
    string? RoleName { get; }
    bool IsSuperAdmin { get; }
    bool HasPermission(string module, string operation);
}