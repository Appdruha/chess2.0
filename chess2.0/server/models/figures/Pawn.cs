public class Pawn : Figure
{
    public bool IsFirstStep { get; set; } = true;

    public Pawn(FigureColors color, Cell cell) : base(color, cell, FigureNames.PAWN) {}

    public new bool CanMove(Cell target, List<Cell> cells, KingAttacker? kingAttacker)
    {
        if (!base.CanMove(target, null, kingAttacker))
        {
            return false;
        }

        int direction = Color == FigureColors.WHITE ? -1 : 1;
        int firstStepDirection = Color == FigureColors.WHITE ? -2 : 2;

        if ((target.Y == Cell.Y + direction || IsFirstStep
                && (target.Y == Cell.Y + firstStepDirection))
            && target.X == Cell.X && target.Figure == null && Cell.IsEmptyVertical(target, cells))
        {
            return true;
        }

        if (target.Y == Cell.Y + direction
            && (target.X == Cell.X + 1 || target.X == Cell.X - 1)
            && target.Figure != null)
        {
            return true;
        }

        return false;
    }
}