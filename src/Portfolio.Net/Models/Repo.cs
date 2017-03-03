using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Portfolio.Net.Models
{
    public class Repo
    {

        public string Name { get; set; }
        public string Url { get; set; }
        public int Stars { get; set; }

        public static List<Repo> GetStarredRepos()
        {
            var client = new RestClient("https://api.github.com/users/");
            var request = new RestRequest("berausch/starred", Method.GET);
            
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            var starredRepoList = JsonConvert.DeserializeObject<List<Repo>>(jsonResponse["repos"].ToString());
            return starredRepoList;
        }
        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
