using Functions.Util.Data;
using Padrao.APi.Configuration;
using Padrao.APi.Filters;
using Padrao.APi.Formatter;
using Padrao.Domain.Interfaces;
using Padrao.Domain.Virtual;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using Padrao.Service.Interface;
using Padrao.Service.Services;

namespace Padrao.APi
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
            services.AddCors();
            //compressao json 
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" });
            });
            services.AddControllers(options => {
                options.InputFormatters.Insert(0, new TextPlainInputFormatter());
                options.ReturnHttpNotAcceptable = true;
                options.Filters.Add(new ExceptionFilter());
                options.Filters.Add(new ProducesAttribute("application/json"));
                options.Filters.Add(typeof(ValidateModelStateFilterAttribute));
            }).AddJsonOptions( options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

            //Desabilitar Model invalido Automatico -- classe efetua a validação ValidateModelStateAttribute
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //config para usar funcçoes do identity 
            services.AddIdentityConfiguration(Configuration);

            var appTokenSection = Configuration.GetSection("AppToken");
            services.Configure<AppToken>(appTokenSection);

            services.Configure<RequestLocalizationOptions>(options => 
            {
                options.DefaultRequestCulture = new RequestCulture("pt-BR");
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo("pt-BR") };
            });

            services.AddScoped<IResponse, Response>();
            services.AddScoped<DataContext,DataContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, UserAuthenticated>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddSwaggerConfiguration();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
           
            app.UseResponseCompression();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Padrao"));
            var cultureInfo = new CultureInfo("pt-Br");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}