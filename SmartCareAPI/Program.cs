using LinkO.Domin.Contract;
using LinkO.Domin.Models.IdentityModule;
using LinkO.Persistence.Data.Context;
using LinkO.Persistence.IdentityData.DbContext;
using LinkO.Persistence.Repository;
using LinkO.ServiceAbstraction;
using LinkO.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
            // ---------------------------
            // 01 Database Context
            // ---------------------------
            builder.Services.AddDbContext<SmartCareDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ---------------------------
            // 02 Repositories & UnitOfWork
            // ---------------------------
            builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            //builder.Services.AddScoped(IGenericRepository, GenericRepository);
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserService , UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAddressService, AddressService>();
            builder.Services.AddScoped<IDeviceService, DeviceService>();
            builder.Services.AddScoped<IMedicineService, MedicineReminderService>();
            builder.Services.AddScoped<IGpsService, GpsService>();
            builder.Services.AddAutoMapper(X => X.AddProfile<AutoMapperProfile>());
            builder.Services.AddHostedService<ReminderUpdateHostedService>();
            builder.Services.AddScoped<AuthService>(); // <<<<< JWT / Auth service

            builder.Services.AddDbContext<LinkOIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
                // add-migration "IdentityTablesCreate" -OutputDir "IdentityData/Migrations" -Context "LinkOIdentityDbContext"
            });
            builder.Services.AddIdentity<ApplicationUser, Microsoft.AspNetCore.Identity.IdentityRole>()
                .AddEntityFrameworkStores<LinkOIdentityDbContext>();
            // ---------------------------
            // 03 JWT Authentication
            // ---------------------------
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



            // ---------------------------
            // 04 Controllers & OpenAPI
            // ---------------------------

            // ---------------------------
            // 05 Build app
            // ---------------------------
            var app = builder.Build();



            app.UseHttpsRedirection();

            // ---------------------------
            // 06 Authentication & Authorization
            // ---------------------------
            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();


            // ---------------------------
            // 07 Map Controllers
            // ---------------------------
            app.MapControllers();

            app.Run();
        }
    }
}

    