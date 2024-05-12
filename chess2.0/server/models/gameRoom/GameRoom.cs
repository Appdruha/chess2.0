using Fleck;
using Newtonsoft.Json;

public class GameRoom
{
    public ChessBoard ChessBoard { get; }
    public FigureColors TurnColor { get; set; } = FigureColors.BLACK;
    public List<Player> Players { get; } = new List<Player>();
    public bool IsMate { get; set; }

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
        SetKingCells();

        TurnColor = FigureColors.WHITE;
        return this;
    }
    
    public GameRoom RestartGame()
    {
        IsMate = false;
        Players[0].Color = Players[0].Color == FigureColors.WHITE ? FigureColors.BLACK : FigureColors.WHITE;
        Players[1].Color = Players[1].Color == FigureColors.WHITE ? FigureColors.BLACK : FigureColors.WHITE;
        foreach (var cell in ChessBoard.ChessBoardState)
        {
            cell.SetFigure(null);
        }
        ChessBoard.InitFigures();
        SetKingCells();

        TurnColor = FigureColors.WHITE;
        return this;
    }

    public GameRoom MoveFigure(string moveParams)
    {
        var isWhiteTurn = TurnColor == FigureColors.WHITE;
        var whiteKingCell = Players.Find(player => player.Color == FigureColors.WHITE)!.KingCell;
        var blackKingCell = Players.Find(player => player.Color == FigureColors.BLACK)!.KingCell;
        var kingAttacker = FindKingAttacker(isWhiteTurn, whiteKingCell, blackKingCell);
        
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

        if (ChessBoard.CheckIsMate(
                FindKingAttacker(TurnColor == FigureColors.WHITE, whiteKingCell, blackKingCell),
                TurnColor, TurnColor == FigureColors.WHITE ? whiteKingCell : blackKingCell)
            )
        {
            IsMate = true;
        }
            
        return this;
    }

    public KingAttacker? FindKingAttacker(bool isWhiteTurn, Cell whiteKingCell, Cell blackKingCell)
    {
        return isWhiteTurn
            ? whiteKingCell.IsUnderAttack(ChessBoard.ChessBoardState, FigureColors.WHITE)
            : blackKingCell.IsUnderAttack(ChessBoard.ChessBoardState, FigureColors.BLACK);
    }

    public void SetKingCells()
    {
        foreach (var cell in ChessBoard.ChessBoardState)
        {
            if (cell.Figure != null && cell.Figure.Name == FigureNames.KING)
            {
                var king = (King)cell.Figure;
                if (king.Color == FigureColors.WHITE)
                {
                    king.IsMyTurn = true;
                    Players.Find(player => player.Color == FigureColors.WHITE)!.KingCell = cell;
                }
                else
                {
                    king.IsMyTurn = false;
                    Players.Find(player => player.Color == FigureColors.BLACK)!.KingCell = cell;
                }
            }
        }
    }
}

public class GameRoomDto
{
    [JsonProperty("chessBoardState")] public List<Cell> ChessBoardState { get; set; }
    [JsonProperty("turn")] public bool Turn { get; set; }
    [JsonProperty("color")] public FigureColors? Color { get; set; }

    public GameRoomDto(GameRoom gameRoom, FigureColors playerColor)
    {
        ChessBoardState = playerColor == FigureColors.WHITE 
            ? gameRoom.ChessBoard.ChessBoardState 
            : gameRoom.ChessBoard.GetReversedBoard();
        
        Turn = gameRoom.TurnColor == playerColor;
        Color = gameRoom.TurnColor;
    }
}