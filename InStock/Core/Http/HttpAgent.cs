using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace InStock.Core.Http;

public class HttpAgent
{
    private string _fullLink;
    private int _timeout;
    private HttpClientHandler _httpClientHandler;

    private HttpClient _httpClient;
    public HttpAgent(string link, int timeout)
    {
        _fullLink = link;
        _timeout = timeout;
        
        _httpClientHandler = new() 
        {
            AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
        };
        
        _httpClient = CreateClient();
    }

    private HttpClient CreateClient()
    {
        TimeSpan timeoutSeconds = TimeSpan.FromSeconds(_timeout);
        HttpClient httpClient =  new(_httpClientHandler, true)
        {
            Timeout = timeoutSeconds
        };
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; rv:87.0) Gecko/20100101 Firefox/87.0");
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

        return httpClient;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    public string FetchBody()
    {
        async Task<string> AsyncImpl()
        {
            return await _httpClient.GetStringAsync(_fullLink);
        }

        return AsyncImpl().Result ?? String.Empty;
    }

    public string FetchLink()
    {
        return _fullLink;
    }
}