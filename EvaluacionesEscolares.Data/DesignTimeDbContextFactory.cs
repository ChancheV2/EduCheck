using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace EvaluacionesEscolares.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Obtener la ruta del directorio del proyecto
            var basePath = Directory.GetCurrentDirectory();

            // Si estamos en el proyecto Data pero el appsettings.json está en el proyecto API o Web
            // podemos ajustar la ruta para encontrarlo
            var parentDir = Directory.GetParent(basePath)?.FullName;
            var apiDir = Path.Combine(parentDir, "EvaluacionesEscolares.API");
            var webDir = Path.Combine(parentDir, "EvaluacionesEscolares.Web");

            string configPath;

            if (Directory.Exists(apiDir) && File.Exists(Path.Combine(apiDir, "appsettings.json")))
            {
                configPath = apiDir;
            }
            else if (Directory.Exists(webDir) && File.Exists(Path.Combine(webDir, "appsettings.json")))
            {
                configPath = webDir;
            }
            else
            {
                configPath = basePath;
            }

            // Construir la configuración
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(configPath)
                .AddJsonFile("appsettings.json")
                .Build();

            // Crear las opciones del DbContext
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new ApplicationDbContext(builder.Options);
        }
    }
}