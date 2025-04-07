using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Projeto_Unifeob.Data;

internal class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<BancoContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

        // Adicionar serviços ao container
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configurar o pipeline de requisições HTTP
        if (!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

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