using LinkO.Domin.Contract;
using LinkO.Domin.Models.IdentityModule;
using LinkO.Persistence.DataSeed;
using LinkO.Persistence.IdentityData.DbContext;
using LinkO.Persistence.Repository;
using LinkO.Service;
using LinkO.ServiceAbstraction;
using LinkO.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartCareAPI.CustomMiddleWares;
using SmartCareAPI.Extensions;
using SmartCareAPI.Factories;
using SmartCareAPI.Hosted;
using SmartCareBLL.Mapping;
using System.Text;

namespace SmartCareAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            #region Registers

            builder.Services.AddDbContext<LinkOIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
                // add-migration "IdentityTablesCreate" -OutputDir "IdentityData/Migrations" -Context "LinkOIdentityDbContext"
            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<LinkOIdentityDbContext>();
            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Auth
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // UnAuth
            }).AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = builder.Configuration["JWTOptions:Issuer"],
                    ValidAudience = builder.Configuration["JWTOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:secretKey"]!))
                };
            });

            builder.Services.AddAutoMapper(X => X.AddProfile<AutoMapperProfile>());
            builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAddressService, AddressService>();
            builder.Services.AddScoped<IDeviceService, DeviceService>();
            builder.Services.AddScoped<IMedicineService, MedicineReminderService>();
            builder.Services.AddScoped<IGpsService, GpsService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse;
            });
            builder.Services.AddHostedService<ReminderUpdateHostedService>();
            builder.Services.AddScoped<IDataInitilizer, DataInitilizer>();
            builder.Services.AddAuthorization();
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

            #endregion

            var app = builder.Build();

            #region DataSeed


            await app.MigrateDbAsync();
            await app.SeedDataAsync();

            #endregion


            #region Exceptions

            app.UseMiddleware<ExceptionHandlerMiddleWare>();

            #endregion

            app.UseHttpsRedirection();

            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

    