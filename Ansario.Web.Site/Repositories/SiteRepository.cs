using Ansario.Web.Site.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Ansario.Web.Site.Repositories
{
    public class SiteRepository : ISiteRepository
    {
        private IRestClient _client;

        public SiteRepository()
        {
            _client = new RestClient("http://api.ansario.com/v1");
        }

        public async Task<string> Login(string username, string password)
        {
            var request = new RestRequest("/account/login", Method.POST);

            request.AddParameter("username", username);
            request.AddParameter("password", password);

            var response = await _client.ExecuteTaskAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<dynamic>(response.Content);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<BodyRecordModel>> Record(string id)
        {
            var request = new RestRequest($"/m3c/record/{id}", Method.GET);

            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            request.AddParameter("Authorization", $"Bearer {claims.First(claim => claim.Type == "token").Value}", ParameterType.HttpHeader);

            var response = await _client.ExecuteTaskAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<BodyRecordModel>>(response.Content);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<BodyRecordModel>> Records(string latitude = null, string longitude = null, double? distance = null)
        {
            var request = new RestRequest("/m3c/all", Method.GET);

            if (!string.IsNullOrWhiteSpace(latitude))
            {
                request.AddParameter("latitude", latitude, ParameterType.UrlSegment);
            }
            if (!string.IsNullOrWhiteSpace(latitude))
            {
                request.AddParameter("longitude", longitude, ParameterType.UrlSegment);
            }
            if (!string.IsNullOrWhiteSpace(latitude))
            {
                request.AddParameter("distance", distance, ParameterType.UrlSegment);
            }

            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            request.AddParameter("Authorization", $"Bearer {claims.First(claim => claim.Type == "token").Value}", ParameterType.HttpHeader);


            var response = await _client.ExecuteTaskAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<BodyRecordModel>>(response.Content);
            }
            else
            {
                return null;
            }
        }
    }
}