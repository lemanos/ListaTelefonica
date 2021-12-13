using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ListaTelefonica.API.SwaggerConfig
{
    public static class ConfigSwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services) =>
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Api de teste",
                        Version = "v1",
                        Description = "REST API de Teste",
                        Contact = new OpenApiContact
                        {
                            Name = "Lista Telefonica",
                            Url = new Uri("https://provertec.com.br/")
                        }
                    });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

        public static void UseSwaggerSetup(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Documentação API Teste";
                c.SwaggerEndpoint("v1/swagger.json", "v1");
                c.OAuthClientId("swagger-dash");
                c.OAuthAppName("Swagger Dashboard");
                c.EnableFilter();
                c.DisplayOperationId();
                c.DisplayRequestDuration();
                c.DocExpansion(DocExpansion.List);
                c.EnableDeepLinking();
                c.RoutePrefix = "swagger";
            });
        }
    }

}
