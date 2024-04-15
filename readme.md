# Projenin İçeriği
C# dili ve ASP.NET platformunda https://dummyjson.com/products adresinden ürünleri multi-thread yapısıyla veritabanına aktaran bir kod yazdım. Multi-thread yapısını kullanmamdaki amaç; çok fazla ürün olması durumunda veri aktarımını hızlı bir şekilde gerçekleştirebilmek. Verileri RabbitMQ aracılığıyla veritabanına yazdım. Böylece veritabanını yormadan ve kayıp işlem olmasını engelleyerek data operasyonlarını asenkron biçimde gerçekleştirdim.

İçeriye aktarılan ürünleri listeleyen bir sayfa inşa ettim. Listedeki bir ürüne tıklayınca ürün detay sayfası açılıyor. Ürün detaylarının; uygulamanın verimini artırmak için Redis'in önbellekleme teknolojisi aracılığıyla getirilmesini sağladım. Eğer ürün detayı Redis'te cache'lenmemişse program veritabanından çekiyor, bu esnada Redis'e cache'leyerek bir  sonraki istekte bilgiyi Redis'ten çağırıyor.

Kullanıcı ürünü sepete atarken stok kontrolü yapılıyor. Ödeme için satın al butonu mevcut. Ödeme butonuna tıklayınca yeniden stok kontrolü yapılıyor ve yeterli stok varsa ödeme alınıyor.

# Not: Proje hâlâ yapım aşamasındadır.
