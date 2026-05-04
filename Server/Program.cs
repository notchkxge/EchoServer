/*
 * multi clietn instances connected to one server: done
 * */

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

class MyTcpListener
{
    public static async Task Main()
    {
        byte[] bytes = new byte[1024];
        TcpListener? server = null;
        try
        {
            Int32 port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            TcpListener tcpListener = new TcpListener(localAddr, port);

            tcpListener.Start();
            Console.WriteLine($"Server started, listening on port: {port}");

            while (true)
            {
                Console.WriteLine("Waiting for a connection......");
                var client = await tcpListener.AcceptTcpClientAsync();
                Console.WriteLine("Client Connected!!");

                HandleClientAsync(client);
                /*
                data = null!;
                NetworkStream stream = client.GetStream();

                int i;
                while ((i = await stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
                {
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);

                    //data = data.ToUpper();

                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                    await stream.WriteAsync(msg, 0, msg.Length);
                    //Console.WriteLine("Sent: {0}", data);
                }*/
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

    private static async void HandleClientAsync(TcpClient client)
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
