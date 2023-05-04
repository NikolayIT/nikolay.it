namespace BlogSystem.Web
{
    using System;
    using System.Reflection;

    using BlogSystem.Common;
    using BlogSystem.Data;
    using BlogSystem.Data.Common;
    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Data.Repositories;
    using BlogSystem.Data.Seeding;
    using BlogSystem.Services;
    using BlogSystem.Services.Data;
    using BlogSystem.Services.Mapping;
    using BlogSystem.Services.Messaging;
    using BlogSystem.Services.YouTube;
    using BlogSystem.Web.ViewModels;
    using Hangfire;
    using Hangfire.Console;
    using Hangfire.Dashboard;
    using Hangfire.SqlServer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.configuration = configuration;
            this.env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //// services.AddHangfire(
            ////     config => config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            ////         .UseSimpleAssemblyNameTypeSerializer().UseRecommendedSerializerSettings().UseSqlServerStorage(
            ////             this.configuration.GetConnectionString("DefaultConnection"),
            ////             new SqlServerStorageOptions
            ////             {
            ////                 CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            ////                 SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            ////                 QueuePollInterval = TimeSpan.Zero,
            ////                 UseRecommendedIsolationLevel = true,
            ////                 UsePageLocksOnDequeue = true,
            ////                 DisableGlobalLocks = true,
            ////             }).UseConsole());
            //// if (this.env.IsProduction())
            //// {
            ////     services.AddHangfireServer(options => options.WorkerCount = 2);
            //// }

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IBlogUrlGenerator, BlogUrlGenerator>();
            services.AddTransient<IEmailSender>(_ => new SendGridEmailSender(this.configuration["SendGrid:ApiKey"]));
            services.AddTransient<ILatestVideosProvider, LatestVideosProvider>();
            services.AddTransient<IYouTubeChannelVideosFetcher>(
                s => new YouTubeChannelVideosFetcher(this.configuration["YouTube:ApiKey"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            this.SeedHangfireJobs(recurringJobManager);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            if (env.IsProduction())
            {
                //// app.UseHangfireDashboard(
                ////     "/hangfire",
                ////     new DashboardOptions { Authorization = new[] { new HangfireAuthorizationFilter() } });
            }

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute(
                            "index.html",
                            "index.html",
                            new { controller = "Home", action = "Index" });
                        endpoints.MapControllerRoute(
                            "Blog post",
                            "Blog/{year}/{month}/{title}/{id}",
                            new { controller = "Blog", action = "Post" });
                        endpoints.MapControllerRoute(
                            "Page",
                            "Pages/{permalink}",
                            new { controller = "Pages", action = "Page" });
                        endpoints.MapControllerRoute(
                            "areaRoute",
                            "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }

        private void SeedHangfireJobs(IRecurringJobManager recurringJobManager)
        {
        }

        private class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                var httpContext = context.GetHttpContext();
                return httpContext.User.IsInRole(GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
