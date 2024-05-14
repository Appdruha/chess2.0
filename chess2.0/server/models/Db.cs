using Fleck;

public class Db
{
    static Dictionary<string, GameRoom> _rooms =
        new Dictionary<string, GameRoom>();

    public static void CreateRoom(string roomId, IWebSocketConnection client, GameMode mode)
    {
        var gameRoomState = new GameRoom(client, mode);
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
    
    public static GameRoom MoveFigure(string roomId, string moveParams)
    {
        var gameRoomState = _rooms[roomId].MoveFigure(moveParams);
        return gameRoomState;
    }
    
    public static GameRoom ChangeFigure(string roomId, string figureName)
    {
        var gameRoomState = _rooms[roomId].ChangeFigure(figureName);
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