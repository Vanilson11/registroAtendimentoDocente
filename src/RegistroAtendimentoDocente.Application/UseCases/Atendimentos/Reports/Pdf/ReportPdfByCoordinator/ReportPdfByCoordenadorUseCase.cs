using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Reports.Pdf.Fonts;
using RegistroAtendimentoDocente.Domain.Enums;
using RegistroAtendimentoDocente.Domain.Reports;
using RegistroAtendimentoDocente.Domain.Repositories.Atendimentos;
using RegistroAtendimentoDocente.Domain.Repositories.Users;
using RegistroAtendimentoDocente.Domain.Services.LoggedUser;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;
using System.Reflection;

namespace RegistroAtendimentoDocente.Application.UseCases.Atendimentos.Reports.Pdf.ReportPdfByCoordinator;

public class ReportPdfByCoordenadorUseCase : IReportPdfByCoordenadorUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IAtendimentosReadOnlyRepository _readOnlyAtendimentosRepository;
    private readonly IReadOnlyUsersRepository _readOnlyUsersRepository;
    public ReportPdfByCoordenadorUseCase(
        ILoggedUser loggedUser,
        IAtendimentosReadOnlyRepository atendimentosReadOnlyRepository,
        IReadOnlyUsersRepository readOnlyUsersRepository
        )
    {
        _loggedUser = loggedUser;
        _readOnlyAtendimentosRepository = atendimentosReadOnlyRepository;
        _readOnlyUsersRepository = readOnlyUsersRepository;
        GlobalFontSettings.FontResolver = new ServicesReportFontResolver();
    }
    public async Task<byte[]> Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();

        var user = await _readOnlyUsersRepository.GetById(id);

        if (user is null) throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);

        if (user.Role is not Roles.COORDENADOR)
            throw new UserNotCoordinatorException(ResourceErrorMessages.USER_IS_NOT_COORDINATOR);

        var atendimentos = await _readOnlyAtendimentosRepository.GetAll(user);

        if (atendimentos.Count == 0) return [];

        var document = CreateDocument(coordinatorName: user.Name, loggedUserName: loggedUser.Name);

        var page = CreatePage(document);

        CreateHeaderWithLogoAndText(page, coordinatorName: user.Name);

        var table = CreateTable(page);

        foreach (var atendimento in atendimentos)
        {
            var row = table.AddRow();
            row.Height = 44;

            row.Cells[0].AddParagraph(atendimento.Docente);
            SetStyleBaseForServiceInformation(row.Cells[0]);

            row.Cells[1].AddParagraph(atendimento.Assunto);
            SetStyleBaseForServiceInformation(row.Cells[1]);

            if (string.IsNullOrWhiteSpace(atendimento.Encaminhamento) == false)
            {
                row.Cells[2].AddParagraph(atendimento.Encaminhamento);
                SetStyleBaseForServiceInformation(row.Cells[2]);
            }

            if (string.IsNullOrWhiteSpace(atendimento.Observacao) == false)
            {
                row.Cells[3].AddParagraph(atendimento.Observacao);
                SetStyleBaseForServiceInformation(row.Cells[3]);
            }

            row.Cells[4].AddParagraph(atendimento.Data.ToString("d"));
            SetStyleBaseForServiceInformation(row.Cells[4]);

            row.Cells[5].AddParagraph(user.Name);
            SetStyleBaseForServiceInformation(row.Cells[5]);
        }

        return RenderDocument(document);
    }

    private Document CreateDocument(string coordinatorName, string loggedUserName)
    {
        var document = new Document();

        document.Info.Title = string.Format(ResourceReportsMessages.ALL_SERVICES_OFF_COORDINATOR, coordinatorName);
        document.Info.Author = loggedUserName;

        var style = document.Styles["Normal"];
        style!.Font.Name = FontsHelper.PT_SERIF_REGULAR;
        style!.Font.Size = 12;

        return document;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();
        section.PageSetup.Orientation = Orientation.Landscape;
        section.PageSetup.PageFormat = PageFormat.A4;

        section.PageSetup.PageWidth = Unit.FromMillimeter(297);
        section.PageSetup.PageHeight = Unit.FromMillimeter(210);

        section.PageSetup.TopMargin = Unit.FromMillimeter(20);
        section.PageSetup.BottomMargin = Unit.FromMillimeter(20);
        section.PageSetup.LeftMargin = Unit.FromMillimeter(15);
        section.PageSetup.RightMargin = Unit.FromMillimeter(15);

        return section;
    }

    private void CreateHeaderWithLogoAndText(Section page, string coordinatorName)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.Alignment = ParagraphAlignment.Center;

        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var pathFile = Path.Combine(directoryName!, "Logo", "logo-seduc.jpg");

        paragraph.AddImage(pathFile);
        paragraph.AddLineBreak();

        paragraph.AddFormattedText(ResourceReportsMessages.DEPARTMENT_EDUCATION, new Font { Name = FontsHelper.PT_SERIF_BOLD, Size = 14 });
        paragraph.AddLineBreak();

        paragraph.AddFormattedText(ResourceReportsMessages.SCHOOL_NAME, new Font { Name = FontsHelper.PT_SERIF_BOLD, Size = 14 });
        paragraph.AddLineBreak();
        paragraph.AddLineBreak();

        var title = string.Format(ResourceReportsMessages.ALL_SERVICES_OFF_COORDINATOR, coordinatorName);
        paragraph.AddFormattedText(title, new Font { Name = FontsHelper.PT_SERIF_BOLD, Size = 12 });
        paragraph.Format.SpaceAfter = "30";
    }

    private Table CreateTable(Section page)
    {
        var table = page.AddTable();
        table.Format.Alignment = ParagraphAlignment.Left;
        table.Borders.Width = 0.5;

        table.AddColumn("135").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("135").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("135").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("135").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("90").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("135").Format.Alignment = ParagraphAlignment.Left;

        var row = table.AddRow();

        row.Cells[0].AddParagraph(ResourceReportsMessages.TEACHER);
        CustomizeHeaderTextTable(row.Cells[0]);

        row.Cells[1].AddParagraph(ResourceReportsMessages.SUBJECT);
        CustomizeHeaderTextTable(row.Cells[1]);

        row.Cells[2].AddParagraph(ResourceReportsMessages.FOWARDING);
        CustomizeHeaderTextTable(row.Cells[2]);

        row.Cells[3].AddParagraph(ResourceReportsMessages.OBSERVATION);
        CustomizeHeaderTextTable(row.Cells[3]);

        row.Cells[4].AddParagraph(ResourceReportsMessages.DATE);
        CustomizeHeaderTextTable(row.Cells[4]);

        row.Cells[5].AddParagraph(ResourceReportsMessages.COORDINATOR);
        CustomizeHeaderTextTable(row.Cells[5]);

        return table;
    }

    private void CustomizeHeaderTextTable(Cell row)
    {
        row.Format.Font = new Font { Name = FontsHelper.PT_SERIF_BOLD, Size = 12 };
        row.Format.Alignment = ParagraphAlignment.Center;
        row.Borders.Visible = true;
    }

    private void SetStyleBaseForServiceInformation(Cell row)
    {
        row.Format.Font = new Font { Name = FontsHelper.PT_SERIF_REGULAR, Size = 12 };
        row.VerticalAlignment = VerticalAlignment.Center;
    }

    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document,
        };

        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }
}
