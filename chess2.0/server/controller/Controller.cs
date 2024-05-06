using Fleck;
using Newtonsoft.Json;

public class Controller
{
    public static string CreateRoom(IWebSocketConnection client)
    {
        var roomId = Guid.NewGuid().ToString();
        var gameRoom = Db.CreateRoom(roomId, client);
        var gameRoomDto = new GameRoomDto(gameRoom);
        var message = new Message(MessageType.Create, gameRoomDto, roomId);
        return JsonConvert.SerializeObject(message);
    }
}