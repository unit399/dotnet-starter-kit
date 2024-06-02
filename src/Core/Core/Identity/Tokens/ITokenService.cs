using System.Threading;
using System.Threading.Tasks;
using ROC.WebApi.Core.Identity.Tokens.Features.Generate;

namespace ROC.WebApi.Core.Identity.Tokens;

public interface ITokenService
{
    Task<TokenGenerationResponse> GenerateTokenAsync(TokenGenerationCommand request, string ipAddress,
        CancellationToken cancellationToken);
}