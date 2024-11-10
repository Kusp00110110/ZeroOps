using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MusicIndustries.ProductLoader.Notifications;
using Newtonsoft.Json;
using ProductLoader.DataContracts.ImageApi;

namespace MusicIndustries.ProductLoader.ProductImages
{
    public class ProcessProductImages : IProcessProductImages
    {
        private readonly HttpClient _fiveSecondClient;
        private readonly HttpClient _httpClient;
        private readonly IProcessNotifications _notificationService;

        public ProcessProductImages(IProcessNotifications notificationService, IHttpClientFactory clientFactory, HttpClient httpClient)
        {
            _fiveSecondClient = clientFactory.CreateClient();
            _fiveSecondClient.Timeout = TimeSpan.FromSeconds(5);
            _notificationService = notificationService;
            _httpClient = httpClient;
        }

        public async Task<ImageApiResponse[]?> SearchDuckDuckGoAsync(string keywords)
        {
            try
            {
                var stopWatch = Stopwatch.StartNew();
                _httpClient.DefaultRequestHeaders.Clear();
                var searchItems = keywords.Split("|");
                if (searchItems.Length > 1)
                {
                    return await SearchFreeAsPrimary(keywords, stopWatch);
                }
                return await SearchFreeAsPrimary(searchItems[0], new Stopwatch());
            }
            catch (Exception e)
            {
                Console.WriteLine("Woops Something went wrong. retrying...");
                await _notificationService.SendNotificationAsync("Woops Something went wrong. retrying...");
                return await SearchDuckDuckGoAsync(keywords);
            }
        }


        public async Task<bool> CheckImageExistsAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Url cannot be null or empty.", nameof(url));

            try
            {
                var response = await _fiveSecondClient.GetAsync(url);
                // Image should return in 5 seconds
                var encoding = response.Content.Headers.ContentEncoding.FirstOrDefault();
                if (encoding == "gzip")
                {
                    await using var stream = new GZipStream(await response.Content.ReadAsStreamAsync(),
                        CompressionMode.Decompress);
                    using var reader = new StreamReader(stream);
                    var responseDataGzip = await reader.ReadToEndAsync();
                    return !string.IsNullOrWhiteSpace(responseDataGzip);
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during image check: {ex.Message}");
                return false;
            }
        }
        public async Task<ImageApiResponse[]> SearchAsync(string keywords)
        {
            var baseUrl = "https://duckduckgo.com/";

            Console.WriteLine("Hitting DuckDuckGo for Token...");
            var vqd = await GetTokenAsync(baseUrl, keywords);
            if (vqd == null)
            {
                Console.WriteLine("Token Parsing Failed!");
                return default;
            }

            Console.WriteLine("Obtained Token.");

            var requestUrl = $"{baseUrl}i.js";
            var queryParams = new[]
            {
                ("l", "us-en"),
                ("o", "json"),
                ("q", keywords),
                ("vqd", vqd),
                ("f", ",,,"),
                ("p", "1"),
                ("v7exp", "a")
            };

            var resultCount = 0;

            return await MakeRequestAsync(requestUrl, queryParams);
        }


        private async Task<ImageApiResponse[]?> SearchFreeAsPrimary(string keywords, Stopwatch stopWatch)
        {
            var searchStrings = keywords.Split("|");
            var primarySearchResults = Array.Empty<ImageApiResponse>();
            foreach (var searchString in searchStrings)
            {
                primarySearchResults = await SearchAsync(searchString);
                stopWatch.Stop();
                Console.WriteLine($"Search took: {stopWatch.ElapsedMilliseconds}ms");

                if (primarySearchResults.Any())
                {
                    return primarySearchResults;
                }
            }
            return primarySearchResults.Any() ? primarySearchResults : Array.Empty<ImageApiResponse>();
        }

        private async Task<string?> GetTokenAsync(string url, string keywords)
        {
            var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("q", keywords) });
            var response = await _httpClient.PostAsync(url, content);
            var body = await response.Content.ReadAsStringAsync();

