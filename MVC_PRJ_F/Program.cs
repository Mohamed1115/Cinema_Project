using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MVC_PRJ_F.Data;
using MVC_PRJ_F.Utilites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MVC_PRJ_F.IRepositories;
using MVC_PRJ_F.Models;
using MVC_PRJ_F.Repositories;

namespace MVC_PRJ_F;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Identity
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // Email Sender
        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
        builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, EmailSender>();
        builder.Services.AddTransient<MVC_PRJ_F.IRepositories.IEmailSender, EmailSender>();
        
        builder.Services.AddScoped<IRepository<Otp>, Repository<Otp>>();
        builder.Services.AddScoped<IRepository<Actor>, Repository<Actor>>();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();     // important
        app.UseRouting();

        app.UseAuthentication();  // MUST be before authorization
        app.UseAuthorization();

        // Routing
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}