using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UEditor.Core;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
//using UEditorNetCore;

namespace STORE.WebAPI
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
            services.AddMvc();
            services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(x =>
            {

                x.ValueLengthLimit = int.MaxValue;

                x.MultipartBodyLengthLimit = int.MaxValue;

            });
            services.AddUEditorService("ueditor.json",true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //        app.UseStaticFiles(new StaticFileOptions
            //        {
            //            FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
            //    System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "ExcelModel")),
            //            RequestPath = "/ExcelModel"
            //        });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
    System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "UploadFiles/community/pic")),
                RequestPath = "/UploadFiles/community/pic",
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=36000");
                }
            });
            #region 解决Ubuntu Nginx 代理不能获取IP问题
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
            });
            #endregion
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
            });
            app.UseMvc();
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //       Path.Combine(Directory.GetCurrentDirectory(), "upload")),
            //    RequestPath = "/upload",
            //    OnPrepareResponse = ctx =>
            //    {
            //        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=36000");
            //    }
            //});
        }
    }
}
