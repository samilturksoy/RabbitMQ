using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ();

//baglanti olustur
factory.Uri = new Uri("amqps://fwxftggn:KeOuBKBQW1kC7aXO0mefLOQOkjTI4pk0@sparrow.rmq.cloudamqp.com/fwxftggn");

// Baglanti aktiflestirme ve kanal acma
using IConnection connection = factory.CreateConnection ();
using IModel channel = connection.CreateModel();

//queve olusturma
channel.QueueDeclare(queue: "example-queve", exclusive: false);

//queve mesaj gonderme RabbitMQ kuyruğa atacağı mesajları byte türünden kabul etmektedir.

byte[] message = Encoding.UTF8.GetBytes("Merhaba");
channel.BasicPublish(exchange: "", routingKey: "example-queve", body: message);

Console.Read();