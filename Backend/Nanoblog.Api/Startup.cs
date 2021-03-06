using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Nanoblog.Api.Data;
using Nanoblog.Api.Data.Models;
using Nanoblog.Api.Middleware;
using Nanoblog.Api.Services;
using Nanoblog.Api.Services.Karma;
using Nanoblog.Api.Settings;
using Nanoblog.Common.Dto;
using System;
using System.Text;

namespace Nanoblog
{
    public class Startup
    {
        public Startup(IConfiguration env)
        {
            Configuration = env;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseMySql(Configuration.GetConnectionString("MySQL")));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddScoped<IEntryService, EntryService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IEntryKarmaService, EntryKarmaService>();
            services.AddScoped<ICommentKarmaService, CommentKarmaService>();

            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddSingleton<IJwtHandler, JwtHandler>();

            services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));

            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<User, AuthorDto>();
                cfg.CreateMap<Entry, EntryDto>();
                cfg.CreateMap<Comment, CommentDto>();

                cfg.CreateMap<KarmaBase, KarmaDto>();
                cfg.CreateMap<EntryKarma, KarmaDto>();
                cfg.CreateMap<CommentKarma, KarmaDto>();
            },
            AppDomain.CurrentDomain.GetAssemblies());

            var jwtSettings = Configuration.GetSection("Jwt").Get<JwtSettings>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
 

            services.AddControllers();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
             if (env.IsProduction())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseMiddleware<ErrorHandler>();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:8080").AllowAnyMethod().AllowAnyHeader();
            });
        }
    }
}
