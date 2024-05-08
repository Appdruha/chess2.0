using Fleck;
using Newtonsoft.Json;

public class GameRoom
{
    public List<Cell> ChessBoardState { get; set; }
    public KingAttacker? KingAttacker { get; set; } = null;
    public FigureColors? Turn { get; set; } = null;
    public List<Player> Players { get; } = new List<Player>();

    public GameRoom(IWebSocketConnection connection)
    {
        Players.Add(new Player(FigureColors.WHITE, connection));
        ChessBoardState = ChessBoard.InitCells();
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
        ChessBoardState = ChessBoard.InitFigures(ChessBoardState);
        Turn = FigureColors.WHITE;
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
        ChessBoardState = gameRoom.ChessBoardState;
        Turn = gameRoom.Turn;
    }
}