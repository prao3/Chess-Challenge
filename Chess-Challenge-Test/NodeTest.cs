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
    public void TestEquals()
    {
        Node testNode2 = new(new Move("e2e4", board), true);
        Node testNode3 = new(new Move("e2e4", board), false);
        Node testNode4 = new(new Move("e2e3", board), true);

        Assert.IsTrue(testNode1.Equals(testNode2));
        Assert.IsFalse(testNode2.Equals(testNode3));
        Assert.IsFalse(testNode1.Equals(testNode4));

        // Testing null condition too
        Assert.IsFalse(testNode2.Equals(null));
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

    [TestMethod]
    public void ChildrenTest()
    {
        // Making sure list is empty to begin with
        Assert.AreEqual(0, testNode1.GetChildren().Length);

        // Adding a child
        Node child1 = new(new Move("e7e5", board), false);
        testNode1.AddChild(child1);
        // Running tests
        Assert.AreEqual(1, testNode1.GetChildren().Length);
        Assert.IsTrue(testNode1.GetChildren()[0].Equals(child1));

        // Adding another child
        Node child2 = new(new Move("d7d6", board), false);
        testNode1.AddChild(child2);
        // Running tests
        Assert.AreEqual(2, testNode1.GetChildren().Length);
        Assert.IsTrue(testNode1.GetChildren()[0].Equals(child1));
        Assert.IsTrue(testNode1.GetChildren()[1].Equals(child2));

        // Adding a third child
        Node child3 = new(new Move("d7d5", board), false);
        testNode1.AddChild(child3);
        // Running tests
        Assert.AreEqual(3, testNode1.GetChildren().Length);
        Assert.IsTrue(testNode1.GetChildren()[0].Equals(child1));
        Assert.IsTrue(testNode1.GetChildren()[1].Equals(child2));
        Assert.IsTrue(testNode1.GetChildren()[2].Equals(child3));
    }
}