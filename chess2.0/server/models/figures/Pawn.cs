public class Pawn : Figure
{
    public bool IsFirstStep { get; set; } = true;

    public Pawn(FigureColors color, Cell cell) : base(color, cell, FigureNames.PAWN) {}

    public override bool CanMove(Cell target, List<Cell> cells, KingAttacker? kingAttacker)
    {
        if (!base.CanMove(target, null, kingAttacker))
        {
            return false;
        }

        int direction = Color == FigureColors.WHITE ? -1 : 1;
        int firstStepDirection = Color == FigureColors.WHITE ? -2 : 2;
        var cell = Cell.GetCellById(cells, CellId)!;

        if ((target.Y == cell.Y + direction || IsFirstStep
                && (target.Y == cell.Y + firstStepDirection))
            && target.X == cell.X && target.Figure == null && cell.IsEmptyVertical(target, cells))
        {
            IsFirstStep = false;
            return true;
        }

        if (target.Y == cell.Y + direction
            && (target.X == cell.X + 1 || target.X == cell.X - 1)
            && target.Figure != null)
        {
            IsFirstStep = false;
            return true;
        }

        return false;
    }
}