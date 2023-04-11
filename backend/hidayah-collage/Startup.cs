using hidayah_collage.DataContext;
using hidayah_collage.Interface;
using hidayah_collage.Models;
using hidayah_collage.Models.Email;
using hidayah_collage.Models.Exceptions;
using hidayah_collage.Models.JWT;
using hidayah_collage.Models.Roles;
using hidayah_collage.Models.TokenGenerator;
using hidayah_collage.Models.TokenValidator;
using hidayah_collage.Repository;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System;
using System.IO;
using System.Text;

namespace hidayah_collage
{
    public class Startup
    {
        public static string ConnectionString { get; private set; }
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;

            ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        public IConfiguration Configuration { get; }
        private readonly string _policyName = "CorsPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(ConnectionString)
            );

            services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequiredLength = 5;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });

            JwtConfig jwtConfig = new JwtConfig();
            Configuration.Bind("JWT", jwtConfig);

            services.AddSingleton(jwtConfig);

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(option =>
                {
                    option.SaveToken = true;
                    option.RequireHttpsMetadata = false;
                    option.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        //ValidAudience = Configuration["JWT:ValidAudience"],
                        //ValidIssuer = Configuration["JWT:ValidIssuer"],
                        ValidAudience = jwtConfig.Audience,
                        ValidIssuer = jwtConfig.Issuer,
                        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"])),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.AccessTokenSecret)),
                        ValidateIssuerSigningKey = true
                    };
                    //option.Events = new JwtBearerEvents
                    //{
                    //    OnMessageReceived = context =>
                    //    {
                    //        context.Token = context.Request.Cookies["X-Access-Token"];
                    //        return Task.CompletedTask;
                    //    }
                    //};
                });
            /*
                AddTransient: A new instance of the service is created each time it's requested
                AddScoped: A new instance of the service is created for each HTTP request
                AddSingleton: A single instance of the service is created for the lifetime of the application
            */
            services.Configure<EmailConfig>(Configuration.GetSection("EmailConfiguration"));
            services.AddScoped<IAccount, AccountRepository>();
            services.AddScoped<IRole, RoleRepository>();
            services.AddScoped<GetMessageRepository>();
            services.AddScoped<SystemMasterRepository>();
            services.AddScoped<AccessTokenGenerator>();
            services.AddScoped<RefreshTokenGenerator>();
            services.AddScoped<RefreshTokenValidator>();            
            services.AddScoped<TokenGenerator>();
            services.AddTransient<IRefreshToken, RefreshTokenRepository>();
            services.AddTransient<IMessage, MessageRepository>();
            services.AddTransient<ISystemMaster, SystemMasterRepository>();
            services.AddTransient<IMailService, MailServiceRepository>();
            services.AddSingleton<ILoggerManager, LoggerManager>();
            //services.AddCors(option =>
            //{
            //    option.AddDefaultPolicy(builder =>
            //    {
            //        builder.AllowAnyOrigin().AllowAnyOrigin().AllowAnyMethod();
            //    });
            //});
            //var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            services.AddCors(option =>
            {
                option.AddPolicy(_policyName, builder =>
                {
                    builder
                    //.AllowAnyOrigin()
                    .WithOrigins(Configuration["AppClientUrl"])
                    .WithMethods("PUT", "DELETE", "POST", "GET")
                    .AllowAnyHeader()
                    //.AllowAnyOrigin()
                    .AllowCredentials()
                    .WithExposedHeaders("X-Access-Token");
                });
            });

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-Version"),
                    new MediaTypeApiVersionReader("ver")
                    );
            });

            services.AddControllers();
            services.AddRazorPages();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseMiddleware<GlobalExceptionErrorHandling>();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            // JWT
            app.UseCookiePolicy();
            app.UseSession();

            //app.Use(async (context, next) =>
            //{
            //    var JWToken = context.Session.GetString("JWToken");
            //    if (!string.IsNullOrEmpty(JWToken))
            //    {
            //        context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
            //    }
            //    await next();
            //});
            //

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(_policyName);

            //app.ConfigureExceptionHandler(logger);
            app.ConfigureExceptionHandler();

            RolesData.SeedRoles(app.ApplicationServices).Wait();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
