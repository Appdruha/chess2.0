using Fleck;
using Newtonsoft.Json;

var server = new WebSocketServer("ws://0.0.0.0:8181");

server.Start(ws =>
{
    ws.OnOpen = () =>
    {
        var m = new MessageToClient(MessageType.Create, null, "");
        Console.WriteLine(JsonConvert.SerializeObject(m));
        ws.Send("Websocket Connection open");
    };
    ws.OnMessage = messageString =>
    {
        try
        {
            Console.WriteLine(messageString);
            var message = JsonConvert.DeserializeObject<MessageFromClient>(messageString);
            if (message == null)
            {
                ws.Send("Incorrect message");
                return;
            }

            var type = message.Type;
            var clientParams = message.Params;
            var roomId = message.RoomId;
            switch (type)
            {
                case MessageType.Create:
                {
                    var messageForClient = Controller.CreateRoom(ws);
                    ws.Send(messageForClient);
                    break;
                }
                case MessageType.Join:
                {
                    var messageForClient = Controller.JoinRoom(ws, roomId);
                    ws.Send(messageForClient);
                    break;
                }
                case MessageType.Init:
                {
                    var messageForClient = Controller.Init(roomId);
                    ws.Send(messageForClient);
                    break;
                }
                case MessageType.Start:
                {
                    var (players, messageForClient) = Controller.Start(roomId);
                    WSActions.Broadcast(players, messageForClient);
                    break;
                }
                case MessageType.Move:
                {
                    var (players, messageForClient) = Controller.Move(roomId, clientParams);
                    WSActions.Broadcast(players, messageForClient);
                    break;
                }
            }
        }
        catch(Exception e)
        {
            ws.Send(e.ToString());
        }
    };
});

WebApplication.CreateBuilder(args).Build().Run();

public class WSActions
{
    public static void Broadcast(List<Player> players, string message)
    {
        foreach (var player in players)
        {
            player.Connection.Send(message);
        }
    }
}