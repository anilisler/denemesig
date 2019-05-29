using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace denemesig
{
    /// <summary>
    /// Startup class where all the configuration is done and processed once the application is starting. 
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor of the <see cref="T:denemesig.Startup"/> class.
        /// Initializes a new instance of the <see cref="T:denemesig.Startup"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the application container.
        /// </summary>
        /// <value>The application container.</value>
        public IContainer ApplicationContainer { get; private set; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures the services.
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <returns>IServiceProvider</returns>
        /// <param name="services">Services.</param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR();

            // .net core built-in dependency injection
            //services.Add(new ServiceDescriptor(typeof(ISave), new CloudStorage()));

            //autofac IoC container build
            // Create the container builder.
            var builder = new ContainerBuilder();
            builder.Populate(services);
            //register CloudStorage class as ISave interface
            builder.RegisterType<CloudStorage>().As<ICloudStorage>();
            this.ApplicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        /// <summary>
        /// Configure the specified app and env.
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">App.</param>
        /// <param name="env">Env.</param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
            app.UseCookiePolicy();

            app.UseMvc();
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}
