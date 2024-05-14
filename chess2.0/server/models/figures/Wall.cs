public class Wall : Figure
{
    public Wall(Cell cell)
        : base(FigureColors.NONE, cell, FigureNames.WALL) {}

    public override bool CanMove(Cell target, List<Cell> cells, KingAttacker? kingAttacker)
    {
        return false;
    }
}