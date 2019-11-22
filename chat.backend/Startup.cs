using chat.backend.Auth.JWT;
using chat.backend.Data.IdentityUserAsp;
using chat.backend.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;

namespace chat.backend
{
    public class Startup
    { 
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc(options => options.EnableEndpointRouting = false);

            var jwtConfig = Configuration.GetSection(nameof(JwtConfig));

            var signingKey = new SigningSymmetricKey(jwtConfig[nameof(JwtConfig.SecretKey)]);

            services.AddSingleton<IJwtSigningEncodingKey>(signingKey);

            var signinDecodKey = (IJwtSigningDecodingKey)signingKey;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = jwtConfig[nameof(JwtConfig.AuthenticationScheme)];
                options.DefaultChallengeScheme = jwtConfig[nameof(JwtConfig.AuthenticationScheme)];
            }).AddJwtBearer(
                jwtConfig[nameof(JwtConfig.AuthenticationScheme)],
                JwtBearerOptions =>
                {
                    JwtBearerOptions.SaveToken = true;
                    JwtBearerOptions.RequireHttpsMetadata = true;
                    JwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signinDecodKey.GetKey(),

                        ValidateIssuer = true,
                        ValidIssuer = "ChatWebApi",

                        ValidateAudience = true,
                        ValidAudience = "Web",

                        RequireExpirationTime = true,
                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.FromSeconds(5)
                    };
                });

            services.AddDbContext<ApplicationDbContex>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));

            services.AddDefaultIdentity<IdentUser>(o => 
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
                o.Password.RequiredLength = 8;
                o.Password.RequireNonAlphanumeric = false;
                o.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContex>()
                .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy =>
                    policy.RequireClaim(Constants.JwtClaimIdentifiers.Rol, Constants.JwtClaims.User));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
                .AllowCredentials()
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            //  .WithOrigins("https://localhost:3000")  путь к нашему SPA клиенту
                );

            app.UseStatusCodePages();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc();
        }
    }
}
