using chess2._0.models.gameRoom;
using NUnit.Framework;

namespace chess2._0.__tests__;

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
    public void ChessBoard_Returns_100_Cells()
    {
        Assert.That(_chessBoard.ChessBoardState.Count, Is.EqualTo(100), "Wrong cells count in Chess20 mode");
    }
    
    [Test]
    public void ChessBoard_Returns_64_Cells()
    {
        var chessBoard = new ChessBoard(GameMode.CommonChess);
        
        Assert.That(chessBoard.ChessBoardState.Count, Is.EqualTo(64), "Wrong cells count in Common mode");
    }
}