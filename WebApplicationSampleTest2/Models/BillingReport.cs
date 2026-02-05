using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;

namespace WebApplicationSampleTest2.Models
{
    public class BillingReport
    {
        public BillingReport()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public bool GenerateReport(BillingReportModel reportModel)
        {
            try
            {
                var document = CreateDocument(reportModel);
                var pdf = document.GeneratePdf();

                var outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "reports");
                Directory.CreateDirectory(outputDirectory);

                var safePatientName = (reportModel.PatientName ?? "Patient")
                    .Replace(" ", "_")
                    .Replace(Path.DirectorySeparatorChar, '_')
                    .Replace(Path.AltDirectorySeparatorChar, '_');

                var fileName = $"Billing_{safePatientName}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                var filePath = Path.Combine(outputDirectory, fileName);

                File.WriteAllBytes(filePath, pdf);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool GenrateReport(BillingReportModel reportModel)
        {
            return GenerateReport(reportModel);
        }

        private static IDocument CreateDocument(BillingReportModel reportModel)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);

                    page.Header().Column(column =>
                    {
                        column.Item().Text(reportModel.HospitalName ?? "Hospital").FontSize(22).Bold().FontColor(Colors.Blue.Medium).AlignCenter();
                        column.Item().Text(reportModel.HospitalAddress ?? string.Empty).FontSize(12).AlignCenter();
                        column.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Black);

                        column.Item().Text(text =>
                        {
                            text.Span("Patient Name: ").SemiBold();
                            text.Span(reportModel.PatientName ?? string.Empty);
                        });

                        column.Item().Text(text =>
                        {
                            text.Span("Visit Time: ").SemiBold();
                            text.Span(reportModel.VisitTime.ToString("g"));
                        });
                    });

                    page.Content().Column(column =>
                    {
                        column.Item().PaddingTop(10).Text("Billing Details").FontSize(16).Bold();

                        foreach (var item in reportModel.BillingDetails)
                        {
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Text(item.Key);
                                row.ConstantItem(120).AlignRight().Text(item.Value);
                            });
                        }

                        column.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Text("Total").SemiBold();
                            row.ConstantItem(120).AlignRight().Text(reportModel.TotalAmount.ToString());
                        });
                    });

                    page.Footer().AlignCenter().Text("Power By OpsTeQ Software Pvt Ltd.");
                });
            });
        }
    }
}
