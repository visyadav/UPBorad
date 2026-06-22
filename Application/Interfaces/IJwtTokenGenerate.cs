namespace Application.Interfaces;

public interface IJwtTokenGenerate
{
    string GenerateToken(Guid userId, string email, int roleId, string roleName, Guid securityStamp, List<string> permissions);
}