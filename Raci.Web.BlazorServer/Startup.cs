using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Raci.Application.Account.Queries;
using Raci.Application.Models.Mapping;
using Raci.Application.Pipelines;
using Raci.Persistence;
using Raci.Persistence.AuditNet;
using Raci.Web.BlazorServer.Data;
using Raci.Application.Security.Queries;
using Raci.Service.JwtBlazor;
using Microsoft.AspNetCore.Authentication.Cookies;
using CurrieTechnologies.Razor.SweetAlert2;
using Radzen;
using Blazored.Toast;
using Raci.Common.Enums;
using System.Threading;
//using Syncfusion.Blazor;

namespace Raci.Web.BlazorServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

            services.AddHttpContextAccessor();

            services.AddSingleton(factory => Configuration.GetSection("JwtSetting").Get<JwtSetting>());
            services.AddSingleton<IAuthenticateService, AuthenticateService>();

            services.AddAutoMapper(typeof(AutoMapperProfile));

            //services.AddSyncfusionBlazor();

            RegisterDatabase(services, this.Configuration);

            RegisterMediator(services);

            RegisterBlazorComponents(services);

            RegisterLoggingUser(services);

            RegisterAuthenticationOptions(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzg0ODU3QDMxMzgyZTM0MmUzMEhFQ0drWkF4UEgxbDBBTVFRU3ZZSVF0V004a0JkRUZZRzhnQkJ3MHVaTzg9");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }

        void RegisterDatabase(IServiceCollection services, IConfiguration configuration)
        {
            AuditNetConfiguration.ConfigureWithSingleTable(
                configuration.GetConnectionString("RaciConnection") + "Application Name=RaciCms-AuditNet;");

            services.AddDbContext<RaciDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("RaciConnection") + "Application Name=RaciCms;",
                    x => x.MigrationsHistoryTable("__EFMigrationsHistory", "dbo")), ServiceLifetime.Transient);
        }

        void RegisterMediator(IServiceCollection services)
        {
            // Add MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IRequestPreProcessor<>), typeof(RequestValidationBehavior<>));
            services.AddMediatR(typeof(LoginRequest.Handler).GetTypeInfo().Assembly);
            var validators = AssemblyScanner.FindValidatorsInAssemblyContaining<LoginRequest.Validator>();
            validators.ForEach(validator => services.AddTransient(validator.InterfaceType, validator.ValidatorType));
        }

        protected virtual void RegisterLoggingUser(IServiceCollection services)
        {
            services.AddScoped<LoggingUser>(serviceProvider =>
            {
                var contextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                using var scope = serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                //var mediator = serviceProvider.GetRequiredService<IMediator>();
                //var dbContext = serviceProvider.GetRequiredService<RaciDbContext>();

                var loggingUser = new LoggingUser();

                var cookehaha = contextAccessor.HttpContext.Request.Cookies["RACIForm"];

                if (contextAccessor.HttpContext.Request.Cookies["RACIForm"] != null)
                {
                    var authenticateService = serviceProvider.GetRequiredService<IAuthenticateService>();
                    var claimsPrincipal = authenticateService.Validate(contextAccessor.HttpContext.Request.Cookies["RACIForm"].ToString());
                    if (claimsPrincipal != null)
                    {
                        loggingUser.UserName = claimsPrincipal.FindFirst(ClaimTypes.Name).Value;
                        loggingUser.UserId = Guid.Parse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value);
                        //loggingUser.LanguageCode = claimsPrincipal.FindFirst("language_code").Value;
                        loggingUser.Email = claimsPrincipal.FindFirst(ClaimTypes.Email).Value;
                    }
                }


                var userPermission = Task.Run(async () => await mediator.Send(new GetRoleBasedPermissionByAccountIdQuery() { AccountId = loggingUser.UserId }))
                    .ConfigureAwait(false).GetAwaiter().GetResult()
                    .Where(p => string.IsNullOrWhiteSpace(p.Function.Url) == false)
                    .ToList();

                loggingUser.UserPermissions = userPermission;

                var avatar = Task.Run(async () => await mediator.Send(new AccountAvatarGetByIdQuery() { AccountGuid = loggingUser.UserId }))
                    .ConfigureAwait(false).GetAwaiter().GetResult();

                loggingUser.Avatar = avatar;

                return loggingUser;
            });

            //services.AddScoped<ICurrentUserService, CurrentUserService>();
        }

        void RegisterAuthenticationOptions(IServiceCollection services)
        {
            services.AddSingleton<AuthenticateService>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Events.OnValidatePrincipal = async context =>
                    {

                    };
                });
        }

        void RegisterBlazorComponents(IServiceCollection services)
        {
            // https://github.com/Blazored/Toast
            services.AddBlazoredToast();

            // https://github.com/Basaingeal/Razor.SweetAlert2
            services.AddSweetAlert2(options =>
            {
                options.Theme = SweetAlertTheme.Default;
            });

            //Radzen Blazor
            services.AddScoped<ContextMenuService>();
        }
    }
}
