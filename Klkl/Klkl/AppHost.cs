using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using Funq;
using Klkl.ServiceInterface;
using ServiceStack.Razor;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;

//好像可以了？？
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
            Plugins.Add(new AutoQueryFeature {MaxLimit = 100});
            //   Feature disableFeatures = Feature.Json| Feature.Html;
            SetConfig(new HostConfig()
            {
                AllowFileExtensions = {"json"},
                DefaultContentType = MimeTypes.Json,
                DefaultRedirectPath = "/page/login"
            });

            this.ServiceExceptionHandlers.Add((httpReq, request, exception) =>
            {
                var builder = new StringBuilder();
                builder.AppendLine(httpReq.AbsoluteUri);
                builder.AppendLine(request.ToJsv());
                builder.AppendLine(exception.Message);
                builder.AppendLine(exception.StackTrace);
                return exception;
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

            var dbFactory = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString(), SqlServerDialect.Provider);
            container.Register<IDbConnectionFactory>(c =>dbFactory);
            container.Register<IUserAuthRepository>(c => new OrmLiteAuthRepository(dbFactory));
            Plugins.Add(new AuthFeature(() => new AuthUserSession(),
                new IAuthProvider[] { new CustomCredentialsAuthProvider() }, "/#/page/login"));
        }
    }
}