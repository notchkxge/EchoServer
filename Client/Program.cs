using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string Server = "127.0.0.1";
        int port = 13000;

        try
        {
            using TcpClient client = new TcpClient();
            await client.ConnectAsync(Server, port);
            Console.WriteLine("Connected to server.");

            NetworkStream stream = client.GetStream();

            while (true)
            {
                Console.WriteLine("Enter message (or 'exit'):");

                string? message = Console.ReadLine();
                if (string.IsNullOrEmpty(message) || message.ToLower() == "exit")
                    return;

                byte[] data = Encoding.ASCII.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length);
                Console.WriteLine($"Sent: {message}");

                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string reply = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received: {reply}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine(ex.ToString());
        }
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
