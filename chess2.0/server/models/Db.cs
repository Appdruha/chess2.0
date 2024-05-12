using Fleck;

public class Db
{
    static Dictionary<string, GameRoom> _rooms =
        new Dictionary<string, GameRoom>();

    public static void CreateRoom(string roomId, IWebSocketConnection client)
    {
        var gameRoomState = new GameRoom(client);
        _rooms.Add(roomId, gameRoomState);
    }
    
    public static GameRoom? JoinRoom(string roomId, IWebSocketConnection client)
    {
        var gameRoomState = _rooms[roomId].JoinGameRoom(client);
        return gameRoomState;
    }
    
    public static GameRoom GetRoomState(string roomId)
    {
        var gameRoomState = _rooms[roomId];
        return gameRoomState;
    }
    
    public static GameRoom ChangeRoomState(string roomId, string moveParams)
    {
        var gameRoomState = _rooms[roomId].MoveFigure(moveParams);
        return gameRoomState;
    }
    
    public static GameRoom StartGame(string roomId)
    {
        var gameRoomState = _rooms[roomId].StartGame();
        return gameRoomState;
    }
    
    public static GameRoom RestartGame(string roomId)
    {
        var gameRoomState = _rooms[roomId].RestartGame();
        return gameRoomState;
    }
}