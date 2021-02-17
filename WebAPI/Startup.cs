using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
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
            services.AddControllers();

            // Autofac, Ninject, CastleWindsor, StructureMap, LightInject, DryInject --> IoC Container
            // AOP : 1 methodun �n�nde, sonunda ve ya hata verdi�inde ve ya ne zaman dersen �al��an kod par�ac�klar�n� AOP ile yazar�z :  Bu durumda da .NET IoC yeterli olmayabiliyor, yukardakilerden birine ihtiya� duyabiliyoruz
            // --> ProductManager i�inde attr�bute notlar�
            // Autofac bize AOP de sunuyor! > �cretsiz
            // Postsharp �cretli 

            // Bana arkaplanda referans olu�tur yani newle : 
            // birisi senden ctorda IProductServiceisterse onu ProductManager ile newle 
            // t�m bellekte 1 tane ProductManager olu�turup her isteyene tek 1 tane ProductManager veriyor.
            // i�erisinde Data tutmuyorsak singleton kullan�r�z !!!

            //services.AddSingleton<IProductService,ProductManager>();
            //services.AddSingleton<IProductDAL, EfProductDAL>();

            // bu i�lem asl�nda burda olmamal� daha geride olmal�. Autofac yap�caz : business manage nuget packages: autofac ve autofac.extras.dynamicproxy> add folder : DependencyResolvers
            // aervices.AddSingleton k�sm�n� kapat, Program.cs .UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer... 2 sat�r� ekle

            // data l� olanlar.
            //services.AddScoped
            //services.AddTransient
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
