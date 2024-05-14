using Fleck;
using Newtonsoft.Json;

public class GameRoom
{
    public ChessBoard ChessBoard { get; set; }
    public GameMode Mode { get; set; }
    public int TurnsToNarrowing { get; set; } = 16;
    public int CurrentTurn { get; set; } = 0;
    public int NarrowingCount { get; set; } = 0;
    public Cell? ChangingFigureCell { get; set; }
    public FigureColors TurnColor { get; set; } = FigureColors.BLACK;
    public List<Player> Players { get; } = new List<Player>();
    public GameWinner? Winner { get; set; }

    public GameRoom(IWebSocketConnection connection, GameMode mode)
    {
        Players.Add(new Player(FigureColors.WHITE, connection));
        Mode = mode;
        ChessBoard = new ChessBoard(mode);
        Winner = null;
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
    
    public GameRoom Leave(IWebSocketConnection connection)
    {
        var player = Players.Find(player => player.Connection == connection);
        if (player != null)
        {
            Players.Remove(player);
        }
        return this;
    }

    public GameRoom StartGame()
    {
        ChessBoard.InitFigures(Mode);
        SetKingCells();

        TurnColor = FigureColors.WHITE;
        CurrentTurn = 1;
        return this;
    }

    public GameRoom RestartGame()
    {
        Winner = null;
        Players[0].Color = Players[0].Color == FigureColors.WHITE ? FigureColors.BLACK : FigureColors.WHITE;
        Players[1].Color = Players[1].Color == FigureColors.WHITE ? FigureColors.BLACK : FigureColors.WHITE;
        ChessBoard = new ChessBoard(Mode);

        ChessBoard.InitFigures(Mode);
        SetKingCells();
        NarrowingCount = 0;
        TurnColor = FigureColors.WHITE;
        CurrentTurn = 1;
        return this;
    }

    public GameRoom ChangeFigure(string figureName)
    {
        ChessBoard.ChangeFigure(figureName, ChangingFigureCell!);
        ChangingFigureCell = null;

        var whiteKingCell = Players.Find(player => player.Color == FigureColors.WHITE)!.KingCell;
        var blackKingCell = Players.Find(player => player.Color == FigureColors.BLACK)!.KingCell;

        if (ChessBoard.CheckIsMate(
                FindKingAttacker(TurnColor == FigureColors.WHITE, whiteKingCell, blackKingCell),
                TurnColor, TurnColor == FigureColors.WHITE ? whiteKingCell : blackKingCell)
           )
        {
            CurrentTurn -= 1;
            if (TurnColor == FigureColors.WHITE)
            {
                Winner = GameWinner.Black;
            } else
            {
                Winner = GameWinner.White;
            }
        }

        if (Mode == GameMode.Chess20)
        {
            Winner = ChessBoard.DoNarrowing(NarrowingCount);
            NarrowingCount += 1;
        }
        
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
            CurrentTurn += 1;
        }

        if (ChessBoard.CheckIsMate(
                FindKingAttacker(TurnColor == FigureColors.WHITE, whiteKingCell, blackKingCell),
                TurnColor, TurnColor == FigureColors.WHITE ? whiteKingCell : blackKingCell)
           )
        {
            if (TurnColor == FigureColors.WHITE)
            {
                Winner = GameWinner.Black;
            } else
            {
                Winner = GameWinner.White;
            }
            CurrentTurn -= 1;
        }
        
        CheckPlayerAbilityToChangeFigure();

        if (Mode == GameMode.Chess20 && (CurrentTurn - 1) % TurnsToNarrowing == 0 && CurrentTurn != 1 && ChangingFigureCell == null)
        {
            Winner = ChessBoard.DoNarrowing(NarrowingCount);
            NarrowingCount += 1;
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

    public void CheckPlayerAbilityToChangeFigure()
    {
        var firstYTrigger = 9 - NarrowingCount;
        var secondYTrigger = NarrowingCount;
        foreach (var cell in ChessBoard.ChessBoardState)
        {
            if (cell.Figure != null && 
                ((cell.Y == (Mode == GameMode.Chess20 ? secondYTrigger : 0) && cell.Figure.Color == FigureColors.WHITE) || 
                 (cell.Y == (Mode == GameMode.Chess20 ? firstYTrigger : 7) && cell.Figure.Color == FigureColors.BLACK)) &&
                cell.Figure.Name == FigureNames.PAWN)
            {
                ChangingFigureCell = cell;
            }
        }
    }
}

public enum GameMode
{
    CommonChess,
    Chess20
}

public enum GameWinner
{
    White,
    Black,
    Draw
}

public class GameRoomDto
{
    [JsonProperty("chessBoardState")] public List<Cell> ChessBoardState { get; set; }
    [JsonProperty("isMyTurn")] public bool IsMyTurn { get; set; }
    [JsonProperty("turn")] public int Turn { get; set; }
    [JsonProperty("winner")] public GameWinner? Winner { get; set; }
    [JsonProperty("color")] public FigureColors? Color { get; set; }

    public GameRoomDto(GameRoom gameRoom, FigureColors playerColor)
    {
        ChessBoardState = playerColor == FigureColors.WHITE
            ? gameRoom.ChessBoard.ChessBoardState
            : gameRoom.ChessBoard.GetReversedBoard(gameRoom.Mode);

        IsMyTurn = gameRoom.TurnColor == playerColor;
        Color = gameRoom.TurnColor;
        Winner = gameRoom.Winner;
        Turn = gameRoom.CurrentTurn;
    }
}