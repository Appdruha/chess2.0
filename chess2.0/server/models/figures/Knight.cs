public class Knight : Figure
{
    public Knight(FigureColors color, Cell cell) 
        : base(color, cell, FigureNames.KNIGHT) {}

    public new bool CanMove(Cell target, List<Cell> cells, KingAttacker? kingAttacker)
    {
        if (!base.CanMove(target, null, kingAttacker))
        {
            return false;
        }

        int dx = Math.Abs(Cell.X - target.X);
        int dy = Math.Abs(Cell.Y - target.Y);

        return (dx == 1 && dy == 2) || (dx == 2 && dy == 1);
    }
}