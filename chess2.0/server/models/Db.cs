using Fleck;

public class Db
{
    static Dictionary<string, GameRoom> _rooms =
        new Dictionary<string, GameRoom>();

    public static GameRoom CreateRoom(string roomId, IWebSocketConnection client)
    {
        var gameRoomState = new GameRoom(client);
        _rooms.Add(roomId, gameRoomState);
        return gameRoomState;
    }
    
    
}