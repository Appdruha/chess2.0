using Fleck;
using Newtonsoft.Json;

public class Controller
{
    public static string CreateRoom(IWebSocketConnection client)
    {
        var roomId = Guid.NewGuid().ToString();
        Db.CreateRoom(roomId, client);
        return CreateJsonMessage(MessageType.Create, null, roomId);
    }

    public static string JoinRoom(IWebSocketConnection client, string roomId)
    {
        var gameRoom = Db.JoinRoom(roomId, client);
        if (gameRoom == null)
        {
            return CreateJsonMessage(MessageType.Error, gameRoom, roomId);
        }

        return CreateJsonMessage(MessageType.Join, null, roomId);
    }

    public static string Init(string roomId)
    {
        var gameRoom = Db.GetRoomState(roomId);
        return CreateJsonMessage(MessageType.Init, gameRoom, roomId);
    }

    public static (List<Player>, string) Start(string roomId)
    {
        var gameRoom = Db.StartGame(roomId);
        return (gameRoom.Players, CreateJsonMessage(MessageType.Start, gameRoom, roomId));
    }
    
    public static (List<Player>, string) Move(string roomId, string moveParams)
    {
        var gameRoom = Db.ChangeRoomState(roomId, moveParams);
        return (gameRoom.Players, CreateJsonMessage(MessageType.NextTurn, gameRoom, roomId));
    }

    private static string CreateJsonMessage(MessageType type, GameRoom? gameRoom, string roomId)
    {
        GameRoomDto? gameRoomDto = null;
        if (gameRoom != null)
        {
            gameRoomDto = new GameRoomDto(gameRoom);
        }

        var message = new MessageToClient(type, gameRoomDto, roomId);
        return JsonConvert.SerializeObject(message);
    }
}