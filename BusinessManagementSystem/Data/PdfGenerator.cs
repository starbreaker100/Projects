using System;
using System.IO;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using BusinessManagementSystem.Models;

namespace BusinessManagementSystem.Data
{
    public class PdfGenerator
    {
        public static void GenerateOrderReceipt(Order order, string companyName = "Your Business Name", 
            string companyAddress = "Your Business Address", string companyPhone = "Your Phone Number")
        {
            try
            {
                // Create directory if it doesn't exist
                string outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Receipts");
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                string fileName = $"Receipt_Order_{order.OrderID}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                string filePath = Path.Combine(outputDir, fileName);

                // Create document
                Document document = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                
                document.Open();

                // Fonts
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLACK);
                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.BLACK);
                Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);
                Font smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.BLACK);

                // Company Header
                Paragraph companyTitle = new Paragraph(companyName, titleFont);
                companyTitle.Alignment = Element.ALIGN_CENTER;
                document.Add(companyTitle);

                Paragraph companyInfo = new Paragraph($"{companyAddress}\nPhone: {companyPhone}", normalFont);
                companyInfo.Alignment = Element.ALIGN_CENTER;
                companyInfo.SpacingAfter = 20f;
                document.Add(companyInfo);

                // Receipt Title
                Paragraph receiptTitle = new Paragraph("SALES RECEIPT", headerFont);
                receiptTitle.Alignment = Element.ALIGN_CENTER;
                receiptTitle.SpacingAfter = 15f;
                document.Add(receiptTitle);

                // Order Information
                PdfPTable orderInfoTable = new PdfPTable(4);
                orderInfoTable.WidthPercentage = 100;
                orderInfoTable.SetWidths(new float[] { 1f, 1f, 1f, 1f });

                orderInfoTable.AddCell(new PdfPCell(new Phrase("Order ID:", headerFont)) { Border = Rectangle.NO_BORDER });
                orderInfoTable.AddCell(new PdfPCell(new Phrase(order.OrderID.ToString(), normalFont)) { Border = Rectangle.NO_BORDER });
                orderInfoTable.AddCell(new PdfPCell(new Phrase("Date:", headerFont)) { Border = Rectangle.NO_BORDER });
                orderInfoTable.AddCell(new PdfPCell(new Phrase(order.OrderDate.ToString("dd/MM/yyyy HH:mm"), normalFont)) { Border = Rectangle.NO_BORDER });

                orderInfoTable.AddCell(new PdfPCell(new Phrase("Customer:", headerFont)) { Border = Rectangle.NO_BORDER });
                orderInfoTable.AddCell(new PdfPCell(new Phrase(order.CustomerName ?? "Walk-in Customer", normalFont)) { Border = Rectangle.NO_BORDER });
                orderInfoTable.AddCell(new PdfPCell(new Phrase("Phone:", headerFont)) { Border = Rectangle.NO_BORDER });
                orderInfoTable.AddCell(new PdfPCell(new Phrase(order.CustomerPhone ?? "-", normalFont)) { Border = Rectangle.NO_BORDER });

                orderInfoTable.AddCell(new PdfPCell(new Phrase("Seller:", headerFont)) { Border = Rectangle.NO_BORDER });
                orderInfoTable.AddCell(new PdfPCell(new Phrase(order.SellerName, normalFont)) { Border = Rectangle.NO_BORDER });
                orderInfoTable.AddCell(new PdfPCell(new Phrase("", headerFont)) { Border = Rectangle.NO_BORDER });
                orderInfoTable.AddCell(new PdfPCell(new Phrase("", normalFont)) { Border = Rectangle.NO_BORDER });

                orderInfoTable.SpacingAfter = 15f;
                document.Add(orderInfoTable);

                // Items Table
                PdfPTable itemsTable = new PdfPTable(6);
                itemsTable.WidthPercentage = 100;
                itemsTable.SetWidths(new float[] { 0.5f, 2f, 0.8f, 0.8f, 0.8f, 1f });

                // Headers
                itemsTable.AddCell(new PdfPCell(new Phrase("No.", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY });
                itemsTable.AddCell(new PdfPCell(new Phrase("Item", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY });
                itemsTable.AddCell(new PdfPCell(new Phrase("Qty", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY });
                itemsTable.AddCell(new PdfPCell(new Phrase("Unit Price", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY });
                itemsTable.AddCell(new PdfPCell(new Phrase("Adj%", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY });
                itemsTable.AddCell(new PdfPCell(new Phrase("Total", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY });

                // Items
                int itemNo = 1;
                foreach (var item in order.OrderDetails)
                {
                    itemsTable.AddCell(new PdfPCell(new Phrase(itemNo.ToString(), normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    itemsTable.AddCell(new PdfPCell(new Phrase($"{item.ProductName} ({item.Unit})", normalFont)));
                    itemsTable.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString("0.##"), normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    itemsTable.AddCell(new PdfPCell(new Phrase($"₹{item.UnitPrice:0.00}", normalFont)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    itemsTable.AddCell(new PdfPCell(new Phrase($"{item.ProductAdjustmentPercentage:0.##}%", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    itemsTable.AddCell(new PdfPCell(new Phrase($"₹{item.LineTotal:0.00}", normalFont)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    itemNo++;
                }

                itemsTable.SpacingAfter = 10f;
                document.Add(itemsTable);

                // Totals Table
                PdfPTable totalsTable = new PdfPTable(2);
                totalsTable.WidthPercentage = 50;
                totalsTable.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalsTable.SetWidths(new float[] { 1f, 1f });

                totalsTable.AddCell(new PdfPCell(new Phrase("Subtotal:", headerFont)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT });
                totalsTable.AddCell(new PdfPCell(new Phrase($"₹{order.SubTotal:0.00}", normalFont)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT });

                if (order.GlobalAdjustmentPercentage != 0)
                {
                    totalsTable.AddCell(new PdfPCell(new Phrase($"Global Adjustment ({order.GlobalAdjustmentPercentage:0.##}%):", headerFont)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT });
                    decimal adjustmentAmount = order.TotalAmount - order.SubTotal;
                    totalsTable.AddCell(new PdfPCell(new Phrase($"₹{adjustmentAmount:0.00}", normalFont)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT });
                }

                totalsTable.AddCell(new PdfPCell(new Phrase("Total Amount:", headerFont)) { Border = Rectangle.TOP, HorizontalAlignment = Element.ALIGN_RIGHT });
                totalsTable.AddCell(new PdfPCell(new Phrase($"₹{order.TotalAmount:0.00}", headerFont)) { Border = Rectangle.TOP, HorizontalAlignment = Element.ALIGN_RIGHT });

                totalsTable.SpacingAfter = 20f;
                document.Add(totalsTable);

                // Notes
                if (!string.IsNullOrWhiteSpace(order.Notes))
                {
                    Paragraph notesTitle = new Paragraph("Notes:", headerFont);
                    document.Add(notesTitle);
                    Paragraph notes = new Paragraph(order.Notes, normalFont);
                    notes.SpacingAfter = 15f;
                    document.Add(notes);
                }

                // Footer
                Paragraph footer = new Paragraph("Thank you for your business!", normalFont);
                footer.Alignment = Element.ALIGN_CENTER;
                footer.SpacingBefore = 30f;
                document.Add(footer);

                Paragraph timestamp = new Paragraph($"Generated on: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", smallFont);
                timestamp.Alignment = Element.ALIGN_CENTER;
                document.Add(timestamp);

                document.Close();

                // Open the PDF
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating PDF: {ex.Message}", ex);
            }
        }
    }
}