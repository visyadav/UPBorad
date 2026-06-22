using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Common;

public interface ICommonServices
{
    Task<string?> SaveFileAsync(IFormFile? file, CancellationToken cancellationToken);
}