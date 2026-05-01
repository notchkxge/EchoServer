using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

class MyTcpListener
{
    public static async Task Main()
    {
        TcpListener? server = null;
        try
        {
            Int32 port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            server = new TcpListener(localAddr, port);

            server.Start();

            byte[] bytes = new byte[1024];
            string data = null!;

            while (true)
            {
                Console.WriteLine("Waiting for a connection......");
                using TcpClient client = await server.AcceptTcpClientAsync();
                Console.WriteLine("Connected!!");

                data = null!;
                NetworkStream stream = client.GetStream();

                int i;
                while ((i = await stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
                {
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);

                    data = data.ToUpper();

                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                    await stream.WriteAsync(msg, 0, msg.Length);
                    Console.WriteLine("Sent: {0}", data);
                }
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
        finally
        {
            server?.Stop();
        }
        Console.WriteLine("\n Hit enter to continue...");
        Console.ReadKey();
    }
}
