public class Knight : Figure
{
    public Knight(FigureColors color, Cell cell) 
        : base(color, cell, FigureNames.KNIGHT) {}

    public override bool CanMove(Cell target, List<Cell> cells, KingAttacker? kingAttacker)
    {
        if (!base.CanMove(target, null, kingAttacker))
        {
            return false;
        }
        
        var cell = Cell.GetCellById(cells, CellId)!;
        int dx = Math.Abs(cell.X - target.X);
        int dy = Math.Abs(cell.Y - target.Y);

        return (dx == 1 && dy == 2) || (dx == 2 && dy == 1);
    }
}