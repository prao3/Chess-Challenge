namespace Chess_Challenge_Test;
using ChessChallenge.API;
using static MyBot;

[TestClass]
public class NodeTest
{
    private readonly Node testNode1;
    private readonly Board board;
    private readonly Move move;

    public NodeTest() {
        // Setting up a test board
        board = Board.CreateBoardFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        // Making a test move
        move = new Move("e2e4", board);
        // Making a test node
        testNode1 = new MyBot.Node(move, true);
    }

    [TestMethod]
    public void TestConstructor()
    {
        // Testing that the move in the node is the same
        Assert.IsTrue(testNode1.GetMove().Equals(move));
        // Testing color
        Assert.IsTrue(testNode1.IsWhite());
    }
}