using Business.Abstract;
using Business.Concrete;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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

            // Core a taþýdýk. > DependencyResolvers.CoreModule
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddCors();  // cors injection

            #region JWTBearer Token kullanýcaz dediðimiz yer burasý

            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                    };
                });

            // geçici çözümdü yerine alttaki
            //ServiceTool.Create(services);

            services.AddDependencyResolvers(new ICoreModule[] 
            { 
                new CoreModule()
            });

            #endregion

            // Autofac, Ninject, CastleWindsor, StructureMap, LightInject, DryInject --> IoC Container
            // AOP : 1 methodun önünde, sonunda ve ya hata verdiðinde ve ya ne zaman dersen çalýþan kod parçacýklarýný AOP ile yazarýz :  Bu durumda da .NET IoC yeterli olmayabiliyor, yukardakilerden birine ihtiyaç duyabiliyoruz
            // --> ProductManager içinde attrþbute notlarý
            // Autofac bize AOP de sunuyor! > ücretsiz
            // Postsharp ücretli 

            // Bana arkaplanda referans oluþtur yani newle : 
            // birisi senden ctorda IProductServiceisterse onu ProductManager ile newle 
            // tüm bellekte 1 tane ProductManager oluþturup her isteyene tek 1 tane ProductManager veriyor.
            // içerisinde Data tutmuyorsak singleton kullanýrýz !!!

            //services.AddSingleton<IProductService,ProductManager>();
            //services.AddSingleton<IProductDAL, EfProductDAL>();

            // bu iþlem aslýnda burda olmamalý daha geride olmalý. Autofac yapýcaz : business manage nuget packages: autofac ve autofac.extras.dynamicproxy> add folder : DependencyResolvers
            // aervices.AddSingleton kýsmýný kapat, Program.cs .UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer... 2 satýrý ekle

            // data lý olanlar.
            //services.AddScoped
            //services.AddTransient
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) // Burada sýralama önemli
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.WithOrigins("http://localhost:4200","http://localhost:53487").AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            //ekledik.
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
