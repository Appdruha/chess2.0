namespace chess2._0.models.figures;

public class Ram : Figure
{
    public Ram(FigureColors color, Cell cell) : base(color, cell, FigureNames.RAM) {}

    public override bool CanMove(Cell target, List<Cell> cells, KingAttacker? kingAttacker)
    {
        if (!base.CanMove(target, null, kingAttacker))
        {
            return false;
        }
        var cell = Cell.GetCellById(cells, CellId)!;

        if (cell.IsEmptyHorizontal(target, cells))
        {
            return true;
        }
        
        if (cell.IsEmptyVertical(target, cells))
        {
            var dy = cell.Y - target.Y;
            if ((dy > 0 && Color == FigureColors.WHITE) || (dy < 0 && Color == FigureColors.BLACK))
            {
                return true;
            }
            return false;
        }

        return false;
    }
}