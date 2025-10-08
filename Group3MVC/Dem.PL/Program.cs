using Dem.BLL.Interfaces;
using Dem.BLL.Repositories;
using Dem.DAL.Context;
using Dem.DAL.Models;
using Dem.PL.Helpers;
using Dem.PL.MappingProfiles;
using Dem.PL.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dem.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<MVCProjectDbContext>(options =>
            //options.UseSqlServer("Server = . ; Database = G3MVCDb ; Trusted_Connextion = True")
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
            ));  // Register DbContext in the DI Container
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); //Allow Dependancy Injection Class DepartmentRepository
            //builder.Services.AddScoped<IEmployeeRepository ,EmployeeRepository>();//Allow Dependancy Injection Class EmployeeRepository
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();//Allow Dependancy Injection Class UnitOfWork
            builder.Services.AddAutoMapper(M =>M.AddProfile(new EmployeeProfile())); //Allow Dependancy Injection AutoMapper
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => 
            {
                //P@ssw0rd
                //Pa$$w0rd
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.AllowedForNewUsers = true;
            })
                .AddEntityFrameworkStores<MVCProjectDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddAuthentication(options => 
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
                .AddCookie(options => 
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Home/Error";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true;
                })
                .AddGoogle( options =>
                {
                    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                    options.Events.OnRedirectToAuthorizationEndpoint = context =>
                    {
                        context.Response.Redirect(context.RedirectUri + "&prompt=select_account");
                        return Task.CompletedTask;
                    };
                });; //Allow Dependancy Injection Authentication Service like UserManger ,SignInManger ,RoleManger ,etc

            // Initialize EmailSettings
            EmailSettings.Initialize(builder.Configuration);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
