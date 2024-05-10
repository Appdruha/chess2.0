using Fleck;
using Newtonsoft.Json;

public class GameRoom
{
    public ChessBoard ChessBoard { get; set; }
    public KingAttacker? KingAttacker { get; set; } = null;
    public FigureColors? Turn { get; set; } = null;
    public List<Player> Players { get; } = new List<Player>();

    public GameRoom(IWebSocketConnection connection)
    {
        Players.Add(new Player(FigureColors.WHITE, connection));
        ChessBoard = new ChessBoard();
    }

    public GameRoom? JoinGameRoom(IWebSocketConnection connection)
    {
        if (Players.Count == 2)
        {
            return null;
        }

        Players.Add(new Player(FigureColors.BLACK, connection));
        return this;
    }

    public GameRoom StartGame()
    {
        ChessBoard.InitFigures();
        Turn = FigureColors.WHITE;
        return this;
    }
    
    public GameRoom MoveFigure(string moveParams)
    {
        ChessBoard.MoveFigure(moveParams);
        Turn = Turn == FigureColors.WHITE ? FigureColors.BLACK : FigureColors.WHITE;
        return this;
    }
}

public class GameRoomDto
{
    [JsonProperty("chessBoardState")] 
    public List<Cell> ChessBoardState { get; set; }
    [JsonProperty("turn")] 
    public FigureColors? Turn { get; set; }

    public GameRoomDto(GameRoom gameRoom)
    {
        ChessBoardState = gameRoom.ChessBoard.ChessBoardState;
        Turn = gameRoom.Turn;
    }
}