namespace Chess_Challenge_Test;
using ChessChallenge.API;
using static MyBot;

[TestClass]
public class TraverseTest {

    // Board at beginning of a chess game
    private readonly Board StartBoard;
    // King and pawn each board
    private Board KPBoard;
    // Checkmating board
    private readonly Board CheckBoard;

    public TraverseTest() {
        // Setting up start position
        StartBoard = Board.CreateBoardFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
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
    // This method is so long :((((((((
    [TestMethod]
    public void DeepTraverseTest() 
    {
        // File name
        string FileName = "..\\..\\..\\deeptest.txt";
        // Constructing tree from file
        Node root = MakeTree(FileName, out KPBoard);
        // Making root move on the board
        KPBoard.MakeMove(root.GetMove());
        // Doing Traverse!!
        Stack<Node> stack  = new();
        stack = Traverse(root, KPBoard, stack);

        // Testing correctness!!!
        // Getting first node off the stack
        Node CurrentNode = stack.Pop();
        // Undoing move on board
        KPBoard.UndoMove(CurrentNode.GetMove());
        // Checking move and color are correct
        Assert.AreEqual(MakeNodeFromString("a7a5 1 1 0", KPBoard), CurrentNode);
        // Checking wins are correct
        Assert.AreEqual(1, CurrentNode.GetWins());
        // Checking visits are correct
        Assert.AreEqual(1, CurrentNode.GetVisits());

        // Getting next node off the stack
        CurrentNode = stack.Pop();
        // Undoing move on board
        KPBoard.UndoMove(CurrentNode.GetMove());
        // Checking move and color are correct
        Assert.AreEqual(MakeNodeFromString("h2h4 4 5 1", KPBoard), CurrentNode);
        // Checking wins are correct
        Assert.AreEqual(4, CurrentNode.GetWins());
        // Checking visits are correct
        Assert.AreEqual(5, CurrentNode.GetVisits());

        // Getting last node off the stack (should be the root)
        CurrentNode = stack.Pop();
        // Undoing move on board
        KPBoard.UndoMove(CurrentNode.GetMove());
        // Checking move and color are correct
        Assert.AreEqual(MakeNodeFromString("b8a8 5 20 0", KPBoard), CurrentNode);
        // Checking wins are correct
        Assert.AreEqual(5, CurrentNode.GetWins());
        // Checking visits are correct
        Assert.AreEqual(20, CurrentNode.GetVisits());

        // Stack should be empty
        Assert.AreEqual(0, stack.Count);
    }

    // Testing Traverse for a shallower travel in tree
    [TestMethod]
    public void ShallowTraverseTest() 
    {
        // File name
        string FileName = "..\\..\\..\\shallowtest.txt";
        // Constructing tree from file
        Node root = MakeTree(FileName, out KPBoard);
        // Making root move on the board
        KPBoard.MakeMove(root.GetMove());
        // Doing Traverse!!
        Stack<Node> stack  = new();
        stack = Traverse(root, KPBoard, stack);

        // Testing correctness!!!
        // Getting first node off the stack
        Node CurrentNode = stack.Pop();
        // Undoing move on board
        KPBoard.UndoMove(CurrentNode.GetMove());
        // Checking move and color are correct
        Assert.AreEqual(MakeNodeFromString("h1g2 1 2 1", KPBoard), CurrentNode);
        // Checking wins are correct
        Assert.AreEqual(1, CurrentNode.GetWins());
        // Checking visits are correct
        Assert.AreEqual(2, CurrentNode.GetVisits());

        // Getting last node off the stack (should be the root)
        CurrentNode = stack.Pop();
        // Undoing move on board
        KPBoard.UndoMove(CurrentNode.GetMove());
        // Checking move and color are correct
        Assert.AreEqual(MakeNodeFromString("b8a8 5 20 0", KPBoard), CurrentNode);
        // Checking wins are correct
        Assert.AreEqual(5, CurrentNode.GetWins());
        // Checking visits are correct
        Assert.AreEqual(20, CurrentNode.GetVisits());

        // Stack should be empty
        Assert.AreEqual(0, stack.Count);
    }

    // Testing Traverse for a node not fully populated with children
    [TestMethod]
    public void IncompleteNodeTest() 
    {
        // File name
        string FileName = "..\\..\\..\\incompletetest.txt";
        // Constructing tree from file
        Node root = MakeTree(FileName, out KPBoard);
        // Making root move on the board
        KPBoard.MakeMove(root.GetMove());
        // Doing Traverse!!
        Stack<Node> stack  = new();
        stack = Traverse(root, KPBoard, stack);

        // Testing correctness!!!
        // Getting first node off the stack
        Node CurrentNode = stack.Pop();
        // Undoing move on board
        KPBoard.UndoMove(CurrentNode.GetMove());
        // Checking move and color are correct
        Assert.AreEqual(MakeNodeFromString("h2h3 2 3 1", KPBoard), CurrentNode);
        // Checking wins are correct
        Assert.AreEqual(2, CurrentNode.GetWins());
        // Checking visits are correct
        Assert.AreEqual(3, CurrentNode.GetVisits());

        // Getting last node off the stack (should be the root)
        CurrentNode = stack.Pop();
        // Undoing move on board
        KPBoard.UndoMove(CurrentNode.GetMove());
        // Checking move and color are correct
        Assert.AreEqual(MakeNodeFromString("b8a8 5 20 0", KPBoard), CurrentNode);
        // Checking wins are correct
        Assert.AreEqual(5, CurrentNode.GetWins());
        // Checking visits are correct
        Assert.AreEqual(20, CurrentNode.GetVisits());

        // Stack should be empty
        Assert.AreEqual(0, stack.Count);
    }

    // Testing Traverse for a completely unexplored node
    [TestMethod]
    public void UnexploredNodeTest() 
    {
        // File name
        string FileName = "..\\..\\..\\unexploredtest.txt";
        // Constructing tree from file
        Node root = MakeTree(FileName, out KPBoard);
        // Making root move on the board
        KPBoard.MakeMove(root.GetMove());
        // Doing Traverse!!
        Stack<Node> stack  = new();
        stack = Traverse(root, KPBoard, stack);

        // Testing correctness!!!
        // Getting first node off the stack
        Node CurrentNode = stack.Pop();
        // Undoing move on board
        KPBoard.UndoMove(CurrentNode.GetMove());
        // Checking move and color are correct
        Assert.AreEqual(MakeNodeFromString("a7a5 0 0 0", KPBoard), CurrentNode);
        // Checking wins are correct
        Assert.AreEqual(0, CurrentNode.GetWins());
        // Checking visits are correct
        Assert.AreEqual(0, CurrentNode.GetVisits());

        // Getting next node off the stack
        CurrentNode = stack.Pop();
        // Undoing move on board
        KPBoard.UndoMove(CurrentNode.GetMove());
        // Checking move and color are correct
        Assert.AreEqual(MakeNodeFromString("h2h3 10 10 1", KPBoard), CurrentNode);
        // Checking wins are correct
        Assert.AreEqual(10, CurrentNode.GetWins());
        // Checking visits are correct
        Assert.AreEqual(10, CurrentNode.GetVisits());

        // Getting last node off the stack (should be the root)
        CurrentNode = stack.Pop();
        // Undoing move on board
        KPBoard.UndoMove(CurrentNode.GetMove());
        // Checking move and color are correct
        Assert.AreEqual(MakeNodeFromString("b8a8 5 20 0", KPBoard), CurrentNode);
        // Checking wins are correct
        Assert.AreEqual(5, CurrentNode.GetWins());
        // Checking visits are correct
        Assert.AreEqual(20, CurrentNode.GetVisits());

        // Stack should be empty
        Assert.AreEqual(0, stack.Count);
    }

    /*
    A method to generate a state tree from a given file (for testing only)
    We assume no repeated moves also
    
    Parameters
    ----------
    FileName: Path to file to read tree from

    board: Any board to assign to nodes. Note that we change this!!!!

    Returns
    -------
    The root node of the completed tree
    */
    public static Node MakeTree(string FileName, out Board board) {
        // Reading all text in file
        string[] AllText = File.ReadAllLines(FileName);
        // Making a board out of given FEN
        board = Board.CreateBoardFromFEN(AllText[0]);
        // The next text should be the root, and it should be indexed at 2
        // The actual info about the root is at index 3
        string RootText = AllText[3];
        // Making root node from text
        Node Root = MakeNodeFromString(RootText, board);
        // Queue to store all nodes in (except root)
        Queue<Node> NodesQueue = new();
        // Adding root node to the queue
        NodesQueue.Enqueue(Root);
        // The rest of the info about the tree starts at index 5
        // We want to loop through all the text and store parent and children info for each set
        for(int i = 5; i < AllText.Length; i++) {
            // Making nodes if we are at a line describing a move
            if (!AllText[i].Contains("parent") && AllText[i] != "") {
                // Making the current node from string text in file
                Node CurrentNode = MakeNodeFromString(AllText[i], board);
                // Storing the current node in a queue
                NodesQueue.Enqueue(CurrentNode);
            }
        }
        // Converting queue to array
        Node[] AllNodes = NodesQueue.ToArray();
        // Node to store current parent
        Node CurrentParent = Root;
        // Node to store current child
        Node CurrentChild = Root;
        // Looping through text again to assign children to parents
        for(int i = 5; i < AllText.Length; i++) {
            // If we are at a parent node, update current parent
            if(AllText[i].Contains("parent")) {
                // Getting node info text
                string NodeText = AllText[i].Remove(0, "parent ".Length);
                // Making node to use to find parent in nodes
                Node SearchParent = MakeNodeFromString(NodeText, board);
                // Searching for current parent and updating variable
                CurrentParent = Array.Find(AllNodes, node => node.Equals(SearchParent));
            }
            // Otherwise, update current child and make it a child of current parent
            else if(AllText[i] != "") {
                // Node to search for
                Node SearchChild = MakeNodeFromString(AllText[i], board);
                // Searching for current child and updating variable
                CurrentChild = Array.Find(AllNodes, node => node.Equals(SearchChild));
                // Assigning child to parent
                CurrentParent.AddChild(CurrentChild);
            }
        }
        // Returning the root of the tree
        return Root;
    }

    /*
    A method to make a node from a string
    The string has format <move> <wins> <visits> <color> (e.g. e2e4 5 20 1)
    A one in the color spot means white, black is 0
    */
    public static Node MakeNodeFromString(string NodeInfo, Board board) {
        // Splitting string around spaces
        string[] SubStrings = NodeInfo.Split(' ');
        // Creating move from substring
        Move move = new(SubStrings[0], board);
        // Getting wins and visits
        float Wins = float.Parse(SubStrings[1]);
        float Visits = float.Parse(SubStrings[2]);
        // Getting color
        bool IsWhite = int.Parse(SubStrings[3]) != 0;
        // Creating the node
        Node node = new(move, IsWhite);
        // Assigning wins and visits to node
        for (int i = 0; i < Visits; i++) {
            // If we still need to add wins
            if (node.GetWins() < Wins) {
                // If we have a 1/2 a point, we only want to add 0.5
                if (Wins % 1 != 0) {
                    node.IncrementWins(0.5f);
                }
                // Otherwise, we want to add 1
                else {
                    node.IncrementWins(1);
                }
            }
            // Otherwise, we just add 0 to wins counter
            else {
                node.IncrementWins(0);
            }
        }
        // Returning our created node
        return node;
    }
}