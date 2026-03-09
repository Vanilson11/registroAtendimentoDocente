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
using RegistroAtendimentoDocente.Application.UseCases.Users.Register;

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
        services.AddScoped<IReportFilterServicesByMonthUseCase, ReportFilterServicesByMonthUseCase>();
        services.AddScoped<IReportPdfServicesUseCase, ReportPdfServicesUseCase>();
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLginUseCase, DoLginUseCase>();
    }
}
