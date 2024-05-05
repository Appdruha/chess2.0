using WebSocketSharp;
using WebSocketSharp.Server;

public class WebSocketServer
{
    private readonly int _port;
    private readonly Dictionary<string, List<WebSocket>> _rooms;

    public WebSocketServer(int port)
    {
        _port = port;
        _rooms = new Dictionary<string, List<WebSocket>>();
    }

    public void Start()
    {
        var server = new WebSocketServer(_port);
        server.AddWebSocketService<MyWebSocketHandler>("/Api");
        server.Start();
        Console.WriteLine("Server started on port 5000");
    }

    public void JoinRoom(WebSocket ws, string roomId)
    {
        if (!_rooms.ContainsKey(roomId))
        {
            _rooms[roomId] = new List<WebSocket>();
        }

        if (_rooms[roomId].Count >= 2)
        {
            ws.Send("Комната переполнена");
            return;
        }

        _rooms[roomId].Add(ws);
        ws.OnClose += (sender, e) => LeaveRoom(ws, roomId);
        ws.OnMessage += (sender, e) => HandleMessage(ws, roomId, e.Data);
    }

    public void LeaveRoom(WebSocketConnection ws, string roomId)
    {
        if (!_rooms.ContainsKey(roomId))
        {
            return;
        }

        _rooms[roomId].Remove(ws);
        if (_rooms[roomId].Count == 0)
        {
            _rooms.Remove(roomId);
        }
    }

    public void HandleMessage(WebSocketConnection ws, string roomId, string message)
    {
        // Handle the message here
    }

    private class MyWebSocketHandler : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            var roomId = Guid.NewGuid().ToString();
            JoinRoom(this, roomId);
            Send(roomId);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            HandleMessage(this, Room, e.Data);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            LeaveRoom(this, Room);
        }
    }
}

// WebSocketServer wss = new WebSocketServer("ws://localhost:5000");
// wss.AddWebSocketService<Api>("/Api");
//
// wss.Start();
// Console.WriteLine("Server started on port 5000");
//
// public class Api : WebSocketBehavior
// {
//     protected override void OnMessage(MessageEventArgs e)
//     {
//         Sessions.;
//     }
// }