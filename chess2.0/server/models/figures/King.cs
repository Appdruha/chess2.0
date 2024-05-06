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

        var dx = Math.Abs(Cell.X - target.X);
        var dy = Math.Abs(Cell.Y - target.Y);
        if (Cell.IsEmptyVertical(target, cells) && dy == 1)
        {
            return true;
        }

        if (Cell.IsEmptyHorizontal(target, cells) && dx == 1)
        {
            return true;
        }

        if (Cell.IsEmptyDiagonal(target, cells) && dy == 1 && dx == 1)
        {
            return true;
        }

        if (IsMyTurn && IsFirstStep && Cell.IsUnderAttack(cells, Color) == null && Cell.IsEmptyHorizontal(target, cells))
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