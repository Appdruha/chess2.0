public class Bishop : Figure
{
    public Bishop(FigureColors color, Cell cell)
        : base(color, cell, FigureNames.BISHOP) {}

    public new bool CanMove(Cell target, List<Cell> cells, KingAttacker? kingAttacker)
    {
        if (!base.CanMove(target, null, kingAttacker ))
        {
            return false;
        }

        return Cell.IsEmptyDiagonal(target, cells);
    }
}