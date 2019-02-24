using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation;
using KuaforEvde.Core.Cache;
using KuaforEvde.Core.Config;
using KuaforEvde.Data.DB;
using KuaforEvde.Service.Pipeline;
using KuaforEvde.Service.Query.Service;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;

namespace KuaforEvde.Web
{
    public class Startup
    {
        public static readonly Container Container = new Container();
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


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(Container));
            services.UseSimpleInjectorAspNetRequestScoping(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitializeContainer(app, env);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitializeContainer(IApplicationBuilder app, IHostingEnvironment env)
        {
            Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            //// Add application presentation components:
            Container.RegisterMvcControllers(app);
            Container.RegisterMvcViewComponents(app);

            var assemblies = GetAssemblies().ToArray();
            Container.RegisterSingleton<IMediator, Mediator>();
            Container.Register(typeof(IRequestHandler<>), assemblies);
            Container.Register(typeof(IRequestHandler<,>), assemblies);
            Container.RegisterCollection(typeof(INotificationHandler<>), assemblies);
            Container.RegisterCollection(typeof(IPipelineBehavior<,>), new[]
            {
                typeof(RequestPreProcessorBehavior<,>),
                typeof(RequestPostProcessorBehavior<,>),
                typeof(KuaforEvdeApiPipelineBehavior<,>)
            });
            Container.RegisterCollection(typeof(IRequestPreProcessor<>), new[] { typeof(KuaforEvdeRequestPreProcessor<>) });
            Container.RegisterCollection(typeof(IRequestPostProcessor<,>), new[] { typeof(KuaforEvdeRequestPostProcessor<,>) });
            Container.RegisterInstance(new ServiceFactory(Container.GetInstance));

            Container.RegisterCollection(typeof(IValidator<>), assemblies);
            Container.Register<KuaforEvdeContext, KuaforEvdeContext>(Lifestyle.Scoped);
            Container.RegisterInstance(new RedisConfig());
            Container.RegisterSingleton<IMemoryCacher, MemoryCacher>();
            Container.RegisterSingleton<IRedisCacher, RedisCacher>();
            Container.RegisterSingleton<IMultiCacher, MultiCacher>();
            Container.Verify();
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(Startup).GetTypeInfo().Assembly;
            yield return typeof(ServiceSaveQuery).GetTypeInfo().Assembly;
        }
    }
}
