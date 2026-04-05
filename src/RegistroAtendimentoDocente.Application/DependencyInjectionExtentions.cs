using Microsoft.Extensions.DependencyInjection;
using RegistroAtendimentoDocente.Application.AutoMapper;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.Delete;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.GetAll;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.GetById;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.Register;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.Reports.Excel.ReportByCoordinator;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.Reports.Excel.ReportExcelConsultations;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.Reports.Pdf.ReportPdfByCoordinator;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.Reports.Pdf.ReportPdfConsultationsUseCase;
using RegistroAtendimentoDocente.Application.UseCases.Consultations.Update;
using RegistroAtendimentoDocente.Application.UseCases.Login.DoLogin;
using RegistroAtendimentoDocente.Application.UseCases.Users.ChangePassword;
using RegistroAtendimentoDocente.Application.UseCases.Users.Delete;
using RegistroAtendimentoDocente.Application.UseCases.Users.DeleteProfile;
using RegistroAtendimentoDocente.Application.UseCases.Users.GetAll;
using RegistroAtendimentoDocente.Application.UseCases.Users.GetAllCoordinators;
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
        services.AddScoped<IRegisterConsultationUseCase, RegisterConsultationUseCase>();
        services.AddScoped<IGetAllConsultationsUseCase, GetAllConsultationsUseCase>();
        services.AddScoped<IGetConsultationByIdUseCase, GetConsultationByIdUseCase>();
        services.AddScoped<IDeleteConsultationUseCase, DeleteConsultationUseCase>();
        services.AddScoped<IUpdateConsultationUseCase, UpdateAtendimentoUseCase>();
        services.AddScoped<IReportExcelConsultationsUseCase, ReportExcelConsultationUseCase>();
        services.AddScoped<IReportPdfConsultationsUseCase, ReportPdfConsultationsUseCase>();
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLginUseCase, DoLginUseCase>();
        services.AddScoped<IGetProfileUserUseCase, GetProfileUserUseCase>();
        services.AddScoped<IUpdateProfileUserUseCase, UpdateProfileUserUseCase>();
        services.AddScoped<IDeleteProfileUserUseCase, DeleteProfileUserUseCase>();
        services.AddScoped<IGetAllUsersUseCase, GetAllUsersUseCase>();
        services.AddScoped<IGetUserByIdUseCase, GetUserByIdUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
        services.AddScoped<IReportExcelByCoordinatorUseCase, ReportExcelByCoordinatorUseCase>();
        services.AddScoped<IReportPdfByCoordinatorUseCase, ReportPdfByCoordinatorUseCase>();
        services.AddScoped<IGetAllCoordinatorsUseCase, GetAllCoordinatorsUseCase>();
    }
}
