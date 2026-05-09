using System.IO;
using System.Net.Sockets;
using System.Text;

public class ChatClient
{
    public TcpClient TcpClient { get; } // no set cz it;s only read and set only once
    public NetworkStream Stream { get; }
    public StreamWriter Writer { get; }
    public string? Username { get; set; }

    private readonly StreamReader _reader;

    public ChatClient(TcpClient client)
    {
        TcpClient = client;
        Stream = client.GetStream();
        Writer = new StreamWriter(Stream) { AutoFlush = true };
        _reader = new StreamReader(Stream);
    }

    public void Start()
    {
        while (true)
        {
            string? line = _reader.ReadLine();
            if (line == null)
                break;
            Console.WriteLine($"Message Received: {line}");

            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.StartsWith("/"))
            {
                //Console.WriteLine($"Received the //: {line}, it has a / as a command");
                string[] parts = line.Split(' ', 2);
                string command = parts[0].Substring(1);
                Console.WriteLine($"Debug command {command}");
                Writer.WriteLine("Command Received");
                switch (command)
                {
                    case "exit":
                        Console.WriteLine("The user has disconnected!");
                        TcpClient.Close();
                        break;

                    default:
                        //Console.WriteLine($"not a command. You can try {case "exit"}");
                        break;
                }
            }
            else
            {
                Writer.WriteLine("Message Received");
            }
        }
    }
}
