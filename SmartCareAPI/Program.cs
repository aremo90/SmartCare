using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SmartCareBLL.Mapping;
using SmartCareBLL.Services.Classes;
using SmartCareBLL.Services.Interfaces;
using SmartCareDAL.Data.Context;
using SmartCareDAL.Repositories.Classes;
using SmartCareDAL.Repositories.Interface;
using System.Text;

namespace SmartCareAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ---------------------------
            // 1️⃣ Database Context
            // ---------------------------
            builder.Services.AddDbContext<SmartCareDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ---------------------------
            // 2️⃣ Repositories & UnitOfWork
            // ---------------------------
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserService , UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAddressService, AddressService>();
            builder.Services.AddScoped<IDeviceService, DeviceService>();
            builder.Services.AddScoped<IMedicineService, MedicineReminderService>();
            builder.Services.AddScoped<IGpsService, GpsService>();
            builder.Services.AddAutoMapper(X => X.AddProfile<AutoMapperProfile>());


            // ---------------------------
            // 3️⃣ Services
            // ---------------------------
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<AuthService>(); // <<<<< JWT / Auth service

            // ---------------------------
            // 4️⃣ AutoMapper
            // ---------------------------
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new AutoMapperProfile()));

            // ---------------------------
            // 5️⃣ JWT Authentication
            // ---------------------------
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy
                        .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();

            builder.Services.AddScoped<IAuthService, AuthService>();


            // ---------------------------
            // 6️⃣ Controllers & OpenAPI
            // ---------------------------
            builder.Services.AddOpenApi();

            // ---------------------------
            // 7️⃣ Build app
            // ---------------------------
            var app = builder.Build();

            app.MapOpenApi();


            app.UseHttpsRedirection();

            // ---------------------------
            // 8️⃣ Authentication & Authorization
            // ---------------------------
            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();


            // ---------------------------
            // 9️⃣ Map Controllers
            // ---------------------------
            app.MapControllers();

            app.Run();
        }
    }
}

    