public class Rook : Figure
{
    public bool IsFirstStep { get; set; } = true;

    public Rook(FigureColors color, Cell cell) : base(color, cell, FigureNames.ROOK) {}

    public new bool CanMove(Cell target, List<Cell> cells, KingAttacker? kingAttacker)
    {
        if (!base.CanMove(target, null, kingAttacker))
        {
            return false;
        }

        return Cell.IsEmptyVertical(target, cells) || Cell.IsEmptyHorizontal(target, cells);
    }
}