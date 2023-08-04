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

    [TestMethod]
    public void TestWins()
    {
        // Testing that we start at 0
        Assert.AreEqual(0, testNode1.GetWins());
        // Checking that we havent visited this node yet
        Assert.AreEqual(0, testNode1.GetVisits());

        // Testing incrementing by 0
        testNode1.IncrementWins(0);
        Assert.AreEqual(0, testNode1.GetWins());
        Assert.AreEqual(1, testNode1.GetVisits());

        // Testing incrementing by 0.5
        testNode1.IncrementWins(0.5f);
        Assert.AreEqual(0.5, testNode1.GetWins());
        Assert.AreEqual(2, testNode1.GetVisits());

        // Testing incrementing by 1
        testNode1.IncrementWins(1);
        Assert.AreEqual(1.5, testNode1.GetWins());
        Assert.AreEqual(3, testNode1.GetVisits());

        // Testing input error
        Assert.ThrowsException<ArgumentException>(() => testNode1.IncrementWins(2));
        Assert.AreEqual(3, testNode1.GetVisits());
    }

    [TestMethod]
    public void IsWhiteTest() {
        // Testing white node
        Assert.IsTrue(testNode1.IsWhite());

        // Testing black node
        Move move2 = new("e7e5", board);
        Node testNode2 = new(move2, false);
        Assert.IsFalse(testNode2.IsWhite());
    }
}