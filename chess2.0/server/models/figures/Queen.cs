public class Queen : Figure
{
    public Queen(FigureColors color, Cell cell) 
        : base(color, cell, FigureNames.QUEEN) {}

    public new bool CanMove(Cell target, List<Cell> cells, KingAttacker? kingAttacker)
    {
        if (!base.CanMove(target, null, kingAttacker))
        {
            return false;
        }
        var cell = Cell.GetCellById(cells, CellId)!;
        if (cell.IsEmptyVertical(target, cells))
        {
            return true;
        }
        if (cell.IsEmptyHorizontal(target, cells))
        {
            return true;
        }
        if (cell.IsEmptyDiagonal(target, cells))
        {
            return true;
        }
        return false;
    }
}