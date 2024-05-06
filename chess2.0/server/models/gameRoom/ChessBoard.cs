public class ChessBoard
{
    public static List<Cell> InitCells()
    {
        var cellColor = CellCollors.BLACK;
        var chessBoard = new List<Cell>();
        var columns = "ABCDEFGH";
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                var x = i;
                var y = 8 - j;
                chessBoard.Add(new Cell(x, y, cellColor, columns[i] + (j + 1).ToString()));
                if (j != 7)
                {
                    cellColor = cellColor == CellCollors.WHITE ? CellCollors.BLACK : CellCollors.WHITE;
                }
            }
        }

        return chessBoard;
    }

    public static List<Cell> InitFigures(List<Cell> cells)
    {
        foreach (var cell in cells)
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

        return cells;
    }
}
