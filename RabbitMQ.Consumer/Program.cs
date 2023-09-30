//Bağlantı Oluşturma
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
//factory.Uri = new Uri("..");cloud url kullanılacaksa
factory.HostName = "localhost";//docker üzerinden 
// Bağlantı Aktifleştirme ve Kanal Açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Queve oluşturma 
channel.QueueDeclare(queue: "example-queve", exclusive: false,durable : true);

//Queve mesaj okuma
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "example-queve", autoAck: false, consumer);
//tüm consumerlar eşit miktarda çalışşın ve veri işlesin
channel.BasicQos(0, 1, false);
consumer.Received += (sender, e) =>
{
    //kuyruğa gelen mesajın işlediğim yer
    //e.Body : Kuyruktaki mesajın verisini bütünsel olarak getirecektir
    //e.Body.Span veya e.Body.ToArray() : Kuyruktaki mesajın byte verisini getirecektir.
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));

    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false); //Kuyruktaki mesajı okuduğunu işlediğini publishera bildirmek için kullanılır ki kuyruktan message silinsin
};

Console.Read();