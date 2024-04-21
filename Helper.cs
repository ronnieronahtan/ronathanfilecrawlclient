using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ronathanFileCrawler
{
    public static class Helper
    {

        public static async Task<string> getAccessToken(params string[] host)
        {
            var client = new HttpClient();
            var urlToUse = "kc.esr-inc.com:8443";
            if (host.Length > 0)
            {
                //check to see if this is a full url or just a host
                if (host[0].Contains("http"))
                {
                    urlToUse = host[0].Split("//")[1].Split("/")[0];
                    Console.WriteLine("host had to strip: " + urlToUse);
                }
                else
                {
                    urlToUse = host[0];
                }

                urlToUse = host[0];

            }
            var fyou = "https://" + urlToUse + "/realms/dev/protocol/openid-connect/token";
            Console.WriteLine("getting token from: " + fyou);
            var request = new HttpRequestMessage
            {

                Method = HttpMethod.Post,
                RequestUri = new Uri(fyou),

                ////Headers =
                ////{
                ////    { "Content-Type", "application/x-www-form-urlencoded" }
                ////},
                Content = new FormUrlEncodedContent(new[]
               {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", "mydevclient"),
                    new KeyValuePair<string, string>("username", "beta"),
                    new KeyValuePair<string, string>("password", "ronathan"),
                    new KeyValuePair<string, string>("client_secret", "AL20HUPhegsbFgrS310CdAGa99a5sZtj")
                })

            };
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine(content);
            return content;
        }
        
        //get the token from local Ronathan Access Token Server
        public static async Task<string> getAccessToken()
        {
            var urlToUse = "ronathanbeta.esr-inc.com:6001";
            var client = new HttpClient();
            var fyou = "https://" + urlToUse + "/api/token";
            Console.WriteLine("getting token from: " + fyou);

            //create a json onject with the email and password
            var json = JsonSerializer.Serialize(new
            {
                email = "CHANGEME",
                password = "CHANGEME"
            });

            var request = new HttpRequestMessage
            {

                Method = HttpMethod.Post,
                RequestUri = new Uri(fyou),
                Content = new StringContent(json, Encoding.UTF8, "application/json")


            };
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine(content);
            return content;

        }
    
    }
}
