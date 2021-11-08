using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleRESTful.Models;
using Microsoft.Net.Http.Headers;

namespace ConsoleRESTful
{
    public class Startup
    {
        public IConfiguration AppConfiguration { get; set; }
        public Startup(IConfiguration configuration)
        {
            AppConfiguration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
            .AddXmlDataContractSerializerFormatters()
            .AddMvcOptions(options => 
                options.FormatterMappings.SetMediaTypeMappingForFormat("xml", new MediaTypeHeaderValue("application/xml")));

            string defaultConnection = AppConfiguration.GetSection("ConnectionStrings")["DefaultConnection"];
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(defaultConnection));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => 
            {
                endpoints.MapControllers();
            });
        }
    }
}
