public class ChessBoard
{
    public List<Cell> ChessBoardState;

    public ChessBoard()
    {
        var cellColor = CellCollors.BLACK;
        var chessBoard = new List<Cell>();
        var columns = "ABCDEFGH";
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                var x = i;
                var y = 7 - j;
                chessBoard.Add(new Cell(x, y, cellColor, columns[i] + (j + 1).ToString()));
                if (j != 7)
                {
                    cellColor = cellColor == CellCollors.WHITE ? CellCollors.BLACK : CellCollors.WHITE;
                }
            }
        }

        ChessBoardState = chessBoard;
    }

    public List<Cell> GetReversedBoard()
    {
        var reversedBoard = new List<Cell>();
        foreach (var cell in ChessBoardState)
        {
            var figure = cell.Figure;
            var newCell = new Cell(cell.X, Math.Abs(cell.Y - 7), cell.Color, cell.Id);
            newCell.SetFigure(figure);
            reversedBoard.Add(newCell);
        }

        return reversedBoard;
    }

    public void InitFigures()
    {
        foreach (var cell in ChessBoardState)
        {
            if (cell.Id.Contains('2'))
            {
                cell.SetFigure(new Pawn(FigureColors.WHITE, cell));
            }

            if (cell.Id == "E1")
            {
                cell.SetFigure(new King(FigureColors.WHITE, cell));
            }

            if (cell.Id == "D1")
            {
                cell.SetFigure(new Queen(FigureColors.WHITE, cell));
            }

            if (cell.Id == "F1" || cell.Id == "C1")
            {
                cell.SetFigure(new Bishop(FigureColors.WHITE, cell));
            }

            if (cell.Id == "A1" || cell.Id == "H1")
            {
                cell.SetFigure(new Rook(FigureColors.WHITE, cell));
            }

            if (cell.Id == "B1" || cell.Id == "G1")
            {
                cell.SetFigure(new Knight(FigureColors.WHITE, cell));
            }

            if (cell.Id.Contains('7'))
            {
                cell.SetFigure(new Pawn(FigureColors.BLACK, cell));
            }

            if (cell.Id == "E8")
            {
                cell.SetFigure(new King(FigureColors.BLACK, cell));
            }

            if (cell.Id == "D8")
            {
                cell.SetFigure(new Queen(FigureColors.BLACK, cell));
            }

            if (cell.Id == "F8" || cell.Id == "C8")
            {
                cell.SetFigure(new Bishop(FigureColors.BLACK, cell));
            }

            if (cell.Id == "A8" || cell.Id == "H8")
            {
                cell.SetFigure(new Rook(FigureColors.BLACK, cell));
            }

            if (cell.Id == "B8" || cell.Id == "G8")
            {
                cell.SetFigure(new Knight(FigureColors.BLACK, cell));
            }
        }
    }

    public (Cell?, bool) MoveFigure(string moveFigureParams, KingAttacker? kingAttacker, Cell kingCell)
    {
        string[] ids = moveFigureParams.Split(' ');
        if (ids[0] == ids[1])
        {
            return (null, false);
        }
        Cell? fromCell = null;
        Cell? toCell = null;
        foreach (var cell in ChessBoardState)
        {
            if (cell.Id == ids[0])
                fromCell = cell;
            if (cell.Id == ids[1])
                toCell = cell;
        }

        if (fromCell == null || toCell == null || fromCell.Figure == null)
        {
            return (null, false);
        }

        if (fromCell.Figure.CanMove(toCell, ChessBoardState, kingAttacker))
        {
            var figure = fromCell.Figure;
            fromCell.SetFigure(null);
            if (figure.Name != FigureNames.KING)
            {
                var possibleKingAttacker = kingCell.IsUnderAttack(ChessBoardState, figure.Color);
                if (possibleKingAttacker != null && !possibleKingAttacker.IntermCells.Contains(toCell))
                {
                    fromCell.SetFigure(figure);
                    return (null, false);
                }
            }
            toCell.SetFigure(figure);
            if (figure.Name == FigureNames.KING)
            {
                var possibleFigure = (King)figure;
                possibleFigure.IsFirstStep = false;
            }
            if (figure.Name == FigureNames.ROOK)
            {
                var possibleFigure = (Rook)figure;
                possibleFigure.IsFirstStep = false;
            }
            if (figure.Name == FigureNames.PAWN)
            {
                var possibleFigure = (Pawn)figure;
                possibleFigure.IsFirstStep = false;
            }
            return figure.Name == FigureNames.KING ? (toCell, true) : (null, true);
        }

        return (null, false);
    }

    public bool CheckIsMate(KingAttacker? kingAttacker, FigureColors playerColor, Cell kingCell)
    {
        if (kingAttacker == null)
        {
            return false;
        }

        var king = (King)kingCell.Figure!;
        foreach (var cell in ChessBoardState)
        {
            if (king.CanMove(cell, ChessBoardState, kingAttacker))
            {
                return false;
            }
        }
        
        var playerFigures = ChessBoardState
            .Where(cell => cell.Figure != null && cell.Figure.Color == playerColor)
            .Select(cell => cell.Figure);
        var cellToPreventCheck = kingAttacker.IntermCells.Find(cell =>
        {
            var result = false;
            foreach (var figure in playerFigures)
            {
                if (figure!.CanMove(cell, ChessBoardState, kingAttacker))
                {
                    result = true;
                    return result;
                }
            }

            return result;
        });
        
        return cellToPreventCheck == null;
    }
}