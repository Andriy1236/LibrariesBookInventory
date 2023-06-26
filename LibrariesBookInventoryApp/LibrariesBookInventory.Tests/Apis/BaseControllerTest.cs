using Autofac;
using LibrariesBookInventory.Persistence;
using LibrariesBookInventoryApi;
using Microsoft.Owin.Hosting;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace LibrariesBookInventory.Tests.Apis
{
    public abstract class BaseControllerTest : IDisposable
    {
        private const string BaseAddress = "http://localhost:59301/";
        private readonly IDisposable _server;
        private readonly HttpClient _httpClient;
        protected abstract string BaseApi { get; }
        protected readonly ApplicationDbContext Db;

        protected BaseControllerTest()
        {
            _server = WebApp.Start<Startup>(url: BaseAddress);
            Db = CreateTestDbContext();
            _httpClient = new HttpClient();
        }

        private static ApplicationDbContext CreateTestDbContext()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ApplicationDbContext>().AsSelf()
                    .WithParameter("connectionString", ConfigurationManager.AppSettings["connectionString"]);
            var container = builder.Build();
            return container.Resolve<ApplicationDbContext>();
        }

        protected async Task<HttpResponseMessage> Get(string requestUrl)
        {
            var url = GetUrl(requestUrl);
            return await _httpClient.GetAsync(url);
        }

        protected async Task<HttpResponseMessage> Post<T>(string requestUrl, T requestData)
        {
            var url = GetUrl(requestUrl);
            return await _httpClient.PostAsJsonAsync(url, requestData);
        }

        protected async Task<HttpResponseMessage> Put<T>(string requestUrl, T requestData)
        {
            var url = GetUrl(requestUrl);
            return await _httpClient.PutAsJsonAsync(url, requestData);
        }

        protected async Task<HttpResponseMessage> Delete(string requestUrl)
        {
            var url = GetUrl(requestUrl);
            return await _httpClient.DeleteAsync(url);
        }

        private string GetUrl(string requestUrl)
        {
            return $"{BaseAddress}/{BaseApi}/{requestUrl}";
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _server.Dispose();
        }
    }
}
