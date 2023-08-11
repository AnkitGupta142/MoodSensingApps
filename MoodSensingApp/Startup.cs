using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MoodSensingApp.CustomMiddleware;
using MoodSensingApp.DatabaseContext;
using MoodSensingApp.Models;
using MoodSensingApp.Repositories;
using MoodSensingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MoodSensingApp
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

            services.AddControllers();
            services.AddDbContext<AppDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };

            });
            // Register the UserRepository with DbContext injection
            services.AddScoped<IUserRepository>(provider =>
            {
                var dbContext = provider.GetService<AppDbContext>();
                return new UserRepository(dbContext);
            });
            services.AddScoped<IUserRepository>(provider =>
            {
                var dbContext = provider.GetService<AppDbContext>();
                return new UserRepository(dbContext);
            });
            services.AddScoped<ILocationRepository>(provider =>
            {
                var dbContext = provider.GetService<AppDbContext>();
                return new LocationRepository(dbContext);
            });
            services.AddScoped<IMoodRepository>(provider =>
            {
                var dbContext = provider.GetService<AppDbContext>();
                return new MoodRepository(dbContext);
            });
            services.AddControllers().AddJsonOptions(options =>
            {
               // options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
  
                options.JsonSerializerOptions.MaxDepth = 64; // or any value greater than 32
            });




            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMoodService, MoodService>();
            services.AddScoped<IFaceRecognitionService, GoogleFaceRecognitionService>();
            services.AddHttpContextAccessor();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mood Sensing API", Version = "v1" });
            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseMiddleware<ExceptionHandlingMiddleware>();
            // app.UseHttpsRedirection();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
            });
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mood Sensing API V1");
            });
        }
    }
}
