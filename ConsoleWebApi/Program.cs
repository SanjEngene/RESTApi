using ConsoleRESTful.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ConsoleWebApi
{
    static class ApiCallerFactory
    {
        public static ApiCaller CreateApiCaller()
        {
            return new ApiCaller();
        }
    }
    class ApiCaller : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        public ApiCaller()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:11610/api/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task Get()
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync("Home?format=xml");
            var content = await responseMessage.Content.ReadAsStringAsync();

            
            if (responseMessage.Content.Headers.ContentType.MediaType == "application/json")
            {
                List<Project> projects = JsonSerializer.Deserialize<List<Project>>(content, _jsonOptions);
                foreach (Project project in projects)
                {
                    Console.WriteLine($"{project?.Id}\n{project?.Name}\n{project?.Filepath}\n{project?.CreatedAt}\n{project?.ProjectType}");
                }
            }
            else if (responseMessage.Content.Headers.ContentType.MediaType == "application/xml")
            {
                
                var xDoc = XDocument.Parse(content);
                Console.WriteLine(Program.GetXML(xDoc.Root.Elements(), ""));
            }

        }

        public async Task Get(int id)
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"Home/{id}?format=xml");
            var content = await responseMessage.Content.ReadAsStringAsync();
            if (responseMessage.Content.Headers.ContentType.MediaType == "application/json")
            {
                Project project = JsonSerializer.Deserialize<Project>(content, _jsonOptions);
                Console.WriteLine($"{project?.Id}\n{project?.Name}\n{project?.Filepath}\n{project?.CreatedAt}\n{project?.ProjectType}");
            }
            else if(responseMessage.Content.Headers.ContentType.MediaType == "application/xml")
            {
                var xDoc = XDocument.Parse(content);
                Console.WriteLine(Program.GetXML(xDoc.Root.Elements(), ""));
            }
        }

        public async Task Post(Project project)
        {
            StringContent content = new StringContent(JsonSerializer.Serialize(project, typeof(Project)), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await _httpClient.PostAsync("Home", content);
        }

        public async Task Put(Project project)
        {
            StringContent content = new StringContent(JsonSerializer.Serialize(project, typeof(Project)), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await _httpClient.PutAsync("Home", content);
        }

        public async Task Delete(int id)
        {
            HttpResponseMessage responseMessage = await _httpClient.DeleteAsync($"Home/{id}");
        }
        public void Dispose()
        {
            Console.WriteLine("Resources are free");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            ApiCaller callApi = null;
            try
            {
                callApi = ApiCallerFactory.CreateApiCaller();
                callApi.Get(1).Wait();
            }
            finally
            {
                if (callApi != null)
                {
                    callApi.Dispose();
                }
            }

        }
        public static string GetXML(IEnumerable<XElement> xmlList, string value)
        {
            foreach (XElement xElement in xmlList)
            {
                if (xElement.HasElements)
                {
                    value += $"<{xElement.Name.LocalName}>\n";
                    value = GetXML(xElement.Elements(), value);
                    value += $"</ {xElement.Name.LocalName}>\n";
                }
                else
                {
                    value += $"<{xElement.Name.LocalName}>\n";
                    value += $"{xElement.Value}\n";
                    value += $"< /{xElement.Name.LocalName}>\n";
                }
            }

            return value;
        }
    }
}
