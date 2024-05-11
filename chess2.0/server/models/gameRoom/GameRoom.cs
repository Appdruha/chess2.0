using Fleck;
using Newtonsoft.Json;

public class GameRoom
{
    public ChessBoard ChessBoard { get; set; }
    public KingAttacker? KingAttacker { get; set; } = null;
    public FigureColors? TurnColor { get; set; }
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
        foreach (var cell in ChessBoard.ChessBoardState)
        {
            if (cell.Figure != null && cell.Figure.Name == FigureNames.KING)
            {
                var king = (King)cell.Figure;
                if (king.Color == FigureColors.WHITE)
                {
                    king.IsMyTurn = true;
                    Players[0].KingCell = cell;
                }
                else
                {
                    king.IsMyTurn = false;
                    Players[1].KingCell = cell;
                }
            }
        }

        TurnColor = FigureColors.WHITE;
        return this;
    }

    public GameRoom MoveFigure(string moveParams)
    {
        var isWhiteTurn = TurnColor == FigureColors.WHITE;
        var whiteKingCell = Players[0].KingCell;
        var blackKingCell = Players[1].KingCell;
        var kingAttacker = isWhiteTurn
            ? whiteKingCell.IsUnderAttack(ChessBoard.ChessBoardState, FigureColors.WHITE)
            : blackKingCell.IsUnderAttack(ChessBoard.ChessBoardState, FigureColors.BLACK);
        
        var (newKingCell, toggleTurn) = 
            ChessBoard.MoveFigure(moveParams, kingAttacker, isWhiteTurn ? whiteKingCell : blackKingCell);
        
        if (newKingCell != null)
        {
            var player = isWhiteTurn ? Players[0] : Players[1];
            player.KingCell = newKingCell;
        }

        if (toggleTurn)
        {
            var whiteKing = (King?)Players[0].KingCell.Figure;
            var blackKing = (King?)Players[1].KingCell.Figure;
            if (whiteKing != null && blackKing != null)
            {
                whiteKing.IsMyTurn = !whiteKing.IsMyTurn;
                blackKing.IsMyTurn = !blackKing.IsMyTurn;
            }
            TurnColor = isWhiteTurn ? FigureColors.BLACK : FigureColors.WHITE;
        }

        return this;
    }
}

public class GameRoomDto
{
    [JsonProperty("chessBoardState")] public List<Cell> ChessBoardState { get; set; }
    [JsonProperty("turn")] public bool Turn { get; set; }
    [JsonProperty("color")] public FigureColors? Color { get; set; }

    public GameRoomDto(GameRoom gameRoom, FigureColors playerColor)
    {
        ChessBoardState = gameRoom.ChessBoard.ChessBoardState;
        Turn = gameRoom.TurnColor == playerColor;
        Color = gameRoom.TurnColor;
    }
}