using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.HostName = "localhost";

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//1.Adım
channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

//2.Adım
string queveName = channel.QueueDeclare().QueueName;

//3.Adım
channel.QueueBind(queue: queveName,
                 exchange: "direct-exchange-example",
                 routingKey : "direct-queve-example");

EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

channel.BasicConsume(consumer: consumer,queue : queveName,autoAck : true);
consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};
Console.Read();


//1. Adım : Publisherdaki 1e1 aynı isiml ve type'a sahip bir exchange tanımlanmalıdır !
//2.Adım : Publisher tarafından router keyde bulunan değerdeki kuyruğa gönderilen mesajları kendi oluşturduğumuz
//kuyruğa yönlendirerek tüketmemiz gerekmektedir Bunun için öncelikle bir kuyruk oluşturulmalıdır.