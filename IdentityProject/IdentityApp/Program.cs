using IdentityApp.Entities;
using IdentityApp.Repo;
using IdentityApp.Repo.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            string DefaultConnectionString = builder.Configuration["ConnectionStrings:DefaultConnection"]!;
            builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlServer(DefaultConnectionString));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<IdentityContext>()
                            .AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(option => option.LoginPath = "/Authentication/Login");
            builder.Services.AddScoped<IUserAuthentication, UserAuthentication>();


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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}