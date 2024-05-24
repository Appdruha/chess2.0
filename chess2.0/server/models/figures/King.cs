namespace chess2._0.models.figures;

public class King : Figure
{
    public bool IsFirstStep { get; set; } = true;
    public bool IsMyTurn { get; set; }

    public King(FigureColors color, Cell cell) : base(color, cell, FigureNames.KING){}

    public override bool CanMove(Cell target, List<Cell> cells, KingAttacker? kingAttacker)
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
        
        if (!base.CanMove(target, cells, null))
        {
            return false;
        }
        
        var cell = Cell.GetCellById(cells, CellId)!;
        var dx = Math.Abs(cell.X - target.X);
        var dy = Math.Abs(cell.Y - target.Y);
        if (dy == 1 && cell.IsEmptyVertical(target, cells))
        {
            return true;
        }
        
        if (dx == 1 && cell.IsEmptyHorizontal(target, cells))
        {
            return true;
        }
        
        if (dx == 1 && dy == 1 && cell.IsEmptyDiagonal(target, cells))
        {
            return true;
        }
        
        if (IsMyTurn && IsFirstStep && cell.IsUnderAttack(cells, Color) == null && cell.IsEmptyHorizontal(target, cells))
        {
            bool Castling(Cell rookCell, int rookDx)
            {
                if (rookCell.Figure != null && rookCell.Figure.Name == FigureNames.ROOK)
                {
                    var rook = (Rook)rookCell.Figure;
                    var newRookCell = cells.Find(cell => cell.X == target.X - rookDx && cell.Y == target.Y);
                    if (newRookCell != null && newRookCell.IsUnderAttack(cells, Color) == null && rook.IsFirstStep &&
                        rook.CanMove(target, cells, null))
                    {
                        rook.IsFirstStep = false;
                        rookCell.SetFigure(null);
                        newRookCell.SetFigure(rook);
                        IsFirstStep = false;
                        return true;
                    }

                    return false;
                }

                return false;
            }

            var rookCell = cells.Find(cell => cell.X == target.X + 1 && cell.Y == target.Y);
            if (rookCell != null && Castling(rookCell, 1))
            {
                return true;
            }

            rookCell = cells.Find(cell => cell.X == target.X - 2 && cell.Y == target.Y);
            if (rookCell != null && Castling(rookCell, -1))
            {
                return true;
            }

            return false;
        }

        return false;
    }
}