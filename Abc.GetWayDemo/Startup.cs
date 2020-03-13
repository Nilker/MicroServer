using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

using System;
using System.Collections.Generic;
using System.Linq;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

namespace Abc.GetWayDemo
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
            services.AddOcelot()
                .AddConsul()
                .AddPolly();
            //services.AddControllers();

            Action<IdentityServerAuthenticationOptions> isaOptClient = option =>
            {
                option.Authority = Configuration["IdentityService:Uri"];
                option.ApiName = "clientservice";
                option.RequireHttpsMetadata = Convert.ToBoolean(Configuration["IdentityService:UseHttps"]);
                option.SupportedTokens = SupportedTokens.Both;
                option.ApiSecret = Configuration["IdentityService:ApiSecrets:clientservice"];
            };

            services.AddAuthentication()
                .AddIdentityServerAuthentication("AbcServiceKey", isaOptClient);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //去掉默认的管道，用ocelot 管道处理请求
            app.UseOcelot();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.UseHttpsRedirection();

            //app.UseRouting();

            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
