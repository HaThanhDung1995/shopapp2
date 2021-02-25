using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using ShopApplication.Data.EF;
using ShopApplication.Data.Entities;
using ShopApplication.Infrastructure.Interfaces;
using System;
using Microsoft.AspNetCore.Authorization;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using ShopApplication.Application.Implementations;
using ShopApplication.Application.Interfaces;
using ShopApplication.Authorization;
using ShopApplication.Helpers;
using ShopApplication.Services;

namespace ShopApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            #region Khai AppDbContext của dự án 
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    o => o.MigrationsAssembly("ShopApplication.Data.EF")
                ));
            #endregion



            #region Congigure Identity Password
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = true;
            });
            #endregion End Congigure Identity Password

            #region Recapcha
            services.AddRecaptcha(new RecaptchaOptions()
            {
                SiteKey = Configuration["Recaptcha:SiteKey"],
                SecretKey = Configuration["Recaptcha:SecretKey"]
            });


            #endregion

            #region Nếu muốn thêm seed data với AppUser và AppRole thì phải khai báo trong Service
            services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();
            #endregion

            #region Cấu hình Auto Mapper
            services.AddAutoMapper();
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp =>
                new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));
            #endregion

            #region Khai Báo Email Extension
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IViewRenderService, ViewRenderService>();

            #endregion

            #region khởi tạo dữ liệu đầu tiên nếu database bị rỗng

            services.AddTransient<DbInitializer>();

            #endregion

            #region Muốn sử dụng CustomClaimPrincipalFactory trong Extensions thì phải khai báo trong start up

            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, CustomClaimsPrincipalFactory>();

            #endregion

            #region Dependency Injection 

            services.AddTransient(typeof(IUnitOfWork), typeof(EFUnitOfWork));
            services.AddTransient(typeof(IRepository<,>), typeof(EFRepository<,>));

            #endregion

            #region Session

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
            });

            #endregion

            #region Service
            services.AddTransient<IProductCategoryService, ProductCategoryService>();
            services.AddTransient<IFunctionService, FunctionService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IBillService, BillService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<ICommonService, CommonService>();
            services.AddTransient<IFeedbackService, FeedbackService>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<IPageService, PageService>();
            //services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IAnnouncementService, AnnouncementService>();


            #endregion

            #region Authorization

            services.AddTransient<IAuthorizationHandler, BaseResourceAuthorizationHandler>();
            

            #endregion
            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/tedu-{Date}.txt");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(name: "areaRoute",
                    template: "{area:exists}/{controller=Login}/{action=Index}/{id?}");
            });
           
        }
    }
}