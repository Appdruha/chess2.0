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
        if (Cell.IsEmptyVertical(target, cells))
        {
            return true;
        }
        if (Cell.IsEmptyHorizontal(target, cells))
        {
            return true;
        }
        if (Cell.IsEmptyDiagonal(target, cells))
        {
            return true;
        }
        return false;
    }
}