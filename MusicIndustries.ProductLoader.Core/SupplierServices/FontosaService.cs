using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProductLoader.DataContracts.SupplierPriceLists;

namespace MusicIndustries.ProductLoader.SupplierServices
{
    public class FontosaService : IFontosaService
    {
           private readonly HttpClient _httpClient;
        private readonly string _token = "1BEBFABFBP29ook";

        public FontosaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://live.frontosacpt.co.za/");
        }

        public async Task<IEnumerable<FontosaPriceList>> GetStockAsync()
        {
            var message = new HttpRequestMessage(HttpMethod.Get, $"json/stock.asp?token={_token}");
            var response = await _httpClient.SendAsync(message);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<FontosaPriceList>>(json)!;
        }

        public async Task<IEnumerable<Brand>> GetBrandsAsync()
        {
            var message = new HttpRequestMessage(HttpMethod.Get, $"json/stock_info.asp?token={_token}");
            var response = await _httpClient.SendAsync(message);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(json);
            return result;

        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var message = new HttpRequestMessage(HttpMethod.Get, $"json/stock_info.asp?token={_token}");
            var response = await _httpClient.SendAsync(message);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(json);
            return result;

        }

        public async Task<Invoice> GetInvoiceAsync(string invoiceNumber, string transactionType)
        {
            var message = new HttpRequestMessage(HttpMethod.Get,
                $"invoice.asp?token={_token}&tt={transactionType}&inv_no={invoiceNumber}");
            var response = await _httpClient.SendAsync(message);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Invoice>(json)!;
            }
            throw new HttpRequestException("Invoice not found.");
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync()
        {
            var message = new HttpRequestMessage(HttpMethod.Get, $"json/transactions.asp?token={_token}");
            var response = await _httpClient.SendAsync(message);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(json);
            return result;
        }

        public async Task<OrderResponse> PlaceOrderAsync(OrderRequest order)
        {

            var message = new HttpRequestMessage(HttpMethod.Post, $"json/order.asp");
            message.Content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(message);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OrderResponse>(json)!;
        }

        public async Task<dynamic> GetProductLinksAsync()
        {
            var message = new HttpRequestMessage(HttpMethod.Get, $"json/links.asp?token={_token}");
            var response = await _httpClient.SendAsync(message);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(json);
            return result;
        }
    }
}
