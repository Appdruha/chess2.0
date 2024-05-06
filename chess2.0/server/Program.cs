using Fleck;
using Newtonsoft.Json;

var server = new WebSocketServer("ws://0.0.0.0:8181");

server.Start(ws =>
{
    ws.OnOpen = () =>
    {
        var m = new Message(MessageType.Create, null, "");
        Console.WriteLine(JsonConvert.SerializeObject(m));
        ws.Send("Websocket Connection open");
    };
    ws.OnMessage = messageString =>
    {
        try
        {
            var message = JsonConvert.DeserializeObject<Message>(messageString);
            if (message == null)
            {
                ws.Send("Incorrect message");
                return;
            }

            var type = message.Type;
            var roomId = message.RoomId;
            switch (type)
            {
                case MessageType.Create:
                {
                    var messageForClient = Controller.CreateRoom(ws);
                    ws.Send(messageForClient);
                    break;
                }
            }
        }
        catch
        {
            ws.Send("error");
        }
    };
});

WebApplication.CreateBuilder(args).Build().Run();