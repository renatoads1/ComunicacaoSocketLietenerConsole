using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ExemploTcpListener
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                // Set TcpListener na port 5777.
                Int32 port = 5777;
                IPAddress localAddr = IPAddress.Parse("172.20.0.138");
                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);
                // Start listening for client requests.
                server.Start();
                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;
                while (true)
                {
                    Console.Write("Aguardando por conexão... ");
                    // Realiza uma chamada de bloqueio para aceitar solicitações.
                    // Você também pode usar server.AcceptSocket() aqui.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Conectado!");
                    data = null;
                    // Obtém um objeto stream para leitura e escrita
                    NetworkStream stream = client.GetStream();
                    
                    int i;
                    // Loop para receber todos os dados enviados pelo cliente.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Traduza bytes de dados para uma string ASCII.
                        data = Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Recebido: {0}", data);

                        // Processe os dados enviados pelo cliente.
                        data = data.ToUpper();

                        byte[] msg = Encoding.ASCII.GetBytes(data);

                        // Envie de volta uma resposta.
                        stream.Write(msg, 0, msg.Length);

                        Console.WriteLine("Enviado: {0}", data);
                    }
                    client.Close();
                }

            }
            catch (Exception exep)
            {

                Console.WriteLine("SocketException: {0}", exep.Message);
            }
            finally
            {
                server.Stop();
            }
            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}
