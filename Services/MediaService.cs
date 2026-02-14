using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static OnlineCoursesPlatform.Controllers.LiveStudentController;

public class MediaService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://video-lb:20000";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _apiUrl = "http://video-lb:20000";
    public MediaService(HttpClient httpClient, IHttpClientFactory httpClientFactory )
    {
        _httpClient = httpClient;
        _httpClientFactory = httpClientFactory;
    }


    public async Task<List<string>> GetActiveStreamKeysAsync()
    {
        var client = _httpClientFactory.CreateClient();
        try
        {
            var loginRes = await client.PostAsJsonAsync($"{_apiUrl}/api/v1/login", new
            {
                username = "admin",
                password = "0192023a7bbd73250516f069df18b500"
            });

            var loginData = await loginRes.Content.ReadFromJsonAsync<NmsResponse<LoginData>>();
            var token = loginData?.data?.token;

            // 2. Get Streams
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var streamsRes = await client.GetAsync($"{_apiUrl}/api/v1/streams");

            if (!streamsRes.IsSuccessStatusCode) return new List<string>();
            var result = await streamsRes.Content.ReadFromJsonAsync<NmsResponse<NmsDataContent>>();

            return result?.data?.streams?.Select(s => s.name).ToList() ?? new List<string>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[NMS Fix Error]: {ex.Message}");
            return new List<string>();
        }
    }
    private async Task<string> GetTokenAsync()
    {
        try
        {
            var loginData = new { username = "admin", password = "0192023a7bbd73250516f069df18b500" };
            var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{BaseUrl}/api/v1/login", content);

            var result = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(result);

            return doc.RootElement.GetProperty("data").GetProperty("token").GetString();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login Failed: {ex.Message}");
            return null;
        }
    }

    public async Task<string> GetActiveStreamsAsync()
    {
        string token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token)) return "[]";

        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/api/v1/streams");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }

   
}
public class NmsResponse<T>
{
    public bool success { get; set; }
    public T data { get; set; }
}

public class NmsDataContent
{
    public List<NmsStreamDetail> streams { get; set; }
}

public class NmsStreamDetail
{
    public string name { get; set; } 
    public object publisher { get; set; } 
}