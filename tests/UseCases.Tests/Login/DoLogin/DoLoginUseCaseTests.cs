using CommonTestUtilities.Entities;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security.Tokens;
using RegistroAtendimentoDocente.Application.UseCases.Login.DoLogin;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Domain.Entities;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Login.DoLogin;
public class DoLoginUseCaseTests
{
    private DoLginUseCase CreateUseCase(User? user, string? password = null)
    {
        var readOnlyRepository = new ReadOnlyUsersRepositoryBuilder().GetByEmail(user!).Build();
        var passwordEncrypter = new PasswordEncripterBuilder().Verify(password!).Build();
        var tokenGenerator = TokenGeneratorBuilder.Build();
        return new DoLginUseCase(readOnlyRepository, passwordEncrypter, tokenGenerator);
    }

    private RequestDoLoginJson CreateRequest()
    {
        return RequestDoLoginJsonBuilder.Build();
    }

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = CreateRequest();
        request.Email = user.Email;
        var useCase = CreateUseCase(user, request.Password);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name = user.Name;
        result.Token.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_User_Not_Exists()
    {
        var request = CreateRequest();
        var useCase = CreateUseCase(null, request.Password);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<InvalidLoginException>();
        result.GetErrors().Count.ShouldBe(1);
        var errorMessage = result.GetErrors().FirstOrDefault();
        errorMessage.ShouldBe(ResourceErrorMessages.INVALID_LOGIN);
    }

    [Fact]
    public async Task Error_Password_Unmatch()
    {
        var user = UserBuilder.Build();
        var request = CreateRequest();
        request.Email = user.Email;
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<InvalidLoginException>();
        result.GetErrors().Count.ShouldBe(1);
        var errorMessage = result.GetErrors().FirstOrDefault();
        errorMessage.ShouldBe(ResourceErrorMessages.INVALID_LOGIN);
    }
}
