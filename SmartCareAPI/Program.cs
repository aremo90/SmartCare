
using Microsoft.EntityFrameworkCore;
using SmartCareBLL.Mapping;
using SmartCareBLL.Services.Classes;
using SmartCareBLL.Services.Interfaces;
using SmartCareDAL.Data.Context;
using SmartCareDAL.Repositories.Classes;
using SmartCareDAL.Repositories.Interface;

namespace SmartCareAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SmartCareDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IMedicineReminderRepository, MedicineReminderRepository>();
            builder.Services.AddScoped<IUserService, UserService>();


            builder.Services.AddAutoMapper(X => X.AddProfile(new AutoMapperProfile()));
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
