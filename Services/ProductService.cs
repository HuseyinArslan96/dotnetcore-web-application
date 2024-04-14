using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Elasticsearch.Net;
using Nest;
using WebApplication.Models;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using RabbitMQ.Client;
using System.Net;

namespace WebApplication.Services
{
    public class ProductService
    {
        private const string RabbitMQHost = "localhost";
        private const string QueueName = "product_queue";
        private const string ElasticsearchUrl = "http://localhost:9200";

        private readonly ElasticClient _elasticClient;

        public ProductService()
        {
            var elasticSettings = new ConnectionSettings(new Uri(ElasticsearchUrl));
            _elasticClient = new ElasticClient(elasticSettings);
        }

        public class ProductApiResponse
        {
            public List<Product> Products { get; set; }
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync("https://dummyjson.com/products");
                var apiResponse = JsonConvert.DeserializeObject<ProductApiResponse>(response);

                if (apiResponse != null && apiResponse.Products != null)
                {
                    return apiResponse.Products;
                }
                else
                {
                    return new List<Product>();
                }
            }
        }

        public void PublishProductsToRabbitMQ(List<Product> products)
        {
            var factory = new ConnectionFactory() { HostName = RabbitMQHost };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                foreach (var product in products)
                {
                    var message = JsonConvert.SerializeObject(product);
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "", routingKey: QueueName, basicProperties: null, body: body);
                    Console.WriteLine($"Published product: {product.Name}, {product.Price}, {product.Stock}");
                }
            }
        }

        public Product GetProductDetail(int productId)
        {
            var productFromApi = FetchProductFromApi(productId);
            return productFromApi;
        }

        public bool AddToBasket(BasketItem item)
        {
            var product = GetProductDetail(item.ProductId);
            if (product.Stock >= item.Quantity)
            {
                product.Stock -= item.Quantity;
                UpdateProductStock(item.ProductId, product.Stock);

                return true;
            }
            return false;
        }

        public bool MakePayment()
        {
            return true;
        }

        private Product FetchProductFromApi(int productId)
        {
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync($"https://dummyjson.com/products/{productId}").Result;

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return null;
                    }

                    throw new HttpRequestException($"HTTP request failed with status code: {response.StatusCode}");
                }

                var product = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);
                return product;
            }
        }

        public void UpdateProductStock(int productId, int newStock)
        {
            var product = FetchProductFromDatabase(productId);
            product.Stock = newStock;
            UpdateProductInDatabase(product);
        }

        private Product FetchProductFromDatabase(int productId)
        {
            // Bu metod, productId kullanarak veritabanından ürün bilgilerini çekebilir.
            // Örneğin, bir Entity Framework DbContext kullanabilirsiniz.
            // Bu örnekte varsayılan bir işlem gösterilmektedir, özel işlemlerinize göre uyarlamanız gerekebilir.
            return new Product { Id = productId, Name = $"Product {productId}", Price = 100, Stock = 10 };
        }

        private void UpdateProductInDatabase(Product product)
        {
            // Bu metod, ürünü veritabanında güncellemek için kullanılabilir.
            // Örneğin, bir Entity Framework DbContext kullanabilirsiniz.
            // Bu örnekte varsayılan bir işlem gösterilmektedir, özel işlemlerinize göre uyarlamanız gerekebilir.
            Console.WriteLine($"Updated product in database: {product.Name}, Stock: {product.Stock}");
        }
    }
}
