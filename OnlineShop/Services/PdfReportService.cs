using OnlineShop.Areas.Admin.Models;
using OnlineShop.Models.Db;
using OnlineShop.Models.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace OnlineShop.Services;

public class PdfReportService
{
    public static byte[] GenerateReport(ReportViewModel model)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var monthNames = CultureInfo.GetCultureInfo("vi-VN").DateTimeFormat.MonthNames;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);

                page.Header().Text("📊 BÁO CÁO THỐNG KÊ NĂM " + DateTime.Now.Year)
                    .FontSize(20).Bold().FontColor(Colors.Blue.Medium);

                page.Content().Column(col =>
                {
                    // Tổng quan
                    col.Item().Text("TỔNG QUAN").FontSize(14).Bold();
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c => { c.RelativeColumn(); c.RelativeColumn(); });

                        table.Cell().Text("Tổng người dùng");
                        table.Cell().Text(model.UserStats.Sum(x => x.Count).ToString());

                        table.Cell().Text("Tổng đơn hàng");
                        table.Cell().Text(model.OrderStats.Sum(x => x.OrderCount).ToString());

                        table.Cell().Text("Tổng doanh thu");
                        table.Cell().Text($"{model.OrderStats.Sum(x => x.TotalRevenue):N0} VND");
                    });

                    col.Item().PaddingVertical(10);

                    // Thống kê theo tháng
                    col.Item().Text("THỐNG KÊ THEO THÁNG").FontSize(14).Bold();
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.ConstantColumn(60);
                            c.RelativeColumn();
                            c.RelativeColumn();
                            c.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Tháng").Bold();
                            header.Cell().Text("Người dùng").Bold();
                            header.Cell().Text("Đơn hàng").Bold();
                            header.Cell().Text("Doanh thu").Bold();
                        });

                        for (int m = 1; m <= 12; m++)
                        {
                            var users = model.UserStats.FirstOrDefault(x => x.Month == m)?.Count ?? 0;
                            var orders = model.OrderStats.FirstOrDefault(x => x.Month == m)?.OrderCount ?? 0;
                            var revenue = model.OrderStats.FirstOrDefault(x => x.Month == m)?.TotalRevenue ?? 0;

                            table.Cell().Text("Tháng " + m);
                            table.Cell().Text(users.ToString());
                            table.Cell().Text(orders.ToString());
                            table.Cell().Text($"{revenue:N0} VND");
                        }
                    });

                    col.Item().PaddingVertical(10);

                    // Top bán chạy
                    col.Item().Text("TOP SẢN PHẨM BÁN CHẠY").FontSize(14).Bold();
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.ConstantColumn(30);
                            c.RelativeColumn(); // Tên
                            c.ConstantColumn(50); // SL
                            c.ConstantColumn(70); // Giá
                            c.ConstantColumn(70); // Doanh thu
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("#").Bold();
                            header.Cell().Text("Tên sản phẩm").Bold();
                            header.Cell().Text("Bán").Bold();
                            header.Cell().Text("Giá").Bold();
                            header.Cell().Text("Doanh thu").Bold();
                        });

                        int i = 1;
                        foreach (var p in model.TopSellingProducts)
                        {
                            var sold = p.TotalSum ?? 0;
                            var price = p.Price ?? 0;
                            var discount = p.Discount ?? 0;
                            var realPrice = price - discount;
                            var revenue = realPrice * sold;

                            table.Cell().Text(i++.ToString());
                            table.Cell().Text(p.Title ?? "");
                            table.Cell().Text(sold.ToString());
                            table.Cell().Text($"{realPrice:N0}");
                            table.Cell().Text($"{revenue:N0}");
                        }
                    });

                    col.Item().PaddingVertical(10);

                    col.Item().Text("Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy"))
                        .FontSize(10).Italic();
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.CurrentPageNumber().FontSize(10);
                    text.Span(" / ");
                    text.TotalPages().FontSize(10);
                });

            });
        });

        return document.GeneratePdf();
    }
}
