using chess2._0.models;
using chess2._0.models.figures;
using chess2._0.models.gameRoom;

namespace Tests;

[TestFixture]
public class FiguresTestsForCommonChess
{
    private ChessBoard _chessBoard;
    private Cell _cellForFigure;
    private Cell _cellForKing;

    [SetUp]
    public void SetUp()
    {
        _chessBoard = new ChessBoard(GameMode.CommonChess);
        _cellForFigure = _chessBoard.ChessBoardState.Find(cell => cell.Id == "B2")!;
        _cellForKing = _chessBoard.ChessBoardState.Find(cell => cell.Id == "E1")!;
        _cellForKing.SetFigure(new King(FigureColors.WHITE, _cellForKing));
    }
    
    //----King under attack tests
    [TestCase("B4")]
    [TestCase("E2")]
    [TestCase("D1")]
    public void White_Figure_Can_Move_Only_To_Prevent_Check(string targetCellId)
    {
        _cellForFigure.SetFigure(new Queen(FigureColors.WHITE, _cellForFigure));
        var enemyCell = _chessBoard.ChessBoardState.Find(cell => cell.Id == "E3")!;
        var enemyRook = new Rook(FigureColors.BLACK, enemyCell);
        enemyCell.SetFigure(enemyRook);
        var intermCell = _chessBoard.ChessBoardState.Find(cell => cell.Id == "E2")!;

        if (targetCellId == "B4")
        {
            var result = _chessBoard.MoveFigure(String.Format("B2 {0}", targetCellId), 
                new KingAttacker(enemyRook, new List<Cell>{intermCell}), _cellForKing);
            Assert.That(result.Item1, Is.EqualTo(null), "Unexpected new king cell after move");
            Assert.That(result.Item2, Is.EqualTo(false), "Figure move should return false");
        }
        
        if (targetCellId == "E2")
        {
            var result = _chessBoard.MoveFigure(String.Format("B2 {0}", targetCellId), 
                new KingAttacker(enemyRook, new List<Cell>{intermCell}), _cellForKing);
            Assert.That(result.Item1, Is.EqualTo(null), "Unexpected new king cell after move");
            Assert.That(result.Item2, Is.EqualTo(true), "Figure move should return true");
        }
        
        if (targetCellId == "D1")
        {
            var result = _chessBoard.MoveFigure(String.Format("E1 {0}", targetCellId), 
                new KingAttacker(enemyRook, new List<Cell>{intermCell}), _cellForKing);
            Assert.That(result.Item1, Is.Not.EqualTo(null), "Expected new king cell after king move");
            Assert.That(result.Item2, Is.EqualTo(true), "King move should return true");
        }
        
    }

    // ----Pawn Tests
    [TestCase("B4")]
    [TestCase("B3")]
    public void Move_Pawn_Return_null_true_Tuple(string targetCellId)
    {
        _cellForFigure.SetFigure(new Pawn(FigureColors.WHITE, _cellForFigure));
        var result = _chessBoard.MoveFigure(String.Format("B2 {0}", targetCellId), null, _cellForKing);
        Assert.That(result.Item1, Is.EqualTo(null), "Unexpected new king cell after pawn move");
        Assert.That(result.Item2, Is.EqualTo(true), "Pawn move should return true");
    }

    [TestCase("B1")]
    [TestCase("A2")]
    [TestCase("C2")]
    [TestCase("A1")]
    [TestCase("C3")]
    [TestCase("B5")]
    public void Move_Pawn_Return_null_false_Tuple(string targetCellId)
    {
        _cellForFigure.SetFigure(new Pawn(FigureColors.WHITE, _cellForFigure));
        var result = _chessBoard.MoveFigure(String.Format("B2 {0}", targetCellId), null, _cellForKing);
        Assert.That(result.Item1, Is.EqualTo(null), "Unexpected new king cell after pawn move");
        Assert.That(result.Item2, Is.EqualTo(false), "Pawn move should return false");
    }

    [Test]
    public void Move_Pawn_Return_null_false_Tuple_After_Second_Two_Cell_Move()
    {
        _cellForFigure.SetFigure(new Pawn(FigureColors.WHITE, _cellForFigure));
        _chessBoard.MoveFigure("B2 B4", null, _cellForKing);
        var result = _chessBoard.MoveFigure("B4 B6", null, _cellForKing);
        Assert.That(result.Item1, Is.EqualTo(null), "Unexpected new king cell after pawn move");
        Assert.That(result.Item2, Is.EqualTo(false), "Pawn move should return false");
    }

