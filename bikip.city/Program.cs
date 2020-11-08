using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityProject.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace CityProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // To install logger in bootstrap period
            Logger logger = NLogBuilder
                .ConfigureNLog("nlog.config")
                .GetCurrentClassLogger();
            try
            {
                logger.Info("Init application - Khởi tạo chương trình");
                CreateHostBuilder(args).Build().Run();

                #region DB Migration by hard code (every time start application)
                // IHost host = CreateHostBuilder(args).Build();
                //
                // using (IServiceScope scope = host.Services.CreateScope())
                // {
                //     try
                //     {
                //         CityDbContext context = scope.ServiceProvider.GetService<CityDbContext>();
                //
                //         // For development, not for Production
                //         context.Database.EnsureDeleted();
                //         context.Database.Migrate();
                //     }
                //     catch (Exception e)
                //     {
                //         logger.Error(e, "Error while migrating the database");
                //     }
                // }
                //
                // host.Run();
                #endregion                
               
            }
            catch (Exception e)
            {
                logger.Error(e, "Application stopped because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseNLog();
                });
    }
}
