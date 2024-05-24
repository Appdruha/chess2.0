using chess2._0.models.gameRoom;
using Fleck;

namespace chess2._0.models;

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
    
    public static GameRoom? Leave(string roomId, IWebSocketConnection client)
    {
        var gameRoomState = _rooms[roomId].Leave(client);
        if (gameRoomState.Players.Count == 0)
        {
            _rooms.Remove(roomId);
            return null;
        }
        return gameRoomState;
    }
    
    public static GameRoom GetRoomState(string roomId)
    {
        var gameRoomState = _rooms[roomId];
        return gameRoomState;
    }
    
    public static GameRoom GiveUp(string roomId, GameWinner winner)
    {
        _rooms[roomId].Winner = winner;
        return _rooms[roomId];
    }
    
    public static GameRoom ConfirmDraw(string roomId)
    {
        _rooms[roomId].Winner = GameWinner.Draw;
        return _rooms[roomId];
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