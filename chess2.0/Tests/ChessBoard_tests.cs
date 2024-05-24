using chess2._0.models.gameRoom;

namespace Tests;

[TestFixture]
public class ChessBoardTests
{
    private ChessBoard _chessBoard;

    [SetUp]
    public void SetUp()
    {
        _chessBoard = new ChessBoard(GameMode.Chess20);
    }

    [Test]
    public void ChessBoard_Return_100_Cells()
    {
        Assert.That(_chessBoard.ChessBoardState.Count, Is.EqualTo(100), "Wrong cells count in Chess20 mode");
    }
    
    [Test]
    public void ChessBoard_Return_64_Cells()
    {
        var chessBoard = new ChessBoard(GameMode.CommonChess);
        
        Assert.That(chessBoard.ChessBoardState.Count, Is.EqualTo(64), "Wrong cells count in Common mode");
    }
    
    [TestCase(0)]
    [TestCase(1)]
    public void ChessBoard_Return_Draw_After_Second_Narrowing(int narrowingsCount)
    {
        _chessBoard.InitFigures(GameMode.Chess20);
        var gameWinner = _chessBoard.DoNarrowing(narrowingsCount);
        if (narrowingsCount == 1)
        {
            Assert.That(gameWinner, Is.EqualTo(GameWinner.Draw), "Wrong winner after Narrowing");
        }
        else
        {
            Assert.That(gameWinner, Is.EqualTo(null), "Unexpected winner after Narrowing");
        }
    }
}