using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Collections.Generic;
using SalesWebMvc.Models;
using SalesWebMvc.Data;
using SalesWebMvc.Services;

namespace SalesWebMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<SalesWebMvcContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("SalesWebMvcContext"), builder =>
                        builder.MigrationsAssembly("SalesWebMvc")));
            //options.UseSqlServer(Configuration.GetConnectionString("SalesWebMvcContext")));

            // Registra o serviço(SeedingService usado para popular os dados do banco)no serviço de injeção de dependência da aplicação
            services.AddScoped<SeedingService>();

            // Registra o serviço(SellerService) no serviço de injeção de dependência da aplicação
            services.AddScoped<SellerService>();

            // Registra o serviço(DepartmentService) no serviço de injeção de dependência da aplicação
            services.AddScoped<DepartmentService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SeedingService seedingservice)
        {
            // Locale/localização en-us

            var enUSLoc = new CultureInfo("en-US"); // Localização da app em US
            var localizationOptions = new RequestLocalizationOptions // localizaçoes posiçoes
            {
                 DefaultRequestCulture = new RequestCulture("en-US"), // localização padrão
                  SupportedCultures = new List<CultureInfo> { enUSLoc }, // localizações possíveis 
                  SupportedUICultures = new List<CultureInfo> { enUSLoc } // 
            };

            // Define para a aplicação as localizações possíveis
            app.UseRequestLocalization(localizationOptions);

            if (env.IsDevelopment()) // Testa se é perfil de desenvolvimento
            {
                app.UseDeveloperExceptionPage();
                seedingservice.Seed(); // Se for perfil de desenvolvimento populará o banco
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
