using DotNetODataWebClientOidc.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DotNetODataWebClientOidc.Services
{
    public static class CourseServiceExtensions
    {
        public static void AddCourseService(this IServiceCollection services, IConfiguration configuration)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            services.AddHttpClient<ICourseService, CourseService>();
        }
    }

    /// <summary>
    /// Service to call CampusNexus API
    /// </summary>
    public class CourseService : ICourseService
    {
        private readonly HttpClient _httpClient;
        private readonly string _CampusNexusScope;
        private readonly string _CampusNexusBaseAddress;
        private readonly ITokenAcquisition _tokenAcquisition;

        public CourseService(ITokenAcquisition tokenAcquisition, HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _tokenAcquisition = tokenAcquisition;
            _CampusNexusScope = configuration["CampusNexus:CampusNexusScope"];
            _CampusNexusBaseAddress = configuration["CampusNexus:CampusNexusBaseAddress"];
        }

        /// <summary>
        /// Get AccessToken and add it to HttpClient as Bearer token
        /// </summary>
        /// <returns></returns>
        private async Task PrepareAuthenticatedClient()
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { _CampusNexusScope });
            Debug.WriteLine($"access token-{accessToken}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Get list of top 100 active courses
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Course>> GetAsync()
        {
            await PrepareAuthenticatedClient();
            var response = await _httpClient.GetAsync(new Uri(new Uri(_CampusNexusBaseAddress), "ds/odata/Courses?$select=Id,Code,Name&$filter=IsActive eq true&$top=100"));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<JObject>(content);
                IEnumerable<Course> courseList = JsonConvert.DeserializeObject<IEnumerable<Course>>(result["value"].ToString());

                return courseList;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        /// <summary>
        /// Get single course by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Course> GetAsync(int id)
        {
            await PrepareAuthenticatedClient();
            var response = await _httpClient.GetAsync(new Uri(new Uri(_CampusNexusBaseAddress), $"ds/odata/Courses({id})"));
 
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                Course course = JsonConvert.DeserializeObject<Course>(content);

                return course;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

       
    }
}