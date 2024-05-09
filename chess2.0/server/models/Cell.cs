using Newtonsoft.Json;

public enum CellCollors
{
    BLACK,
    WHITE,
    RED
}

public class Cell
{
    [JsonProperty("x")]
    public int X { get; }
    [JsonProperty("y")]
    public int Y { get; }
    [JsonProperty("color")]
    public CellCollors Color { get; }
    [JsonProperty("id")]
    public string Id { get; }
    private Figure? _figure;

    public Cell(int x, int y, CellCollors color, string id)
    {
        X = x;
        Y = y;
        Color = color;
        Id = id;
        _figure = null;
    }

    public static Cell? GetCellById(List<Cell> cells, string id)
    {
        return cells.Find(cell => cell.Id == id);
    }
    
    [JsonProperty("figure")]
    public Figure? Figure
    {
        get { return _figure; }
    }

    public void SetFigure(Figure? figure)
    {
        if (figure != null)
        {
            _figure = figure;
            _figure.CellId = Id;
        }
        else
        {
            _figure = null;
        }
    }

    public bool IsEmptyVertical(Cell target, List<Cell> cells)
    {
        if (X != target.X)
        {
            return false;
        }

        int min = Math.Min(Y, target.Y);
        int max = Math.Max(Y, target.Y);
        for (int yPos = min + 1; Y < max; yPos += 1)
        {
            Cell? cell = cells.Find(c => c.X == target.X && c.Y == yPos);
            if (cell == null || cell.Figure != null)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsEmptyHorizontal(Cell target, List<Cell> cells)
    {
        if (Y != target.Y)
        {
            return false;
        }

        int min = Math.Min(X, target.X);
        int max = Math.Max(X, target.X);
        for (int xPos = min + 1; X < max; xPos += 1)
        {
            Cell? cell = cells.Find(c => c.Y == target.Y && c.X == xPos);
            if (cell == null || cell._figure != null)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsEmptyDiagonal(Cell target, List<Cell> cells)
    {
        int absX = Math.Abs(target.X - X);
        int absY = Math.Abs(target.Y - Y);
        if (absY != absX)
        {
            return false;
        }

        int dy = Y < target.Y ? 1 : -1;
        int dx = X < target.X ? 1 : -1;

        for (int i = 1; i < absY; i += 1)
        {
            Cell? cell = cells.Find(c => c.Y == Y + dy * i && c.X == X + dx * i);
            if (cell == null || cell._figure != null)
            {
                return false;
            }
        }

        return true;
    }

    public KingAttacker? IsUnderAttack(List<Cell> cells, FigureColors color)
    {
        foreach (Cell cell in cells)
        {
            Figure? figure = cell.Figure;
            if (figure != null && figure.Color != color &&
                (figure.Name == FigureNames.PAWN ||
                 figure.CanMove(this, cells, null)))
            {
                if (!(figure.Name == FigureNames.PAWN && cell.X == X))
                {
                    if (figure.Name == FigureNames.PAWN && Y == cell.Y + (figure.Color == FigureColors.BLACK ? 1 : -1)
                                                        && (X == cell.X || X == cell.X))
                    {
                        return new KingAttacker(figure, new List<Cell> { cell } );
                    }

                    if (figure.Name != FigureNames.PAWN)
                    {
                        List<Cell> intermCells = new List<Cell>();
                        if (cell.X == X)
                        {
                            int maxY = Math.Max(Y, cell.Y);
                            int minY = Math.Min(Y, cell.Y);
                            intermCells = cells
                                .Where(i => i.X == X && i.Y <= maxY && i.Y >= minY && i.Y != Y).ToList();
                        }

                        if (cell.Y == Y)
                        {
                            int maxX = Math.Max(X, cell.X);
                            int minX = Math.Min(X, cell.X);
                            intermCells = cells
                                .Where(i => i.Y == Y && i.X <= maxX && i.X >= minX && i.X != X).ToList();
                        }

                        int absX = Math.Abs(cell.X - X);
                        int absY = Math.Abs(cell.Y - Y);
                        if (absY == absX)
                        {
                            int maxX = Math.Max(X, cell.X);
                            int minX = Math.Min(X, cell.X);
                            intermCells = cells.Where(i =>
                                i.X <= maxX && i.X >= minX && Math.Abs(i.X - X) == Math.Abs(i.Y - Y) &&
                                i.X != X).ToList();
                        }

                        return new KingAttacker(figure, intermCells);
                    }
                }
            }
        }
        return null;
    }
}