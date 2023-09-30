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
channel.QueueDeclare(queue: "example-queve", exclusive: false);

//queve mesaj gonderme RabbitMQ kuyruğa atacağı mesajları byte türünden kabul etmektedir.

//byte[] message = Encoding.UTF8.GetBytes("Merhaba");
//channel.BasicPublish(exchange: "", routingKey: "example-queve", body: message);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);
    channel.BasicPublish(exchange: "", routingKey: "example-queve", body: message);
}

Console.Read();