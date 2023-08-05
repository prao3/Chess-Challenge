namespace Chess_Challenge_Test;
using ChessChallenge.API;
using static MyBot;

[TestClass]
public class TraverseTest {

    // Board at beginning of a chess game
    private readonly Board StartBoard;
    // King and pawn each board
    private readonly Board KPBoard;
    // Checkmating board
    private readonly Board CheckBoard;

    public TraverseTest() {
        // Setting up start position
        StartBoard = Board.CreateBoardFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        // Setting up king and pawn board
        KPBoard = Board.CreateBoardFromFEN("1k6/p7/8/8/8/8/7P/7K b - - 0 1");
        //Setting up checkmating board
        CheckBoard = Board.CreateBoardFromFEN("1k6/2P5/1KP5/1PP5/8/8/8/8 b - - 0 1");
    }

    // Testing Traverse when root has no children
    [TestMethod]
    public void RootNoChildrenTest() 
    {
        // Creating move
        Move StartMove = new Move("e2e4", StartBoard);
        // Making the move on the board
        StartBoard.MakeMove(StartMove);
        // Create root node
        Node root = new(StartMove, true);
        // Making the stack
        Stack<Node> stack = new();
        // Traversing from root
        stack = Traverse(root, StartBoard, stack);

        // Testing correctness!!
        // Getting node on the stack
        Node StackNode = stack.Pop();
        // Making sure the node on the stack is the root
        Assert.AreEqual(root, StackNode);
        // Making sure the stack is empty
        Assert.AreEqual(0, stack.Count);
    }

    // Testing Traverse for travelling 'deep' in tree
    [TestMethod]
    public void DeepTraverseTest() 
    {
        Assert.IsTrue(false, "Not Implemented");
    }

    // Testing Traverse for a shallower travel in tree
    [TestMethod]
    public void ShallowTraverseTest() 
    {
        Assert.IsTrue(false, "Not Implemented");
    }

    // Testing Traverse for a node not fully populated with children
    [TestMethod]
    public void IncompleteNodeTest() 
    {
        Assert.IsTrue(false, "Not Implemented");
    }

    // Testing Traverse for a completely unexplored node
    [TestMethod]
    public void UnexploredNodeTest() 
    {
        Assert.IsTrue(false, "Not Implemented");
    }

    // Testing Traverse for decisive result!
    [TestMethod]
    public void CheckmateTest() 
    {
        Assert.IsTrue(false, "Not Implemented");
    }
}