using TesseractNet5.Api.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TesseractNet5
{
    public class Startup
    {
        private const string DefaultCorsPolicyName = "MyPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // enable cors 
            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName,
                    builder =>
                    {
                        builder.WithOrigins(Configuration["App:CorsOrigins"])
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });

            services.AddRazorPages();
            services.AddControllers();

            // add swagger
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<SwaggerDefaultValues>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();              
            }

            //show ui
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DisplayOperationId();               
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"Aspose Net 5");
            });

            app.UseRouting();

            app.UseCors(DefaultCorsPolicyName);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
