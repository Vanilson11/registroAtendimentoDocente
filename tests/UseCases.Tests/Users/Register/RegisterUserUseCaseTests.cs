using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Users;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security.Cryptograph;
using CommonTestUtilities.Security.Tokens;
using RegistroAtendimentoDocente.Application.UseCases.Users.Register;
using RegistroAtendimentoDocente.Communication.Requests;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Users.Register;
public class RegisterUserUseCaseTests
{
    private RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var passwordEncripter = new PasswordEncripterBuilder().Build();
        var readOnlyRepository = new ReadOnlyUsersRepositoryBuilder().ExistActiveUserWithEmail(email!).Build();
        var writeOnlyRepository = WriteOnlyUsersRepositoryBuilder.Build();
        var unitOffWork = UnitOffWorkBuilder.Build();
        var tokenGenerator = TokenGeneratorBuilder.Build();

        return new RegisterUserUseCase(mapper, passwordEncripter, readOnlyRepository, writeOnlyRepository, unitOffWork, tokenGenerator);
    }

    private RequestRegisterUserJson CreateRequest()
    {
        return RequestRegisterUserJsonBuilder.Build();
    }

    [Fact]
    public async Task Success()
    {
        var useCase = CreateUseCase();
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
        result.Token.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = CreateRequest();
        request.Name = string.Empty;
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        result.GetErrors().Count.ShouldBe(1);
        var errorMessage = result.GetErrors().FirstOrDefault();
        errorMessage.ShouldBe(ResourceErrorMessages.NAME_USER_EMPTY);
    }

    [Fact]
    public async Task Error_Email_User_Exists()
    {
        var request = CreateRequest();
        var useCase = CreateUseCase(request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        result.GetErrors().Count.ShouldBe(1);
        var errorMessage = result.GetErrors().FirstOrDefault();
        errorMessage.ShouldBe(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED);
    }
}
