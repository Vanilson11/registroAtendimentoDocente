using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Reports.Pdf.Fonts;
using RegistroAtendimentoDocente.Domain.Reports;
using RegistroAtendimentoDocente.Domain.Repositories.AtendimentoRepository;
using System.Reflection;

namespace RegistroAtendimentoDocente.Application.UseCases.RegistroAtendimento.Service.Reports.Pdf;
public class ReportPdfServicesUseCase : IReportPdfServicesUseCase
{
    private readonly IAtendimentosReadOnlyRepository _repository;

    public ReportPdfServicesUseCase(IAtendimentosReadOnlyRepository repository)
    {
        _repository = repository;
        GlobalFontSettings.FontResolver = new ServicesReportFontResolver();
    }
    public async Task<byte[]> Execute(DateOnly month)
    {
        var services = await _repository.FilterServicesByMonth(month);

        if (services.Count == 0)
            return [];

        var document = CreateDocument(month);

        var page = CreatePage(document);

        CreateHeaderWithLogoAndText(page, month);

        var table = CreateTable(page);

        foreach(var service in services)
        {
            var row = table.AddRow();
            row.Height = 44;

            row.Cells[0].AddParagraph(service.Docente);
            SetStyleBaseForServiceInformation(row.Cells[0]);

            row.Cells[1].AddParagraph(service.Assunto);
            SetStyleBaseForServiceInformation(row.Cells[1]);

            if (string.IsNullOrWhiteSpace(service.Observacao) == false &&
                string.IsNullOrWhiteSpace(service.Encaminhamento) == false)
            {
                row.Cells[2].AddParagraph(service.Observacao);
                SetStyleBaseForServiceInformation(row.Cells[2]);

                row.Cells[4].AddParagraph(service.Encaminhamento);
                SetStyleBaseForServiceInformation(row.Cells[4]);
            }

            row.Cells[3].AddParagraph(service.Data.ToString("d"));
            SetStyleBaseForServiceInformation(row.Cells[3]);
        }

        return RenderDocument(document);
    }

    private Document CreateDocument(DateOnly month)
    {
        var document = new Document();

        document.Info.Title = string.Format(ResourceReportsMessages.SERVICES_PROVIDED_AT, month.ToString("Y"));
        document.Info.Author = "Vanilson Sousa";

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

    private void CreateHeaderWithLogoAndText(Section page, DateOnly month) {
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

        var title = string.Format(ResourceReportsMessages.SERVICES_PROVIDED_AT, month.ToString("Y"));
        paragraph.AddFormattedText(title, new Font { Name = FontsHelper.PT_SERIF_BOLD, Size = 12 });
        paragraph.Format.SpaceAfter = "30";
    }

    private Table CreateTable(Section page)
    {
        var table = page.AddTable();
        table.Format.Alignment = ParagraphAlignment.Left;
        table.Borders.Width = 0.5;

        table.AddColumn("130").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("190").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("180").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("90").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("180").Format.Alignment = ParagraphAlignment.Left;

        var row = table.AddRow();

        row.Cells[0].AddParagraph(ResourceReportsMessages.TEACHER);
        CustomizeHeaderTextTable(row.Cells[0]);

        row.Cells[1].AddParagraph(ResourceReportsMessages.SUBJECT);
        CustomizeHeaderTextTable(row.Cells[1]);

        row.Cells[2].AddParagraph(ResourceReportsMessages.OBSERVATION);
        CustomizeHeaderTextTable(row.Cells[2]);

        row.Cells[3].AddParagraph(ResourceReportsMessages.DATE);
        CustomizeHeaderTextTable(row.Cells[3]);

        row.Cells[4].AddParagraph(ResourceReportsMessages.FOWARDING);
        CustomizeHeaderTextTable(row.Cells[4]);

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
