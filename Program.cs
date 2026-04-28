using ControleFinanceiroMvc.Data;
using ControleFinanceiroMvc.Repositories;
using ControleFinanceiroMvc.Services;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace ControleFinanceiroMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.Development.local.json", optional: true, reloadOnChange: true);
            var supportedCultures = new[] { new CultureInfo("pt-BR") };

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("pt-BR");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            builder.Services.AddSingleton<DbConnectionFactory>();
            builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            builder.Services.AddScoped<ICategoriaService, CategoriaService>();
            builder.Services.AddScoped<ILancamentoRepository, LancamentoRepository>();
            builder.Services.AddScoped<ILancamentoService, LancamentoService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseRequestLocalization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
