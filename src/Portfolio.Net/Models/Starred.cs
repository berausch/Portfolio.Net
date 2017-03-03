using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Portfolio.Net.Models
{
    public class Starred
    {

        public string Name { get; set; }
        public string Html_url { get; set; }
        public int Stargazers_count { get; set; }
        public DateTime Updated_at { get; set; }

        public static List<Starred> GetStarredRepos()
        {
            var client = new RestClient("https://api.github.com");
            
            var request = new RestRequest("/users/berausch/starred", Method.GET);
            request.AddHeader("User-Agent", "berausch");
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            var starredRepoList = JsonConvert.DeserializeObject<List<Starred>>(response.Content.ToString())
                .OrderByDescending(o => o.Stargazers_count).ThenByDescending(o => o.Updated_at).ToList();



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
