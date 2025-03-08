using Autofac;
using Autofac.Extensions.DependencyInjection;
using CommissionMe.API.Infrastructure.Database;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

        // Add services to the container.

        builder.Services.AddControllersWithViews();

        builder.Services.AddSingleton<CreatedUpdatedInterceptor>();
        builder.Services.AddDbContext<PersonManagerContext>(
            (services, options) =>
                options
                    .UseInMemoryDatabase("PersonManager")
                    .AddInterceptors(services.GetRequiredService<CreatedUpdatedInterceptor>())
        );
        builder.Services.Configure<ApiConfiguration>(
            builder.Configuration.GetSection(ApiConfiguration.Section)
        );

        builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
        {
            var execAssembly = typeof(Program).Assembly;
            var assemblies = execAssembly
                .GetReferencedAssemblies()
                .Where(a => a.Name?.StartsWith("UKParliament.CodeTest") ?? false)
                .Select(Assembly.Load)
                .ToList();

            assemblies.Add(execAssembly);

            containerBuilder
                .RegisterAssemblyTypes([.. assemblies])
                .Where(t => t.GetConstructors().Any(c => c.IsPublic))
                .AsImplementedInterfaces();
        });

        var app = builder.Build();

        // Create database so the data seeds
        using (
            var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope()
        )
        {
            using var context =
                serviceScope.ServiceProvider.GetRequiredService<PersonManagerContext>();
            context.Database.EnsureCreated();
            context.SeedDatabase();
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}
