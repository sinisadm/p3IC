//using Dropbox.Infrastructure;
//using MediatR;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http.Features;
//using Microsoft.AspNetCore.HttpsPolicy;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Microsoft.OpenApi.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Threading.Tasks;

using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;

using System;
using System.Linq;
using System.Reflection;
using Dropbox.API.Filters;
using Dropbox.Application;
using Dropbox.Application.Common;
using Dropbox.Application.Common.Interfaces;
using Dropbox.Infrastructure;
using Microsoft.OpenApi.Models;

namespace Dropbox.API
{
    public class Startup
    {
        private readonly IWebHostEnvironment _currentEnvironment;
        private readonly IConfigurationRoot _configuration;

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();

            _configuration = builder.Build();
            _currentEnvironment = env;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var multipartBodyLengthLimit = _configuration.GetValue<long>("MultipartBodyLengthLimit");

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = multipartBodyLengthLimit;
            });

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = (int)multipartBodyLengthLimit;
                x.MultipartBodyLengthLimit = multipartBodyLengthLimit;
                x.MultipartHeadersLengthLimit = (int)multipartBodyLengthLimit;
            });

            services.AddOptions();
            services.AddHttpClient(); 
            services.AddHealthChecks();

            services.AddCors();

            services.AddApplication(_configuration);
            services.AddInfrastructure(_configuration, _currentEnvironment.ContentRootPath);
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dropbox.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dropbox.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(c => c
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .WithExposedHeaders(HeaderNames.ContentDisposition));


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
