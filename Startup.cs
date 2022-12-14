using AmanahTeknologTask.Domains;
using AmanahTeknologTask.Services;
using AmanahTeknologTask.Unit_Of_Work;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmanahTeknologTask
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
            services.AddDbContext<DataContext>
             (options =>
             {
                 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
             });
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod().AllowAnyHeader();
                });
            });
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddControllers();
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AmanahTeknologTask", Version = "v1" });
            });
           
            //services.AddCors(o =>
            //{
            //    o.AddDefaultPolicy(builder=>builder.AllowAnyMethod()
            //    .AllowAnyHeader().SetIsOriginAllowed((host)=>true).AllowCredentials());
            //});
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AmanahTeknologTask v1"));
            }
            app.UseRouting();
            app.UseHttpsRedirection();  
            app.UseAuthorization();
            //app.UseCors(options => options.AllowAnyOrigin());
            app.UseCors(options => options.WithOrigins("http://localhost:4200")//should like this, without any / in the last.
            .AllowAnyMethod() //to allow methods get post put .....
            .AllowAnyHeader());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
