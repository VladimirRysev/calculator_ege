using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DataAccessLayer;
using Calculator_ege_bu.Services;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Calculator_ege_bu
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
                Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options=>
                options.UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddRoles<IdentityRole<int>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);

                options.LoginPath = "/Calculator/Login";
                options.SlidingExpiration = true;
            });
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ExcelService>();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dbContex)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else
            {
                app.UseExceptionHandler("/Calculator/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                 endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Calculator}/{action=Index}/{id?}");
            });

            dbContex.Database.Migrate();
            ConfigureAdmin(app.ApplicationServices).Wait();
        }

        private async Task ConfigureAdmin(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    await dbContext.Database.MigrateAsync();
                }

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                var superAdminRole = await roleManager.FindByNameAsync(RoleEnum.SuperAdministrator.ToString());
                if (superAdminRole == null)
                {
                    var roleResult =
                        await roleManager.CreateAsync(new IdentityRole<int>(RoleEnum.SuperAdministrator.ToString()));
                    if (!roleResult.Succeeded)
                    {
                        throw new InvalidOperationException($"Unable to create SuperAdministrator role");
                    }
                    var adminRole = await roleManager.FindByNameAsync(RoleEnum.Administrator.ToString());
                    if (adminRole == null)
                    {
                        roleResult =
                            await roleManager.CreateAsync(new IdentityRole<int>(RoleEnum.Administrator.ToString()));
                        if (!roleResult.Succeeded)
                        {
                            throw new InvalidOperationException($"Unable to create Administrator role");
                        }

                    }
                    superAdminRole = await roleManager.FindByNameAsync(RoleEnum.SuperAdministrator.ToString());
                }

                var adminUser = await userManager.FindByIdAsync("1");
                if (adminUser == null)
                {
                    var currentTime = DateTime.Now;
                    var userResult = await userManager.CreateAsync(new ApplicationUser
                    {
                        UserName = "admin",
                    }, "9cfY3ivCyKv6Qu7"); 
                    if (!userResult.Succeeded)
                    {
                        throw new InvalidOperationException($"Unable to create admin user");
                    }

                    adminUser = await userManager.FindByNameAsync("admin");
                }

                if (!await userManager.IsInRoleAsync(adminUser, superAdminRole.Name))
                {
                    await userManager.AddToRoleAsync(adminUser, superAdminRole.Name);
                }
            }
        }
    }
}
