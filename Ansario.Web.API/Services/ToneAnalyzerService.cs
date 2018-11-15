using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Ansario.Web.API.Models.ToneAnalyzerModels;

namespace Ansario.Web.API.Services
{
    public class ToneAnalyzerService : IToneAnalyzerService
    {

        public IRestClient _restClient; 

        public ToneAnalyzerService()
        {
            _restClient = new RestClient("https://gateway.watsonplatform.net/tone-analyzer/api/v3/tone")
            {
                Authenticator = new HttpBasicAuthenticator("a086a918-448e-4cb1-87b6-8cd8be95609f", "RDSchXmvh7KM")
            };
        }

        public AnalyzedTone AnalyzeText(string text)
        {
            var request = new RestRequest("", Method.GET);
            request.AddParameter("text", HttpUtility.UrlEncode(text));
            request.AddParameter("version", "2017-09-21");
            request.AddParameter("sentences", "false");

            var response = _restClient.Execute(request);

            return JsonConvert.DeserializeObject<AnalyzedTone>(response.Content);
        }
    }
}