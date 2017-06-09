using SimpleABC.Api.Interface.ILogHistoryBll;
using SimpleABC.Api.Program.MiddleWares;
using FreshCommonUtility.CoreModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SimpleABC.Api.Business.LogHistoryBll;

namespace SimpleABC.Api.Program
{
    /// <summary>
    /// 启动程序
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// construct function
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        /// <summary>
        /// interface server
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime.Use this method to add services to the container.
        /// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc(opt =>
            {
                opt.UseControllRoutePrefix(new RouteAttribute("api/"));
            });
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //Add options
            services.AddOptions();
            services.Configure<AppSettingsModel>(Configuration.GetSection("AppSettings"));
            //Add Error log server
            services.AddTransient<IErrorLogBll, ErrorLogBll>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();
            //异常处理中间件
            app.UseMiddleware(typeof(ExceptionHandlerMiddleWare));
            app.UseMvc();
        }
    }
}
