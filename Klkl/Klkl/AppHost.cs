using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using Domain;
using Funq;
using Klkl.ServiceInterface;
using ServiceStack.Razor;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;
using ServiceStack.Redis;
using ServiceStack.Text;


namespace Klkl
{
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Default constructor.
        /// Base constructor requires a name and assembly to locate web service classes. 
        /// </summary>
        public AppHost()
            : base("Klkl", typeof(MyServices).Assembly)
        {

        }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        /// <param name="container"></param>
        public override void Configure(Container container)
        {
            //Config examples
            //this.Plugins.Add(new PostmanFeature());
            //this.Plugins.Add(new CorsFeature());

            this.Plugins.Add(new RazorFormat());
            Plugins.Add(new AutoQueryFeature {MaxLimit = 10000});
            Plugins.Add(new SwaggerFeature());
            //   Feature disableFeatures = Feature.Json| Feature.Html;
            SetConfig(new HostConfig()
            {
                AllowFileExtensions = {"json"},
                DefaultContentType = MimeTypes.Json,
                GlobalResponseHeaders =
                {
                    {"Access-Control-Allow-Origin", "*"},
                    {"Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS"},
                    {"Access-Control-Allow-Headers", "Content-Type"},
                },
                //  DefaultRedirectPath = "/page/login"
            });
            JsConfig<DateTime>.SerializeFn =time => new DateTime(time.Ticks, DateTimeKind.Local).ToString("yyyy-MM-dd HH:mm:ss");
            this.ServiceExceptionHandlers.Add((httpReq, request, exception) =>
            {
                var builder = new StringBuilder();
                builder.AppendLine(httpReq.AbsoluteUri);
                builder.AppendLine(request.ToJsv());
                builder.AppendLine(exception.Message);
                builder.AppendLine(exception.StackTrace);
                return DtoUtils.CreateErrorResponse(request, exception);
            }
                );

            this.UncaughtExceptionHandlers.Add((req, res, operationName, ex) =>
            {
                var builder = new StringBuilder("UncaughtException\r\n");
                builder.AppendLine(req.AbsoluteUri);
                builder.AppendLine(req.Dto.ToJson());
            
                builder.AppendLine(req.GetRawBody());
                builder.AppendLine(ex.Message);
                builder.AppendLine(ex.StackTrace);
                res.EndRequest(skipHeaders: true);
            });
            Plugins.Add(new RegistrationFeature());
            var appSettings = new AppSettings();
            container.Register(appSettings);
            var dbFactory = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString(), SqlServerDialect.Provider);
            container.Register<IDbConnectionFactory>(c =>dbFactory);
            var respo = new OrmLiteAuthRepository(dbFactory);
            respo.InitSchema();
            container.Register<IUserAuthRepository>(c => respo);
            Plugins.Add(new AuthFeature(
               () => new AuthUserSession(),
               new IAuthProvider[] { new CustomCredentialsAuthProvider() }, "/#/page/login"
               ));
            container.Register<IRedisClientsManager>(new PooledRedisClientManager(appSettings.GetString("redisHost")));
            dbFactory.OpenDbConnection().CreateTable<Cost>();
            dbFactory.OpenDbConnection().CreateTable<OrderCost>();
        }
    }
}