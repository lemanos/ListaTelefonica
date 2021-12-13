using FluentAssertions;
using ListaTelefonica.API;
using ListaTelefonica.Domain.Entities;
using ListaTelefonica.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ListaTelefonica.Tests
{
   public  class CadastroAPITest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string DatabaseName = "ContatoDbTest";
        private const string ContatoGetEndpoint = "/Contato";
        private const string ContatoPostEndpoint = "/Contato";
        private const string PathJsonCorretoJson = "Json//InclusaoCorreta.json";
        private const string PathJsonErroJson = "Json//IncErro.json";

        private readonly WebApplicationFactory<Startup> _factory;

        public CadastroAPITest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ContatoGetEndpoint_Should_GetData()
        {
            // arrange
            var client = CreateClient();
            await using var context = GetDbContext();

            //act
            var response = await client.GetAsync(ContatoGetEndpoint);
            var content = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<Contato>>(content);

            //assert
            Assert.Equal(4, list.Count());
            response.EnsureSuccessStatusCode();
        }


        [Fact]
        public async Task ContatoPostEndpoint_Should_Persist_Data_In_Database()
        {
            // arrange
            var client = CreateClient();

            //act
            var response = await client.PostAsync(ContatoPostEndpoint,
                new StringContent(await File.ReadAllTextAsync(PathJsonCorretoJson), Encoding.UTF8, "application/json"));

            //assert
            response.EnsureSuccessStatusCode();
            await using var context = GetDbContext();
            context.Contato.Count().Should().Be(4);
        }


        [Fact]
        public async Task ContatoPostEndpoint_Should_Persist_Data_In_Database_Whith_Error()
        {
            // arrange
            var client = CreateClient();

            //act
            var response = await client.PostAsync(ContatoPostEndpoint,
                new StringContent(await File.ReadAllTextAsync(PathJsonErroJson), Encoding.UTF8, "application/json"));

            //assert
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        }






        private static ListaTelefonicaDbContext GetDbContext()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase(DatabaseName);
            var context = new ListaTelefonicaDbContext(builder.Options);

            if (context.Contato.Count() == 0)
            {
                context.Contato.Add(new Domain.Entities.Contato() { Id = 1, Nome = "Nome1", IsAtivo = true });
                context.Contato.Add(new Domain.Entities.Contato() { Id = 2, Nome = "Nome2", IsAtivo = true });
                context.Contato.Add(new Domain.Entities.Contato() { Id = 3, Nome = "Nome3", IsAtivo = true });
                context.Contato.Add(new Domain.Entities.Contato() { Id = 4, Nome = "Nome4", IsAtivo = true });


                context.SaveChanges();
            }
            return context;
        }

        private HttpClient CreateClient() =>
            _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ListaTelefonicaDbContext>));
                    services.Remove(descriptor);
                    services.AddDbContext<ListaTelefonicaDbContext>(options => options.UseInMemoryDatabase(DatabaseName));

                    using var scope = services.BuildServiceProvider().CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ListaTelefonicaDbContext>();

                    db.Database.EnsureCreated();
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }
}