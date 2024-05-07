using Fleck;
using Newtonsoft.Json;

public class Controller
{
    public static string CreateRoom(IWebSocketConnection client)
    {
        var roomId = Guid.NewGuid().ToString();
        var gameRoom = Db.CreateRoom(roomId, client);
        return CreateJsonMessage(MessageType.Create, gameRoom, roomId);
    }
    
    public static string JoinRoom(IWebSocketConnection client, string roomId)
    {
        var gameRoom = Db.JoinRoom(roomId, client);
        if (gameRoom == null)
        {
            return CreateJsonMessage(MessageType.Error, gameRoom, roomId);
        }
        return CreateJsonMessage(MessageType.Join, gameRoom, roomId);
    }
    
    public static (List<Player>, string) Start(string roomId)
    {
        var gameRoom = Db.StartGame(roomId);
        return (gameRoom.Players, CreateJsonMessage(MessageType.Start, gameRoom, roomId));
    }

    private static string CreateJsonMessage(MessageType type, GameRoom? gameRoom, string roomId)
    {
        if (gameRoom == null)
        {
            var errorMessage = new Message(type, null, roomId);
            return JsonConvert.SerializeObject(errorMessage);
        }
        var gameRoomDto = new GameRoomDto(gameRoom);
        var message = new Message(type, gameRoomDto, roomId);
        return JsonConvert.SerializeObject(message);
    }
}