            var match = Regex.Match(body, @"vqd=([\d-]+)&");
            return match.Success ? match.Groups[1].Value : null;
        }

        private async Task<ImageApiResponse[]?> MakeRequestAsync(string url, (string, string)[] queryParams)
        {
            var requestUrl = QueryString(url, queryParams);
            Console.WriteLine($"Searching url: {requestUrl}");
            ;
            var headers = new HttpRequestMessage(HttpMethod.Get, requestUrl);
/*headers = {
    'authority': 'duckduckgo.com',
    'accept': 'application/json, text/javascript, * /*; q=0.01',
    'sec-fetch-dest': 'empty',
    'x-requested-with': 'XMLHttpRequest',
    'user-agent': 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36',
    'sec-fetch-site': 'same-origin',
    'sec-fetch-mode': 'cors',
    'referer': 'https://duckduckgo.com/',
    'accept-language': 'en-US,en;q=0.9',
}*/
            headers.Headers.Add("authority", "duckduckgo.com");
            headers.Headers.Add("accept", "application/json, text/javascript, * /*; q=0.01");
            headers.Headers.Add("sec-fetch-dest", "empty");
            headers.Headers.Add("x-requested-with", "XMLHttpRequest");
            headers.Headers.Add("user-agent",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36");
            headers.Headers.Add("sec-fetch-site", "same-origin");
            headers.Headers.Add("sec-fetch-mode", "cors");
            headers.Headers.Add("referer", "https://duckduckgo.com/");
            headers.Headers.Add("accept-language", "en-US,en;q=0.9");

            var response = await _httpClient.SendAsync(headers);
            var responseEncoding = response.Content.Headers.ContentEncoding.FirstOrDefault();
            if (response.IsSuccessStatusCode)
            {
                if (responseEncoding == "gzip")
                {
                    await using var stream = new GZipStream(await response.Content.ReadAsStreamAsync(),
                        CompressionMode.Decompress);
                    using var reader = new StreamReader(stream);
                    var responseDataGzip = await reader.ReadToEndAsync();
                    var jsonDocumentGzip = JsonDocument.Parse(responseDataGzip);
                    var imagesJsonElement = jsonDocumentGzip.RootElement.GetProperty("results");
                    var jsonArray = imagesJsonElement.ToString();
                    var modeled = JsonConvert.DeserializeObject<ImageApiResponse[]>(jsonArray);
                    return modeled;
                }
                if (responseEncoding == "deflate")
                {
                    await using var stream = new DeflateStream(await response.Content.ReadAsStreamAsync(),
                        CompressionMode.Decompress);
                    using var reader = new StreamReader(stream);
                    var responseDataDeflate = await reader.ReadToEndAsync();
                    return ParseResponse(responseDataDeflate);
                }

                if (responseEncoding == "br")
                {
                    await using var stream = new BrotliStream(await response.Content.ReadAsStreamAsync(),
                        CompressionMode.Decompress);
                    using var reader = new StreamReader(stream);
                    var responseDataBr = await reader.ReadToEndAsync();
                    return ParseResponse(responseDataBr);
                }

                var responseData = await response.Content.ReadAsStringAsync();
                return ParseResponse(responseData);
            }

            return Array.Empty<ImageApiResponse>();
        }

        private static string QueryString(string url, (string, string)[] parameters)
        {
            var query = string.Join("&",
                Array.ConvertAll(parameters, p => $"{p.Item1}={Uri.EscapeDataString(p.Item2)}"));
            return $"{url}?{query}";
        }

        // Helper method for parsing the response.
        private ImageApiResponse[] ParseResponse(string responseData)
        {
            var jsonDocument = JsonDocument.Parse(responseData);
            var imagesJsonElement = jsonDocument.RootElement.GetProperty("results");
            var jsonArray = imagesJsonElement.ToString();
            return JsonConvert.DeserializeObject<ImageApiResponse[]>(jsonArray) ??
                   Array.Empty<ImageApiResponse>();
        }
    }
}
