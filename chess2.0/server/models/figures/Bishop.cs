namespace chess2._0.models.figures;

public class Bishop : Figure
{
    public Bishop(FigureColors color, Cell cell)
        : base(color, cell, FigureNames.BISHOP) {}

    public override bool CanMove(Cell target, List<Cell> cells, KingAttacker? kingAttacker)
    {
        if (!base.CanMove(target, null, kingAttacker ))
        {
            return false;
        }

        return Cell.GetCellById(cells, CellId)!.IsEmptyDiagonal(target, cells);
    }
}