    [Test]
    public void Move_Pawn_Return_null_true_Tuple_After_Hit()
    {
        _cellForFigure.SetFigure(new Pawn(FigureColors.WHITE, _cellForFigure));
        var targetCell = _chessBoard.ChessBoardState.Find(cell => cell.Id == "C3")!;
        targetCell.SetFigure(new Queen(FigureColors.BLACK, targetCell));
        var result = _chessBoard.MoveFigure("B2 C3", null, _cellForKing);
        Assert.That(result.Item1, Is.EqualTo(null), "Unexpected new king cell after pawn move");
        Assert.That(result.Item2, Is.EqualTo(true), "Pawn move should return true");
    }

    // ----King Tests
    [TestCase("B1")]
    [TestCase("A1")]
    [TestCase("A2")]
    [TestCase("C2")]
    [TestCase("C1")]
    [TestCase("C3")]
    [TestCase("B3")]
    [TestCase("A3")]
    public void Move_King_Return_cell_true_Tuple(string targetCellId)
    {
        _cellForKing.SetFigure(null);
        _cellForFigure.SetFigure(new King(FigureColors.WHITE, _cellForFigure));
        var result = _chessBoard.MoveFigure(String.Format("B2 {0}", targetCellId), null, _cellForKing);
        Assert.That(result.Item1, Is.Not.EqualTo(null), "Expected new king cell after move");
        Assert.That(result.Item2, Is.EqualTo(true), "King move should return true");
    }

    [TestCase("B4")]
    [TestCase("J3")]
    [TestCase("B2")]
    [TestCase("C4")]
    [TestCase("D2")]
    public void Move_King_Return_null_false_Tuple(string targetCellId)
    {
        _cellForKing.SetFigure(null);
        _cellForFigure.SetFigure(new King(FigureColors.WHITE, _cellForFigure));
        var result = _chessBoard.MoveFigure(String.Format("B2 {0}", targetCellId), null, _cellForKing);
        Assert.That(result.Item1, Is.EqualTo(null), "Unexpected new king cell after move");
        Assert.That(result.Item2, Is.EqualTo(false), "King move should return false");
    }

    [TestCase("C1")]
    [TestCase("G1")]
    public void Castling_Return_cell_true_Tuple_And_Move_Rook(string targetCellId)
    {
        foreach (var cell in _chessBoard.ChessBoardState)
        {
            if (cell.Id == "A1" || cell.Id == "H1")
            {
                cell.SetFigure(new Rook(FigureColors.WHITE, cell));
            }
        }

        var king = (King)_cellForKing.Figure!;
        king.IsMyTurn = true;
        var result = _chessBoard.MoveFigure(String.Format("E1 {0}", targetCellId), null, _cellForKing);
        Assert.That(result.Item1, Is.Not.EqualTo(null), "Expected new king cell after move");
        Assert.That(result.Item2, Is.EqualTo(true), "King move should return true");
        if (targetCellId == "C1")
        {
            var newRookCell = _chessBoard.ChessBoardState.Find(cell => cell.Id == "D1")!;
            Assert.That(newRookCell.Figure, Is.Not.EqualTo(null), "Expected rook move during castling");
        }
        else
        {
            var newRookCell = _chessBoard.ChessBoardState.Find(cell => cell.Id == "F1")!;
            Assert.That(newRookCell.Figure, Is.Not.EqualTo(null), "Expected rook move during castling");
        }
    }


    [TestCase("G1")]
    [TestCase("D1")]
    public void Move_King_Return_null_false_Because_Target_Cell_Attacked(string targetCellId)
    {
        foreach (var cell in _chessBoard.ChessBoardState)
        {
            if (cell.Id == "H1")
            {
                cell.SetFigure(new Rook(FigureColors.WHITE, cell));
            }

            if (cell.Id == "F2" || cell.Id == "D2")
            {
                cell.SetFigure(new Rook(FigureColors.BLACK, cell));
            }
        }

        var king = (King)_cellForKing.Figure!;
        king.IsMyTurn = true;
        var result = _chessBoard.MoveFigure(String.Format("E1 {0}", targetCellId), null, _cellForKing);
        Assert.That(result.Item1, Is.EqualTo(null), "Unexpected new king cell after move");
        Assert.That(result.Item2, Is.EqualTo(false), "King move should return false");
    }
    
