using RabbitMQ.Client;
using System.Text;



//baglanti olustur
ConnectionFactory factory = new();
//factory.Uri = new Uri("..."); cloud url kullanılacaksa
factory.HostName = "localhost"; //docker üzerinden 
// Baglanti aktiflestirme ve kanal acma
using IConnection connection = factory.CreateConnection ();
using IModel channel = connection.CreateModel();

//queve olusturma
// RabitMQ sunucusu çökerse kuruktaki tüm messagelar kayboluyor bunu önleme için öncelikle 1.yapılandırma QueueDeclare dyrable:true değerini veriyoruz
channel.QueueDeclare(queue: "example-queve", exclusive: false, durable : true);

//queve mesaj gonderme RabbitMQ kuyruğa atacağı mesajları byte türünden kabul etmektedir.

//byte[] message = Encoding.UTF8.GetBytes("Merhaba");
//channel.BasicPublish(exchange: "", routingKey: "example-queve", body: message);

//RabitMQ sunucusu çökerse kuruktaki tüm messagelar kayboluyor bunu önleme için 2.yapılandırma properties oluşturuluyor.
IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent = true;
for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);
    channel.BasicPublish(exchange: "", routingKey: "example-queve", body: message,basicProperties:properties);
}

Console.Read();