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
        // Making root move on board
        board.MakeMove(root.GetMove());
        // Traversing tree
        Stack<Node> stack = new();
        stack = Traverse(root, board, stack);
        // Hanging onto nodes
        Node EndNode = stack.Pop();
        root = stack.Pop();
        // Undoing move
        board.UndoMove(EndNode.GetMove());
        // Re-traversing tree
        stack = Traverse(root, board, stack);
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
        // Making root move on board
        board.MakeMove(root.GetMove());
        // Traversing tree
        Stack<Node> stack = new();
        stack = Traverse(root, board, stack);
        // Hanging onto nodes
        Node EndNode = stack.Pop();
        root = stack.Pop();
        // Undoing move
        board.UndoMove(EndNode.GetMove());
        // Re-traversing tree
        stack = Traverse(root, board, stack);
        // Backpropagating
        Backpropagate(board, stack);

        // Asserting end node visits and wins are correct
        Assert.AreEqual(4, EndNode.GetVisits());
        Assert.AreEqual(4, EndNode.GetWins());

        // Checking root visits and wins
        Assert.AreEqual(11, root.GetVisits());
        Assert.AreEqual(3, root.GetWins());
    }

    [TestMethod]
    public void BlackWinTest() {
        // Test file name
        string FileName = "..\\..\\..\\BackpropagateTests\\blackwintest.txt";
        // Making tree
        Node root = TraverseTest.MakeTree(FileName, out Board board);
        // Making root move on board
        board.MakeMove(root.GetMove());
        // Traversing tree
        Stack<Node> stack = new();
        stack = Traverse(root, board, stack);
        // Hanging onto nodes
        Node EndNode = stack.Pop();
        root = stack.Pop();
        // Undoing move
        board.UndoMove(EndNode.GetMove());
        // Re-traversing tree
        stack = Traverse(root, board, stack);
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