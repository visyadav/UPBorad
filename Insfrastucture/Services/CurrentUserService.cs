namespace Insfrastucture.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public Guid? UserId
    {
        get
        {
            var sub = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(sub, out var id) ? id : null;
        }
    }

    public int? RoleId
    {
        get
        {
            var roleIdStr = User?.FindFirst("roleId")?.Value;
            return int.TryParse(roleIdStr, out var id) ? id : null;
        }
    }

    public string? RoleName => User?.FindFirst(ClaimTypes.Role)?.Value;

    public bool IsSuperAdmin =>
        string.Equals(RoleName, "SuperAdmin", StringComparison.OrdinalIgnoreCase);

    public bool HasPermission(string module, string operation)
    {

        // Permission claims are stored as "ModuleName:Operation" (e.g., "Grievances:Read")
        var permissionClaim = $"{module}:{operation}";
        var result = User?.Claims.Any(c => c.Type == "permission" && c.Value == permissionClaim) ?? false;
        return User?.Claims.Any(c => c.Type == "permission" && c.Value == permissionClaim) ?? false;
    }
}