/*
 * multi clietn instances connected to one server: done
 * */

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static async Task Main()
    {
        try
        {
            ChatServer server = new ChatServer();
            await server.StartAsync(13000, IPAddress.Parse("127.0.0.1"));
        }
        catch (Exception e)
        {
            Console.WriteLine($"Fatal error: {e}");
        }
    }
}
