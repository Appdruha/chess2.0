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
    
    public static GameRoom? JoinRoom(string roomId, IWebSocketConnection client)
    {
        var gameRoomState = _rooms[roomId].JoinGameRoom(client);
        return gameRoomState;
    }
    
    public static GameRoom StartGame(string roomId)
    {
        var gameRoomState = _rooms[roomId].StartGame();
        return gameRoomState;
    }
}