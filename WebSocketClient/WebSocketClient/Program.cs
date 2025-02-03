using System.Net.WebSockets;
using System.Text;


class Program{

  static async Task Main(){

    string Message = "Hello Server!";
    byte[] emsg = Encoding.UTF8.GetBytes(Message);
    ArraySegment<byte> smsg = new(emsg);

    byte[] res = new byte[1024];

    Uri server = new Uri("ws://echo.websocket.org/");
    ClientWebSocket ws = new ClientWebSocket();

    await ws.ConnectAsync(server, default);
    await ws.SendAsync(smsg, WebSocketMessageType.Text, true, CancellationToken.None);

    var bres = await ws.ReceiveAsync(new ArraySegment<byte>(res), CancellationToken.None);
    string resultMessage = Encoding.UTF8.GetString(res, 0, bres.Count);

    Console.WriteLine(resultMessage);

    Console.WriteLine("Skickat");

    Console.ReadKey();


  }
}
