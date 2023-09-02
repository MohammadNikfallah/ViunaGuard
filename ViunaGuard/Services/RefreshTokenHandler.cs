using System.Net.Http.Headers;
using System.Text.Json;

namespace ViunaGuard.Services
{
    public class RefreshTokenHandlerClass : IRefreshTokenHandler
    {
        private readonly IConfiguration _configuration;

        public RefreshTokenHandlerClass(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public async Task<JsonDocument> AccessRefresh(string refreshToken)
        {
            System.IO.File.AppendAllText("log", DateTime.Now + " : refreshing the access token\n");
            var request = new RtRequest()
            {
                ClientId = "12345",
                ClientSecret = "secretTest",
                RefreshToken = refreshToken,
                TokenEndpoint = $"{_configuration.GetValue<string>("Constants:OauthBaseUrl")}api/OAuth/token"
            };

            var tokens = await RefreshTokenHandlerClass.RefreshTokenHandler(request);
            return tokens;
        }
        public static async Task<JsonDocument> RefreshTokenHandler(RtRequest request)
        {
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "client_id", request.ClientId },
                { "client_secret", request.ClientSecret },
                { "refresh_token", request.RefreshToken },
                { "grant_type", "refresh_token" },
            };

            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);

            var http = new HttpClient();

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, request.TokenEndpoint);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Content = requestContent;
            requestMessage.Version = http.DefaultRequestVersion;
            var response = await http.SendAsync(requestMessage);
            var body = await response.Content.ReadAsStringAsync();

            return JsonDocument.Parse(body);
        }

    }

    public class RtRequest
    {
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public string TokenEndpoint { get; set; } = null!;
    }
}
