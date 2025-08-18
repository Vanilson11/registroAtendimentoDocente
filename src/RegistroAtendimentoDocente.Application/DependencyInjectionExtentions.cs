using Microsoft.Extensions.DependencyInjection;
using RegistroAtendimentoDocente.Application.AutoMapper;
using RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Delete;
using RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.GetAll;
using RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.GetById;
using RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Register;
using RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Reports.Excel;
using RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Reports.Pdf;
using RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Update;

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
    }
}
