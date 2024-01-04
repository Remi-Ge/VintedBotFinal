using System.Net;

namespace VintedBot.code.network;

public class Session
{
    private readonly HttpClient _session = new HttpClient();
    
    public async Task<HttpResponseMessage> Post(string url, string content = "")
    {
        HttpContent requestContent = new StringContent(content);
        HttpResponseMessage response = await _session.PostAsync(url, requestContent);
        return response;
    }

    public async Task<string> Get(string url)
    {
        HttpResponseMessage response = await _session.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error while trying to post at {url}\n" +
                                           $"Code: {response.StatusCode}, {(int)response.StatusCode}");
        }
        string responseContent = await response.Content.ReadAsStringAsync();
        return responseContent;
    }
}