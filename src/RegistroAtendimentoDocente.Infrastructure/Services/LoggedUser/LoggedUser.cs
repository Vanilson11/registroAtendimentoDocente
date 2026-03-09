using Microsoft.EntityFrameworkCore;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Domain.Security.Tokens;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;
using RegistroAtendimentoDocente.Infrastructure.DataAccess;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RegistroAtendimentoDocente.Infrastructure.Services.LoggedUser;
public class LoggedUser : ILoggedUser
{
    private readonly RegistroAtendimentoDocenteDbContext _context;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(RegistroAtendimentoDocenteDbContext context, ITokenProvider tokenProvider)
    {
        _context = context;
        _tokenProvider = tokenProvider;
    }
    public async Task<User> Get()
    {
        var token = _tokenProvider.GetTokenOnRequest();
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(token);
        var userIdentifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

        return await _context.Users.AsNoTracking().FirstAsync(user => user.UserIdentifier == Guid.Parse(userIdentifier));
    }
}
