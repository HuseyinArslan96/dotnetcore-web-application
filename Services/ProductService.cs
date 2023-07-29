using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using StackExchange.Redis;
using Newtonsoft.Json;
using Elasticsearch.Net;
using Nest;
using WebApplication.Models;
using Newtonsoft.Json.Linq;

namespace WebApplication.Services
{
    public class ProductService
    {
        private const string RabbitMQHost = "localhost";
        private const string QueueName = "product_queue";
        private const string ElasticsearchUrl = "http://localhost:9200";
        private const string RedisCacheKeyPrefix = "product_";

        private readonly ElasticClient _elasticClient;
        private readonly ConnectionMultiplexer _redisConnection;

        public ProductService()
        {
            var elasticSettings = new ConnectionSettings(new Uri(ElasticsearchUrl));
            _elasticClient = new ElasticClient(elasticSettings);
            _redisConnection = ConnectionMultiplexer.Connect("localhost");
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync("https://dummyjson.com/products");
                return JsonConvert.DeserializeObject<List<Product>>(response);
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
            var cache = _redisConnection.GetDatabase();
            var cacheKey = RedisCacheKeyPrefix + productId;

            var cachedProduct = cache.StringGet(cacheKey);
            if (!cachedProduct.IsNull)
            {
                return JsonConvert.DeserializeObject<Product>(cachedProduct);
            }

            var productFromDatabase = FetchProductFromDatabase(productId);
            cache.StringSet(cacheKey, JsonConvert.SerializeObject(productFromDatabase));
            return productFromDatabase;
        }

        public bool AddProductToBasket(BasketItem item)
        {
            var product = GetProductDetail(item.ProductId);
            if (product.Stock >= item.Quantity)
            {
                return true;
            }

            return false;
        }

        public bool MakePayment()
        {
            
            return true;
        }

        private Product FetchProductFromDatabase(int productId)
        {
            
            return new Product { Id = productId, Name = $"Product {productId}", Price = 100, Stock = 50 };
        }
    }
}