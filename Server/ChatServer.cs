/* client username before their message:
 *
 *
 */

using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class ChatServer
{
    private readonly ConcurrentDictionary<string, ChatClient> clients = new();

    public async Task StartAsync(int port, IPAddress localAddr)
    {
        TcpListener tcpListener = new TcpListener(localAddr, port);

        tcpListener.Start();
        Console.WriteLine($"Server started, listening on port: {port}");
        Console.WriteLine("Waiting for a connection......");

        while (true)
        {
            var client = await tcpListener.AcceptTcpClientAsync();
            Console.WriteLine("Client Connected!!");

            _ = HandleClientAsync(client);
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        NetworkStream stream = client.GetStream(); //send a receive msg in bytes
        string? data;

        try
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int byteRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                data = Encoding.UTF8.GetString(buffer, 0, byteRead);
                Console.WriteLine($"Received: {data}");

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                await stream.WriteAsync(msg, 0, data.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            client.Close();
            Console.WriteLine("Client disconnected!");
        }
    }
}
