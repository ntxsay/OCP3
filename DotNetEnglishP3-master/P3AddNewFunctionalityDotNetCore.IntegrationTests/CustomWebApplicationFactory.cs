using System.Data.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using P3AddNewFunctionalityDotNetCore.Data;

namespace P3AddNewFunctionalityDotNetCore.IntegrationTests;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Charger la configuration depuis appsettings.json
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var environment = context.HostingEnvironment;

            // Ajouter appsettings.json et appsettings.{Environment}.json
            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

            // Ajouter des variables d'environnement (facultatif)
            config.AddEnvironmentVariables();
        });
        
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<P3Referential>));

            services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbConnection));

            services.Remove(dbConnectionDescriptor);

            // Create open SqliteConnection so EF won't automatically close it.
            services.AddSingleton<DbConnection>(container =>
            {
                var configuration = container.GetRequiredService<IConfiguration>();
                var connection = new SqlConnection(configuration.GetConnectionString("P3Referential"));
                connection.Open();

                return connection;
            });

            services.AddDbContext<P3Referential>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlServer(connection);
            });
        });

        builder.UseEnvironment("Development");
    }
}