    // ----Bishop Tests
    [TestCase("A1")]
    [TestCase("C1")]
    [TestCase("A3")]
    [TestCase("E5")]
    public void Move_Bishop_Return_null_true_Tuple(string targetCellId)
    {
        _cellForFigure.SetFigure(new Bishop(FigureColors.WHITE, _cellForFigure));
        var result = _chessBoard.MoveFigure(String.Format("B2 {0}", targetCellId), null, _cellForKing);
        Assert.That(result.Item1, Is.EqualTo(null), "Unexpected new king cell after move");
        Assert.That(result.Item2, Is.EqualTo(true), "Bishop move should return true");
    }
    
    [TestCase("A2")]
    [TestCase("J3")]
    [TestCase("B2")]
    [TestCase("C2")]
    [TestCase("B1")]
    [TestCase("B3")]
    [TestCase("E6")]
    public void Move_Bishop_Return_null_false_Tuple(string targetCellId)
    {
        _cellForFigure.SetFigure(new Bishop(FigureColors.WHITE, _cellForFigure));
        var result = _chessBoard.MoveFigure(String.Format("B2 {0}", targetCellId), null, _cellForKing);
        Assert.That(result.Item1, Is.EqualTo(null), "Unexpected new king cell after move");
        Assert.That(result.Item2, Is.EqualTo(false), "Bishop move should return false");
    }

    // ----Knight Tests
    [TestCase("D3")]
    [TestCase("D1")]
    [TestCase("A4")]
    public void Move_Knight_Return_null_true_Tuple(string targetCellId)
    {
        _cellForFigure.SetFigure(new Knight(FigureColors.WHITE, _cellForFigure));
        var result = _chessBoard.MoveFigure(String.Format("B2 {0}", targetCellId), null, _cellForKing);
        Assert.That(result.Item1, Is.EqualTo(null), "Unexpected new king cell after move");
        Assert.That(result.Item2, Is.EqualTo(true), "Knight move should return true");
    }

    [TestCase("E3")]
    [TestCase("J3")]
    [TestCase("A1")]
    [TestCase("B2")]
    [TestCase("E6")]
    public void Move_Knight_Return_null_false_Tuple(string targetCellId)
    {
        _cellForFigure.SetFigure(new Knight(FigureColors.WHITE, _cellForFigure));
        var result = _chessBoard.MoveFigure(String.Format("B2 {0}", targetCellId), null, _cellForKing);
        Assert.That(result.Item1, Is.EqualTo(null), "Unexpected new king cell after move");
        Assert.That(result.Item2, Is.EqualTo(false), "Knight move should return false");
    }

    // ----Rook Tests
    [TestCase("B1")]
    [TestCase("B8")]
    [TestCase("B5")]
    [TestCase("A2")]
    [TestCase("E2")]
    [TestCase("H2")]
    public void Move_Rook_Return_null_true_Tuple(string targetCellId)
    {
        _cellForFigure.SetFigure(new Rook(FigureColors.WHITE, _cellForFigure));
        var result = _chessBoard.MoveFigure(String.Format("B2 {0}", targetCellId), null, _cellForKing);
        Assert.That(result.Item1, Is.EqualTo(null), "Unexpected new king cell after move");
        Assert.That(result.Item2, Is.EqualTo(true), "Rook move should return true");
    }

    [TestCase("A1")]
    [TestCase("J3")]
    [TestCase("B2")]
    [TestCase("C4")]
    public void Move_Rook_Return_null_false_Tuple(string targetCellId)
    {
        _cellForFigure.SetFigure(new Rook(FigureColors.WHITE, _cellForFigure));
        var result = _chessBoard.MoveFigure(String.Format("B2 {0}", targetCellId), null, _cellForKing);
        Assert.That(result.Item1, Is.EqualTo(null), "Unexpected new king cell after move");
        Assert.That(result.Item2, Is.EqualTo(false), "Rook move should return false");
    }

