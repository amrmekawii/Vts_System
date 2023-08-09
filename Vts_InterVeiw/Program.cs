using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Security.Claims;
using Vts.BL;
using Vts.DAL;

namespace Vts_InterVeiw
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            object value = builder.Services.AddDbContext<VtsContext>(options =>
    options.UseSqlServer());


            builder.Services.AddScoped<IUserRepo, UserRepo>();
          //  builder.Services.AddScoped<ICatRepo, CatRepo>();
          //  builder.Services.AddScoped<ICategoryManger, CategoryManger>();


            #region Identity

            // Mainly specify the context and the type of the user that the UserManger will use
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;


                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<VtsContext>()
                                .AddDefaultUI().AddDefaultTokenProviders();


            #endregion

            #region Authroization

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ForUserOnly", policy =>
                {
                    policy.RequireClaim("Role", "User")
                        .RequireClaim(ClaimTypes.NameIdentifier);
                    //.RequireAssertion(conte)
                });

                options.AddPolicy("ForAdminsOnly", policy =>
                {
                    policy.RequireClaim("Role", "Admin")
                        .RequireClaim(ClaimTypes.NameIdentifier);
                    //.RequireAssertion(conte)
                });
            });

            #endregion

            builder.Services.ConfigureApplicationCookie(o =>
            {
                o.LoginPath = "/User/Login";
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "CookieTest";
                options.ExpireTimeSpan = TimeSpan.FromHours(3);
                options.SlidingExpiration = true;
                options.LoginPath = "/User/Login";
                options.LogoutPath = "/User/Logout";
            });


            builder.Services.AddLocalization(options => options.ResourcesPath = "Recources");
          
            builder.Services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
        new CultureInfo("en-US"),
        new CultureInfo("ar-EG"),


    };
                options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

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
            var locOptions = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Items}/{action=Index}/{id?}");

            app.Run();
        }
    }
}