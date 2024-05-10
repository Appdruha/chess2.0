public class Rook : Figure
{
    public bool IsFirstStep { get; set; } = true;

    public Rook(FigureColors color, Cell cell) : base(color, cell, FigureNames.ROOK) {}

    public override bool CanMove(Cell target, List<Cell> cells, KingAttacker? kingAttacker)
    {
        if (!base.CanMove(target, null, kingAttacker))
        {
            return false;
        }
        var cell = Cell.GetCellById(cells, CellId)!;

        return cell.IsEmptyVertical(target, cells) || cell.IsEmptyHorizontal(target, cells);
    }
}