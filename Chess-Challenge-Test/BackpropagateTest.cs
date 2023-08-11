namespace Chess_Challenge_Test;
using ChessChallenge.API;
using static MyBot;

[TestClass]
public class BackpropagateTest {
    [TestMethod]
    public void DrawTest() {
        // Test file name
        string FileName = "..\\..\\..\\BackpropagateTests\\drawtest.txt";
        // Making tree
        Node root = TraverseTest.MakeTree(FileName, out Board board);
        // Traversing tree
        Stack<Node> stack = new();
        stack = Traverse(root, board, stack);
        // Hanging onto nodes
        Node EndNode = stack.Pop();
        // Backpropagating
        Backpropagate(board, stack);

        // Asserting end node visits and wins are correct
        Assert.AreEqual(2, EndNode.GetVisits());
        Assert.AreEqual(1, EndNode.GetWins());

        // Checking root visits and wins
        Assert.AreEqual(11, root.GetVisits());
        Assert.AreEqual(2, root.GetWins());
    }

    [TestMethod]
    public void WhiteWinTest() {
        // Test file name
        string FileName = "..\\..\\..\\BackpropagateTests\\whitewintest.txt";
        // Making tree
        Node root = TraverseTest.MakeTree(FileName, out Board board);
        // Traversing tree
        Stack<Node> stack = new();
        stack = Traverse(root, board, stack);
        // Hanging onto nodes
        Node EndNode = stack.Pop();
        // Backpropagating
        Backpropagate(board, stack);

        // Asserting end node visits and wins are correct
        Assert.AreEqual(4, EndNode.GetVisits());
        Assert.AreEqual(4, EndNode.GetWins());

        // Checking root visits and wins
        Assert.AreEqual(11, root.GetVisits());
        Assert.AreEqual(3, root.GetWins());
    }
}