    // ----Queen Tests
    [TestCase("B1")]
    [TestCase("A1")]
    [TestCase("B5")]
    [TestCase("A2")]
    [TestCase("E2")]
    [TestCase("H2")]
    [TestCase("E5")]
    [TestCase("H8")]
    public void Move_Queen_Return_null_true_Tuple(string targetCellId)
    {
        _cellForFigure.SetFigure(new Queen(FigureColors.WHITE, _cellForFigure));
        var result = _chessBoard.MoveFigure(String.Format("B2 {0}", targetCellId), null, _cellForKing);
        Assert.That(result.Item1, Is.EqualTo(null), "Unexpected new king cell after move");
        Assert.That(result.Item2, Is.EqualTo(true), "Queen move should return true");
    }

    [TestCase("C4")]
    [TestCase("J3")]
    [TestCase("B2")]
    [TestCase("E8")]
    public void Move_Queen_Return_null_false_Tuple(string targetCellId)
    {
        _cellForFigure.SetFigure(new Queen(FigureColors.WHITE, _cellForFigure));
        var result = _chessBoard.MoveFigure(String.Format("B2 {0}", targetCellId), null, _cellForKing);
        Assert.That(result.Item1, Is.EqualTo(null), "Unexpected new king cell after move");
        Assert.That(result.Item2, Is.EqualTo(false), "Queen move should return false");
    }
}

[TestFixture]
public class FiguresTestsForChess20
{
    private ChessBoard _chessBoard;
    private Cell _cellForFigure;
    private Cell _cellForKing;

    [SetUp]
    public void SetUp()
    {
        _chessBoard = new ChessBoard(GameMode.Chess20);
        _cellForFigure = _chessBoard.ChessBoardState.Find(cell => cell.Id == "B2")!;
        _cellForKing = _chessBoard.ChessBoardState.Find(cell => cell.Id == "E1")!;
        _cellForKing.SetFigure(new King(FigureColors.WHITE, _cellForKing));
    }
    
    // ----Figures with walls Tests
    [TestCase("B5")]
    [TestCase("B4")]
    public void Move_Figure_Return_null_false_Tuple(string targetCellId)
    {
        _cellForFigure.SetFigure(new Queen(FigureColors.WHITE, _cellForFigure));
        var wallCell = _chessBoard.ChessBoardState.Find(cell => cell.Id == "B4")!;
        wallCell.SetFigure(new Wall(wallCell));
        var result = _chessBoard.MoveFigure(String.Format("B2 {0}", targetCellId), null, _cellForKing);
        Assert.That(result.Item1, Is.EqualTo(null), "Unexpected new king cell after move");
        Assert.That(result.Item2, Is.EqualTo(false), "Figure move should return false");
    }
    
    // ----Ram Tests
    [TestCase("A2")]
    [TestCase("C2")]
    [TestCase("B3")]
    [TestCase("B5")]
    public void Move_Ram_Return_null_true_Tuple(string targetCellId)
    {
        _cellForFigure.SetFigure(new Ram(FigureColors.WHITE, _cellForFigure));
        Cell? wallCell = null;

        if (targetCellId == "B5")
        {
            wallCell = _chessBoard.ChessBoardState.Find(cell => cell.Id == targetCellId)!;
            wallCell.SetFigure(new Wall(wallCell));
            
        }
        
        var result = _chessBoard.MoveFigure(String.Format("B2 {0}", targetCellId), null, _cellForKing);

        if (targetCellId == "B5")
        {
            if (wallCell!.Figure == null)
            {
                Assert.That(wallCell.Figure, Is.Not.EqualTo(null), "On wall cell should be Pawn");
            }
            else
            {
                Assert.That(wallCell.Figure.Name, Is.EqualTo(FigureNames.PAWN), "On wall cell should be Pawn");
            }
        }
        
        Assert.That(result.Item1, Is.EqualTo(null), "Unexpected new king cell after move");
        Assert.That(result.Item2, Is.EqualTo(true), "Ram move should return true");
    }
    
    [TestCase("A1")]
    [TestCase("B1")]
    [TestCase("C1")]
    [TestCase("A3")]
    [TestCase("C3")]
    public void Move_Ram_Return_null_false_Tuple(string targetCellId)
    {
        _cellForFigure.SetFigure(new Ram(FigureColors.WHITE, _cellForFigure));
        var result = _chessBoard.MoveFigure(String.Format("B2 {0}", targetCellId), null, _cellForKing);
        Assert.That(result.Item1, Is.EqualTo(null), "Unexpected new king cell after move");
        Assert.That(result.Item2, Is.EqualTo(false), "Ram move should return false");
    }
}