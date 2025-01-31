using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocketSharp;



class Program{

  static void Main(string[] args){

    // create a instance of a websocket client
    WebSocket ws = new WebSocket("ws://simple-websocket-server-echo.glitch.me/");


    ws.Connect();
    ws.Send("Hello server");

    Console.ReadKey();
  }
}
