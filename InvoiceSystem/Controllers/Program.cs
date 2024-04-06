using Microsoft.EntityFrameworkCore;

using InvoiceSystem.Models;

namespace InvoiceSystem.Controllers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var provider = builder.Services.BuildServiceProvider();
            var service = provider.GetRequiredService<IConfiguration>();
            builder.Services.AddDbContext<DatabaseContext>(item => item.UseSqlServer(service.GetConnectionString("dataconn")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            //app.UseRouting();
            //app.MapControllerRoute(
            //    name: "invoiceRoute",
            //    pattern: "InvoiceController/Index",
            //    defaults: new { controller = "InvoiceController", action = "Index" });


            app.Run();
        }
    }
}