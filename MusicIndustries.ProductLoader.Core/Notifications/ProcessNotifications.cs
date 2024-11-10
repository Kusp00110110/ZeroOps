using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MusicIndustries.ProductLoader.Notifications
{
    public class ProcessNotifications(HttpClient httpClient) : IProcessNotifications
    {

        private const string Token = "arkmv661urpi1qyepejmpsuf3f9koq";
        private const string User = "grxswvwt4yi1mqtmkvuxk71pndyt4t";
        private const string UrlApi = "https://api.pushover.net/1/messages.json";

        public async Task SendNotificationAsync(
            string message,
            string title = null,
            string url = null,
            string urlTitle = null,
            string sound = null,
            int? priority = null,
            string attachmentPath = null)
        {
            using var form = new MultipartFormDataContent();
            // Add required parameters
            form.Add(new StringContent(Token), "token");
            form.Add(new StringContent(User), "user");
            form.Add(new StringContent(message), "message");

            // Add optional parameters if provided
            if (!string.IsNullOrEmpty(title))
            {
                form.Add(new StringContent(title), "title");
            }
            if (!string.IsNullOrEmpty(url))
            {
                form.Add(new StringContent(url), "url");
            }
            if (!string.IsNullOrEmpty(urlTitle))
            {
                form.Add(new StringContent(urlTitle), "url_title");
            }
            if (!string.IsNullOrEmpty(sound))
            {
                form.Add(new StringContent(sound), "sound");
            }
            if (priority.HasValue)
            {
                form.Add(new StringContent(priority.Value.ToString()), "priority");
            }
            if (!string.IsNullOrEmpty(attachmentPath))
            {
                var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(attachmentPath));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                form.Add(fileContent, "attachment", Path.GetFileName(attachmentPath));
            }
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, "")
            {
                Content = form
            };

            // Send the request
            var response = await httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Notification sent successfully!");
        }
    }
}
