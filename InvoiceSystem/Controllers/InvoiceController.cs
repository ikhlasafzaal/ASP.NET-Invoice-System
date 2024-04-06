using Microsoft.AspNetCore.Mvc;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using InvoiceSystem.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace InvoiceSystem.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly DatabaseContext _context;

        public InvoiceController(DatabaseContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var invoices = _context.CustomerInvoices.Include(i => i.Items).ToList();
            return View(invoices);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CustomerInvoice());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CustomerInvoice invoice)
        {
            if (ModelState.IsValid)
            {
                _context.CustomerInvoices.Add(invoice);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
        }

        public IActionResult Edit(int id)
        {
            var invoice = _context.CustomerInvoices.Find(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CustomerInvoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(invoice);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
        }


        //public IActionResult GeneratePdf(int id)
        //{/*Include(i => i.Items).FirstOrDefaultAsync(i => i.InvoiceId == id);*/
        //    var invoice = _context.CustomerInvoices.Include(i => i.Items).FirstOrDefault(i => i.Id == id); 
        //    if (invoice == null)
        //    {
        //        return NotFound();
        //    }

        //    // Create a MemoryStream for the overall invoice
        //    using (var invoiceMemoryStream = new MemoryStream())
        //    {
        //        // Create a new document
        //        var document = new Document(PageSize.A4);

        //        // Create a PDF writer
        //        var pdfWriter = PdfWriter.GetInstance(document, invoiceMemoryStream);

        //        // Open the document for writing
        //        document.Open();

        //        // Add invoice details using iTextSharp methods
        //        AddInvoiceDetails(document, invoice);

        //        // Close the document
        //        document.Close();

        //        // Set the Content-Type and return the overall invoice PDF
        //        return File(invoiceMemoryStream.ToArray(), "application/pdf", $"invoice_{id}.pdf");
        //    }
        //}
            
        //private void AddInvoiceDetails(Document document, CustomerInvoice invoice)
        //{
        //    // Add customer information
        //    var customerParagraph = new Paragraph();
        //    customerParagraph.Add(new Chunk("Customer: " + invoice.CustomerName, FontFactory.GetFont("Arial", 12)));
        //    customerParagraph.Add(new Chunk("\nEmail: " + invoice.CustomerEmail, FontFactory.GetFont("Arial", 10)));
        //    customerParagraph.Add(new Chunk("\nAddress: " + invoice.CustomerAddress, FontFactory.GetFont("Arial", 10)));
        //    document.Add(customerParagraph);

        //    // Add invoice date
        //    var invoiceDateParagraph = new Paragraph();
        //    invoiceDateParagraph.Add(new Chunk("Invoice Date: " + invoice.InvoiceDate.ToShortDateString(), FontFactory.GetFont("Arial", 10)));
        //    document.Add(invoiceDateParagraph);

        //    // Add invoice items table
        //    var table = new PdfPTable(3);
        //    table.AddCell(new Phrase("Description", FontFactory.GetFont("Arial", 10, Font.BOLD)));
        //    table.AddCell(new Phrase("Price", FontFactory.GetFont("Arial", 10, Font.BOLD)));
        //    table.AddCell(new Phrase("Quantity", FontFactory.GetFont("Arial", 10, Font.BOLD)));
           

        //    if (invoice.Items != null)
        //    {
        //        foreach (var item in invoice.Items)
        //        {
        //            // Generate individual PDF for the item
                    

        //            // Add item details to the table
        //            table.AddCell(new Phrase(item.Description, FontFactory.GetFont("Arial", 10)));
        //            table.AddCell(new Phrase(item.Price.ToString("C2"), FontFactory.GetFont("Arial", 10)));
        //            table.AddCell(new Phrase(item.Quantity.ToString(), FontFactory.GetFont("Arial", 10)));
                
        //        }
        //    }
        //    else
        //    {
        //        table.AddCell(new Phrase("No items found for this invoice.", FontFactory.GetFont("Arial", 10, Font.ITALIC)));
        //    }

        //    document.Add(table);

        //    // Add total amount
        //    var totalAmountParagraph = new Paragraph();
        //    totalAmountParagraph.Add(new Chunk("Total Amount: " + invoice.TotalPrice.ToString("C2"), FontFactory.GetFont("Arial", 12, Font.BOLD)));
        //    document.Add(totalAmountParagraph);
        //}


 

public IActionResult GeneratePdf(int id)
    {
        var invoice = _context.CustomerInvoices.Include(i => i.Items).FirstOrDefault(i => i.Id == id);
        if (invoice == null)
        {
            return NotFound();
        }

        // Create a MemoryStream for the overall invoice
        using (var invoiceMemoryStream = new MemoryStream())
        {
            // Create a new PdfWriter with the MemoryStream
            using (var pdfWriter = new iText.Kernel.Pdf.PdfWriter(invoiceMemoryStream))
            {
                // Create a PdfDocument with the PdfWriter
                using (var pdf = new iText.Kernel.Pdf.PdfDocument(pdfWriter))
                {
                    // Create a Document with the PdfDocument
                    using (var document = new iText.Layout.Document(pdf))
                    {
                        // Add invoice details using iTextSharp methods
                        AddInvoiceDetails(document, invoice);
                    }
                }
            }

            // Set the Content-Type and return the overall invoice PDF
            return File(invoiceMemoryStream.ToArray(), "application/pdf", $"invoice_{id}.pdf");
        }
    }

    private void AddInvoiceDetails(iText.Layout.Document document, CustomerInvoice invoice)
    {
        // Add customer information
        var customerParagraph = new iText.Layout.Element.Paragraph()
            .Add("Customer: " + invoice.CustomerName)
            .Add("\nEmail: " + invoice.CustomerEmail)
            .Add("\nAddress: " + invoice.CustomerAddress);

        document.Add(customerParagraph);

        // Add invoice date
        var invoiceDateParagraph = new iText.Layout.Element.Paragraph().Add("Invoice Date: " + invoice.InvoiceDate.ToShortDateString());
        document.Add(invoiceDateParagraph);

        // Add invoice items table
        var table = new iText.Layout.Element.Table(3)
            .AddCell("Description").AddCell("Price").AddCell("Quantity");

        if (invoice.Items != null && invoice.Items.Any())
        {
            foreach (var item in invoice.Items)
            {
                // Add item details to the table
                table.AddCell(item.Description).AddCell(item.Price.ToString("C2")).AddCell(item.Quantity.ToString());
            }
        }
        else
        {
            table.AddCell("No items found for this invoice.").SetItalic();
        }

        document.Add(table);

        // Add total amount
        var totalAmountParagraph = new iText.Layout.Element.Paragraph().Add("Total Amount: " + invoice.TotalPrice.ToString("C2")).SetBold();
        document.Add(totalAmountParagraph);
    }


}
}
