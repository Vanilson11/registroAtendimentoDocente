using RegistroAtendimentoDocente.Domain.Entities;

namespace WeApi.Test.Resources;
public class UserIdentityManager
{
    private readonly string _password;
    private readonly string _token;
    private readonly User _user;

    public UserIdentityManager(User user, string password, string token)
    {
        _user = user;
        _token = token;
        _password = password;
    }

    public string GetName() => _user.Name;
    public string GetPassword() => _password;
    public string GetEmail() => _user.Email;
    public string GetToken() => _token;
    public long GetId() => _user.Id;
    public User GetUser() => _user;
}
