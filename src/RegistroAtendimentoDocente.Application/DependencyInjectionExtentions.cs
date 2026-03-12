using Microsoft.Extensions.DependencyInjection;
using RegistroAtendimentoDocente.Application.AutoMapper;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Delete;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.GetAll;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.GetById;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Register;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Reports.Excel;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Reports.Pdf;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Update;
using RegistroAtendimentoDocente.Application.UseCases.Login.DoLogin;
using RegistroAtendimentoDocente.Application.UseCases.Users.DeleteProfile;
using RegistroAtendimentoDocente.Application.UseCases.Users.GetAll;
using RegistroAtendimentoDocente.Application.UseCases.Users.GetById;
using RegistroAtendimentoDocente.Application.UseCases.Users.GetProfile;
using RegistroAtendimentoDocente.Application.UseCases.Users.Register;
using RegistroAtendimentoDocente.Application.UseCases.Users.Update;
using RegistroAtendimentoDocente.Application.UseCases.Users.UpdateProfile;

namespace RegistroAtendimentoDocente.Application;
public static class DependencyInjectionExtentions
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddAutoMapper(services);
        AddUseCases(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapping));
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterAtendimentoUseCase, RegisterAtendimentoUseCase>();
        services.AddScoped<IGetAllAtendimentosUseCase, GetAllAtendimentosUseCase>();
        services.AddScoped<IGetAtendimentoByIdUseCase, GetAtendimentoByIdUseCase>();
        services.AddScoped<IDeleteAtendimentosUseCase, DeleteAtendimentosUseCase>();
        services.AddScoped<IUpdateAtendimentoUseCase, UpdateAtendimentoUseCase>();
        services.AddScoped<IReportExcelServicesUseCase, ReportExcelServicesUseCase>();
        services.AddScoped<IReportPdfServicesUseCase, ReportPdfServicesUseCase>();
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLginUseCase, DoLginUseCase>();
        services.AddScoped<IGetProfileUserUseCase, GetProfileUserUseCase>();
        services.AddScoped<IUpdateProfileUserUseCase, UpdateProfileUserUseCase>();
        services.AddScoped<IDeleteProfileUserUseCase, DeleteProfileUserUseCase>();
        services.AddScoped<IGetAllUsersUseCase, GetAllUsersUseCase>();
        services.AddScoped<IGetUserByIdUseCase, GetUserByIdUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
    }
}
