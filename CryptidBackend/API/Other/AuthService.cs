using Newtonsoft.Json;
using System.Net;

namespace Cryptid.Backend
{
    public class AuthService
    {
        public class UnityAuthModel
        {
            public string id;
            public bool disabled;
        }

        private readonly ILogger<AuthService> logger;

        public AuthService(ILogger<AuthService> logger)
        {
            this.logger = logger;
        }

        public UnityAuthModel Authenticate(string userId, string accessToken)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://player-auth.services.api.unity.com/v1/users/{userId}");
                httpWebRequest.Method = "GET";
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("ProjectId: 0d249754-4ef3-4583-bb0f-8a8286dfaaaf");
                httpWebRequest.Headers.Add($"Authorization: Bearer {accessToken}");

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                switch (httpResponse.StatusCode)
                {
                    case HttpStatusCode.OK:
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            UnityAuthModel model = JsonConvert.DeserializeObject<UnityAuthModel>(result);
                            return model;
                        }
                    case HttpStatusCode.Forbidden:

                        break;
                }
                return new UnityAuthModel();
            }
            catch(Exception ex)
            {
                logger.LogError("Error while trying to login.");
                return new UnityAuthModel();
            }
        }
    }
}
