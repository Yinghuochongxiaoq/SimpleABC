using SimpleABC.Api.Interface.ILogHistoryBll;
using SimpleABC.Api.Program.MiddleWares;
using FreshCommonUtility.CoreModel;
using FreshCommonUtility.Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SimpleABC.Api.Business.LogHistoryBll;
using SimpleABC.Api.Interface.IUploadFileBll;
using SimpleABC.Api.Business.UploadFileBll;
using Swashbuckle.Swagger.Model;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

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
            //set db access type
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
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
            //Add file upload server
            services.AddTransient<IUploadFileStorageBll, UploadFileStorageBll>();
            services.AddSwaggerGen();
            //Add the detail information for the API.http://localhost:port/swagger/ui/index.html
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "SimpleABC.Api",
                    Description = "SimpleABC.Api doc",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "FreshMan", Email = "qinbocai@sina.cn", Url = "https://github.com/Yinghuochongxiaoq" },
                    //License = new License { Name = "Use under LICX", Url = "http://url.com" }
                });

                //Determine base path for the application.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                System.Console.WriteLine(basePath);
                //Set the comments path for the swagger json and ui.
                var separatorChar = Path.DirectorySeparatorChar;
                options.IncludeXmlComments(basePath + $@"{separatorChar}SimpleABC.Api.Program.xml");
                options.IncludeXmlComments(basePath + $@"{separatorChar}SimpleABC.Api.Business.xml");
                options.IncludeXmlComments(basePath + $@"{separatorChar}SimpleABC.Api.DataAccess.xml");
                options.IncludeXmlComments(basePath + $@"{separatorChar}SimpleABC.Api.Interface.xml");
                options.IncludeXmlComments(basePath + $@"{separatorChar}SimpleABC.Api.Model.xml");
                options.DescribeAllEnumsAsStrings();
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();
            //异常处理中间件
            app.UseMiddleware(typeof(ExceptionHandlerMiddleWare));
            app.UseSwagger();
            app.UseSwaggerUi("doc/api");
            app.UseMvc();
            app.UseStaticFiles();
        }
    }
}
