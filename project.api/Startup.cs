using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using project.api.Core.Interfaces;
using project.api.Core.Services;
using project.api.Core.Utilities;
using project.api.Data;
using project.api.Data.Context;
using project.api.Data.DTO;
using project.api.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.api
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
        public Startup(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        //public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new projectProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDBContext>()
                .AddDefaultTokenProviders();
                
            services.AddSingleton(mapper);
            services.AddDbContext<AppDBContext>();
            services.AddTransient<IJobServices, JobServices>();
            //services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<ISkillServices, SkillServices>();
            services.AddTransient<IApplicationServices, ApplicationServices>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ITokenManager, JwtGenerator>();
            services.AddTransient<DatabaseSeeder, DatabaseSeeder>();
            //services.AddSwaggerDocument(settings =>
            //{
            //    settings.Title = "Jobs Agency";
            //});

            services.AddOpenApiDocument(document =>
            {

                document.Title = "Jobs Agency";
                document.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });

                document.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });


            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new projectProfile());
            });
            var secret = Configuration["JWT:JWT_SECRET"];
            var issuer = Configuration["JWT:VALIDISSUER"];
            var audience = Configuration["JWT:VALIDAUDIENCE"];

            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // To immediately reject the access token
                };
            });
            //}).AddJwtBearer(opts =>
            //{
            //    opts.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        //ValidateIssuerSigningKey = true,
            //        ValidIssuer = issuer,
            //        ValidAudience = audience,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret))
            //    };
            //});

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

            app.UseOpenApi();

            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
