using System.Net.WebSockets;
using System.Text;


class WebSocketClient
{
    static void handleMessage(string msg, string user, bool write){


        if (write){
        using (StreamWriter w = File.AppendText("chat.log")){
            log(msg, user, w);
            Console.WriteLine($"| {DateTime.Now.ToLongTimeString()} | {user} | {msg} |");
        }
    }
}
    static void log(string msg,string user,StreamWriter w){
        // Funktionen ska skriva: <nuvarande klockslag> <user> | <msg> till en logfil som definieras av w.
        // Se exempel i filen log.example
        w.WriteLine($"| {DateTime.Now.ToLongTimeString()} | {user} | {msg} |");
    }
    static async Task Send(ClientWebSocket client){
        Console.WriteLine("Skriv in ett meddelande att skicka:");
        string message = Console.ReadLine();
        handleMessage(message, "Samuel", true);
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }
    public static async Task Main()
    {
        string serverUri = "wss://echo.websocket.org";
        ClientWebSocket client = new ClientWebSocket();
        await client.ConnectAsync(new Uri(serverUri), CancellationToken.None);
        byte[] receiveBuffer = new byte[1024];


        bool ServerMessage = false;
        Console.Clear();
        while (client.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

            
            if (result.MessageType == WebSocketMessageType.Text)
            {
                string receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
                handleMessage(receivedMessage, "Echo", ServerMessage);
                await Send(client);
                ServerMessage = true;
            }
        }
    }
}