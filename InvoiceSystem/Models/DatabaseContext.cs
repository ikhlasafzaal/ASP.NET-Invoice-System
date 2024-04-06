using InvoiceSystem.Controllers;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSystem.Models

{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }


        public DbSet<CustomerInvoice> CustomerInvoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }


    }
}
