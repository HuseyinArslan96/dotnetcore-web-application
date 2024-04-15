# Projenin İçeriği
C# dili ve ASP.NET platformunda, https://dummyjson.com/products adresinden ürünleri JSON formatında multi-thread yapısıyla veritabanına aktaran bir kod yazdım. Multi-thread yapısını kullanmamdaki amaç; çok fazla ürün olması durumunda veri aktarımını hızlı bir şekilde gerçekleştirebilmek. Verileri RabbitMQ aracılığıyla veritabanına yazdım. Böylece veritabanını yormadan ve kayıp işlem olmasını engelleyerek data operasyonlarını asenkron biçimde gerçekleştirdim.

İçeriye aktarılan ürünleri listeleyen bir sayfa inşa ettim. Listedeki bir ürüne tıklayınca ürün detay sayfası açılıyor. Ürün detaylarının; uygulamanın verimini artırmak için Redis'in önbellekleme teknolojisi aracılığıyla getirilmesini sağladım. Eğer ürün detayı Redis'te cache'lenmemişse program veritabanından çekiyor, bu esnada Redis'e cache'leyerek bir  sonraki istekte bilgiyi Redis'ten çağırıyor.

Kullanıcı ürünü sepete atarken stok kontrolü yapılıyor. Ödeme için satın al butonu mevcut. Ödeme butonuna tıklayınca yeniden stok kontrolü yapılıyor ve yeterli stok varsa ödeme alınıyor.

# Not: Proje yapım aşamasındadır.

# Content of the Project
In C# language and ASP.NET platform, I wrote a code that transfers the products from https://dummyjson.com/products to the database in JSON format with a multi-thread structure. The purpose of using multi-thread structure is; to be able to transfer data quickly in case of a lot of products. I wrote the data to the database via RabbitMQ. Thus, I performed data operations asynchronously, without tiring the database and preventing lost transactions.

I built a page that lists imported products. When you click on a product in the list, the product detail page opens. Product details; I enabled Redis to be fetched through caching technology to increase the efficiency of the application. If the product detail is not cached in Redis, the program pulls it from the database, meanwhile caches it to Redis and calls the information from Redis on the next request.

Stock control is performed when the user puts the product into the cart. There is a buy button for payment. When you click on the payment button, stock is checked again and payment is taken if there is sufficient stock.

# Note: The project is under construction.
