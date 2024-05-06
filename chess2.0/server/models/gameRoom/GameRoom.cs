using Fleck;

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
    
    public void JoinGameRoom(IWebSocketConnection connection)
    {
        var chessBoard = ChessBoardState;
        Players.Add(new Player(FigureColors.BLACK, connection));
        ChessBoardState = ChessBoard.InitFigures(ChessBoardState);
        return;
    }
}

public class GameRoomDto
{
    public List<Cell> ChessBoardState { get; set; }
    public KingAttacker? KingAttacker { get; set; }
    public FigureColors? Turn { get; set; }
    
    public GameRoomDto(GameRoom gameRoom)
    {
        ChessBoardState = gameRoom.ChessBoardState;
        KingAttacker = gameRoom.KingAttacker;
        Turn = gameRoom.Turn;
    }
}