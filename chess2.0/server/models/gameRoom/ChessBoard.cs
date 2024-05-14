public class ChessBoard
{
    public List<Cell> ChessBoardState;

    public ChessBoard(GameMode mode)
    {
        var limit = 8;
        if (mode == GameMode.Chess20)
        {
            limit = 10;
        }

        var cellColor = CellCollors.BLACK;
        var chessBoard = new List<Cell>();
        var columns = "ABCDEFGHIJ";
        for (var i = 0; i < limit; i++)
        {
            for (var j = 0; j < limit; j++)
            {
                var x = i;
                var y = limit - 1 - j;
                chessBoard.Add(new Cell(x, y, cellColor, columns[i] + (j + 1).ToString()));
                if (j != limit - 1)
                {
                    cellColor = cellColor == CellCollors.WHITE ? CellCollors.BLACK : CellCollors.WHITE;
                }
            }
        }

        ChessBoardState = chessBoard;
    }

    public List<Cell> GetReversedBoard(GameMode mode)
    {
        var reversedBoard = new List<Cell>();
        foreach (var cell in ChessBoardState)
        {
            var figure = cell.Figure;
            var newCell = new Cell(cell.X, Math.Abs(cell.Y - (mode == GameMode.CommonChess ? 7 : 9)), cell.Color,
                cell.Id);
            newCell.SetFigure(figure);
            reversedBoard.Add(newCell);
        }

        return reversedBoard;
    }

    public void InitFigures(GameMode mode)
    {
        var wallsLimit = 6;
        var wallsCount = 0;
        var random = new Random();
        foreach (var cell in ChessBoardState)
        {
            if ((mode == GameMode.CommonChess && cell.Id.Contains('2')) ||
                (mode == GameMode.Chess20 && cell.Id.Contains('3')))
            {
                cell.SetFigure(new Pawn(FigureColors.WHITE, cell));
            }
            else if ((mode == GameMode.CommonChess && cell.Id == "E1") ||
                     (mode == GameMode.Chess20 && cell.Id == "F2"))
            {
                cell.SetFigure(new King(FigureColors.WHITE, cell));
            }
            else if ((mode == GameMode.CommonChess && cell.Id == "D1") ||
                     (mode == GameMode.Chess20 && cell.Id == "E2"))
            {
                cell.SetFigure(new Queen(FigureColors.WHITE, cell));
            }
            else if ((mode == GameMode.CommonChess && (cell.Id == "F1" || cell.Id == "C1")) ||
                     (mode == GameMode.Chess20 && (cell.Id == "G2" || cell.Id == "D2")))
            {
                cell.SetFigure(new Bishop(FigureColors.WHITE, cell));
            }
            else if ((mode == GameMode.CommonChess && (cell.Id == "A1" || cell.Id == "H1")) ||
                     (mode == GameMode.Chess20 && (cell.Id == "B2" || cell.Id == "I2")))
            {
                cell.SetFigure(new Rook(FigureColors.WHITE, cell));
            }
            else if ((mode == GameMode.CommonChess && (cell.Id == "B1" || cell.Id == "G1")) ||
                     (mode == GameMode.Chess20 && (cell.Id == "C2" || cell.Id == "H2")))
            {
                cell.SetFigure(new Knight(FigureColors.WHITE, cell));
            }
            else if ((mode == GameMode.CommonChess && cell.Id.Contains('7')) ||
                     (mode == GameMode.Chess20 && cell.Id.Contains('8')))
            {
                cell.SetFigure(new Pawn(FigureColors.BLACK, cell));
            }
            else if ((mode == GameMode.CommonChess && cell.Id == "E8") ||
                     (mode == GameMode.Chess20 && cell.Id == "F9"))
            {
                cell.SetFigure(new King(FigureColors.BLACK, cell));
            }
            else if ((mode == GameMode.CommonChess && cell.Id == "D8") ||
                     (mode == GameMode.Chess20 && cell.Id == "E9"))
            {
                cell.SetFigure(new Queen(FigureColors.BLACK, cell));
            }
            else if ((mode == GameMode.CommonChess && (cell.Id == "F8" || cell.Id == "C8")) ||
                     (mode == GameMode.Chess20 && (cell.Id == "G9" || cell.Id == "D9")))
            {
                cell.SetFigure(new Bishop(FigureColors.BLACK, cell));
            }
            else if ((mode == GameMode.CommonChess && (cell.Id == "A8" || cell.Id == "H8")) ||
                     (mode == GameMode.Chess20 && (cell.Id == "B9" || cell.Id == "I9")))
            {
                cell.SetFigure(new Rook(FigureColors.BLACK, cell));
            }
            else if ((mode == GameMode.CommonChess && (cell.Id == "B8" || cell.Id == "G8")) ||
                     (mode == GameMode.Chess20 && (cell.Id == "C9" || cell.Id == "H9")))
            {
                cell.SetFigure(new Knight(FigureColors.BLACK, cell));
            }

            if (mode == GameMode.Chess20)
            {
                if (cell.Id.Contains('5') || cell.Id.Contains('6'))
                {
                    if (wallsCount < wallsLimit && random.Next(0, 10) > 6)
                    {
                        cell.SetFigure(new Wall(cell));
                        wallsCount += 1;
                    }
                }
                else if (cell.Id == "A3" || cell.Id == "J3")
                {
                    cell.SetFigure(new Ram(FigureColors.WHITE, cell));
                }
                else if (cell.Id == "A8" || cell.Id == "J8")
                {
                    cell.SetFigure(new Ram(FigureColors.BLACK, cell));
                }
            }
        }
    }

    public void ChangeFigure(string figureName, Cell cell)
    {
        var newFigureColor = cell.Figure!.Color;
        if (figureName == "rook")
        {
            cell.SetFigure(new Rook(newFigureColor, cell));
        }
        else if (figureName == "queen")
        {
            cell.SetFigure(new Queen(newFigureColor, cell));
        }
        else if (figureName == "bishop")
        {
            cell.SetFigure(new Bishop(newFigureColor, cell));
        }
        else
        {
            cell.SetFigure(new Knight(newFigureColor, cell));
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

            if (toCell.Figure != null && figure.Name == FigureNames.RAM)
            {
                toCell.SetFigure(new Pawn(figure.Color, toCell));
            }
            else
            {
                toCell.SetFigure(figure);
            }

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

    public GameWinner? DoNarrowing(int narrowingCount)
    {
        var kings = new List<Figure>();
        var columns = "ABCDEFGHIJ";
        foreach (var cell in ChessBoardState)
        {
            if (cell.Id.Contains((narrowingCount + 1).ToString()) ||
                cell.Id.Contains(columns[narrowingCount]) ||
                cell.Id.Contains(columns[10 - narrowingCount - 1]) ||
                cell.Id.Contains((11 - narrowingCount - 1).ToString()))
            {
                if (cell.Figure != null && cell.Figure.Name == FigureNames.KING)
                {
                    kings.Add(cell.Figure);
                }

                cell.SetFigure(null);
                cell.Color = CellCollors.RED;
            }
            else if ((cell.Id.Contains((narrowingCount + 2).ToString()) ||
                      cell.Id.Contains((11 - narrowingCount - 2).ToString())) &&
                     cell.Figure != null && cell.Figure.Name == FigureNames.PAWN)
            {
                cell.SetFigure(new Knight(cell.Figure.Color, cell));
            }
        }

        if (kings.Count == 2)
        {
            return GameWinner.Draw;
        }

        if (kings.Count == 1)
        {
            if (kings[0].Color == FigureColors.BLACK)
            {
                return GameWinner.White;
            }

            return GameWinner.Black;
        }

        return null;
    }
}