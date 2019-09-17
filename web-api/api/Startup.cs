using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Config;
using api.Repositories;
using api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string CorsDefaultPolicy = "_corsDefaultPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(
                options =>
                {
                    options.AddPolicy(
                        CorsDefaultPolicy,
                        builder => { builder.WithOrigins("*").WithMethods("*").WithHeaders("*"); }
                    );
                }
            );
            services.Configure<TimeTrackConfig>(Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();

            var sp = services.BuildServiceProvider();

            var us = sp.GetService<IUserService>();
            us.EnsureAdminUser("admin@onlinetrack.com");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(CorsDefaultPolicy);
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
