using System.Net.WebSockets;
using System.Text;
using Microsoft.VisualBasic;

/* Källor:
1. https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-open-and-append-to-a-log-file
2. https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-write-text-to-a-file
3. https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-datetime#assign-a-computed-value
*/

/* Att Göra

    user = <ditt namn> när du skickar meddelande
    user = echo när du tar emot meddelande från echo-servern
    logfilens namn ska vara chat.log

    * Implementera funktionen log
    * Implementera handleMessage (båda funktions-kroppen och -huvud)
        - Skriver ut det mottagna meddelandet till konsolen.
        - Anropar log-funktionen med lämplig parametrar.
    * Modifiera Main
        - Anropa handleMessage i samband med att meddelande skickats och tas emot.
    
*/
class WebSocketClient
{
    static void handleMessage(string msg, string user){
        // funktionen saknar både funktionskropp och funktionshuvud (dvs nödvändiga parametrar)

        
        using (StreamWriter w = File.AppendText("chat.log")){
            log(msg, user, w);
            Console.WriteLine($"| {DateTime.Now.ToLongTimeString()} | {user} | {msg} |");
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
        handleMessage(message, "Samuel");
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }
    public static async Task Main()
    {
        string serverUri = "wss://echo.websocket.org";
        ClientWebSocket client = new ClientWebSocket();
        await client.ConnectAsync(new Uri(serverUri), CancellationToken.None);
        byte[] receiveBuffer = new byte[1024];
        while (client.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
           
            if (result.MessageType == WebSocketMessageType.Text)
            {
                string receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
                handleMessage(receivedMessage, "Echo");
                await Send(client);
            }
        }
    }
}