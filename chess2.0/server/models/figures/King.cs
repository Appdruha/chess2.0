public class King : Figure
{
    public bool IsFirstStep { get; set; } = true;
    public (string From, string To)? RookCastling { get; set; }
    public bool IsMyTurn { get; set; }

    public King(FigureColors color, Cell cell) : base(color, cell, FigureNames.KING)
    {
        IsMyTurn = false;
    }

    public bool CanMove(Cell target, List<Cell> cells, KingAttacker? kingAttacker)
    {
        if (IsMyTurn)
        {
            var figure = target.Figure;
            target.SetFigure(null);
            if (target.IsUnderAttack(cells, Color) != null)
            {
                target.SetFigure(figure);
                return false;
            }

            target.SetFigure(figure);
        }

        if (!base.CanMove(target, cells, kingAttacker))
        {
            return false;
        }
        
        var cell = Cell.GetCellById(cells, CellId)!;
        var dx = Math.Abs(cell.X - target.X);
        var dy = Math.Abs(cell.Y - target.Y);
        if (cell.IsEmptyVertical(target, cells) && dy == 1)
        {
            return true;
        }

        if (cell.IsEmptyHorizontal(target, cells) && dx == 1)
        {
            return true;
        }

        if (cell.IsEmptyDiagonal(target, cells) && dy == 1 && dx == 1)
        {
            return true;
        }

        if (IsMyTurn && IsFirstStep && cell.IsUnderAttack(cells, Color) == null && cell.IsEmptyHorizontal(target, cells))
        {
            bool Castling(Cell rookCell, int dx)
            {
                if (rookCell.Figure != null && rookCell.Figure.Name == FigureNames.ROOK)
                {
                    var rook = (Rook)rookCell.Figure;
                    var newRookCell = cells.FirstOrDefault(cell => cell.X == target.X - dx && cell.Y == target.Y);
                    if (newRookCell != null && newRookCell.IsUnderAttack(cells, Color) == null && rook.IsFirstStep &&
                        rook.CanMove(target, cells, null))
                    {
                        rook.IsFirstStep = false;
                        RookCastling = (rookCell.Id, newRookCell.Id);
                        return true;
                    }

                    return false;
                }

                return false;
            }

            var rookCell = cells.FirstOrDefault(cell => cell.X == target.X + 1 && cell.Y == target.Y);
            if (rookCell != null && Castling(rookCell, 1))
            {
                return true;
            }

            rookCell = cells.FirstOrDefault(cell => cell.X == target.X - 2 && cell.Y == target.Y);
            if (rookCell != null && Castling(rookCell, -1))
            {
                return true;
            }
        }

        return false;
    }
}