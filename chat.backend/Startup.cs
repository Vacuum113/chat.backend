using chat.backend.Auth.JWT;
using chat.backend.Data;
using chat.backend.Data.IdentityUserAsp;
using chat.backend.Helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

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
            var jwtConfig = Configuration.GetSection(nameof(JwtConfig));

            var signingKey = new SigningSymmetricKey(jwtConfig[nameof(JwtConfig.SecretKey)]);

            services.AddSingleton<IJwtSigningEncodingKey>(signingKey);

            services.AddCors();
            services.AddMvc(options => options.EnableEndpointRouting = false);


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = jwtConfig[nameof(JwtConfig.AuthenticationScheme)];
                options.DefaultChallengeScheme = jwtConfig[nameof(JwtConfig.AuthenticationScheme)];
            })
                .AddJwtBearerOptions(nameof(JwtConfig.AuthenticationScheme), (IJwtSigningDecodingKey)signingKey);
            

            services.AddDbContext<ApplicationDbContex>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ConnectionString"), b => b.MigrationsAssembly("chat.backend.Data")));


            services.AddDefaultIdentity<IdentUser>(o => o.AddIdentityOptions())
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
