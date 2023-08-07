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

    /*
    A method to generate a state tree from a given file (for testing only)
    
    Parameters
    ----------
    FileName: Path to file to read tree from

    board: Chess board tree is associated with. It should not have the root move on it!

    Returns
    -------
    The root node of the completed tree
    */
    private Node MakeTree(string FileName, Board board) {
        return new Node(Move.NullMove, true);
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
        // Starting move
        Move StartMove = new ("b8a8", KPBoard);
        // Making the move on the board
        KPBoard.MakeMove(StartMove);
        // Creating root
        Node root = new Node(StartMove, false);
        // Setting root visits and wins
        int RootVisits = 20;
        float RootWins = 5;
        for (int i = 0; i < RootVisits; i++) {
            // If we still need to add wins
            if (root.GetWins() < RootWins) {
                // If we have a 1/2 a point, we only want to add 0.5
                if (RootWins % 1 != 0) {
                    root.IncrementWins(0.5f);
                }
                // Otherwise, we want to add 1
                else {
                    root.IncrementWins(1);
                }
            }
            // Otherwise, we just add 0 to wins counter
            else {
                root.IncrementWins(0);
            }
        }
        // Getting possible moves
        Move[] PossibleMoves = KPBoard.GetLegalMoves();
        // An array to store root children in
        Node[] RootChildren = new Node[PossibleMoves.Length];
        // Target node for next tree joint
        Node BranchNode = new(Move.NullMove, true);
        // Expected node first off the stack (for testing)
        Node ExpectedFirstNode = new(Move.NullMove, true);
        // Looping through PossibleMoves, making them into nodes, and storing in RootChildren
        for (int i = 0; i < PossibleMoves.Length; i++) {
            // Current move
            Move CurrentMove = PossibleMoves[i];
            // Making node out of current move
            // All these moves are for white
            Node CurrentNode = new(CurrentMove, true);
            // Saving node
            RootChildren[i] = CurrentNode;
            // If this is the node we want to branch from...
            if ((PossibleMoves[i].StartSquare.Name + PossibleMoves[i].TargetSquare.Name).Equals("h2h4")) {
                // Hold on to it!
                BranchNode = CurrentNode;
            }
        }
        // Setting wins/visits and adding as children to root
        // This foreach loop is getting pretty complex
        foreach (Node node in RootChildren) {
            // Adding to root as a child
            root.AddChild(node);
            // Target # of visits and wins
            int visits = 0;
            float wins = 0;
            // Grabbing this node's move
            Move move = node.GetMove();
            // Making move string
            string MoveName = move.StartSquare.Name + move.TargetSquare.Name;
            // Assigning visits and wins depending on move
            switch (MoveName)
            {
                case "h2h3":
                    wins = 7;
                    visits = 8;
                    break;

                case "h2h4":
                    wins = 4;
                    visits = 5;
                    break;

                case "h1g1":
                    wins = 0;
                    visits = 2;
                    break;

                case "h1g2":
                    wins = 3;
                    visits = 5;
                    break;
            }
            // Incrementing wins to match targets
            for (int i = 0; i < visits; i++) {
                // If we still need to add wins
                if (node.GetWins() < wins) {
                    // If we have a 1/2 a point, we only want to add 0.5
                    if (wins % 1 != 0) {
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
        }

        // Making the next move on the board
        KPBoard.MakeMove(BranchNode.GetMove());
        // Doing it all again :/
        PossibleMoves = KPBoard.GetLegalMoves();
        RootChildren = new Node[PossibleMoves.Length];
        // Looping through possible moves and making nodes
        for (int i = 0; i < PossibleMoves.Length; i++) {
            // Current move
            Move CurrentMove = PossibleMoves[i];
            // Making node out of current move
            // All these moves are for white
            Node CurrentNode = new(CurrentMove, true);
            // Saving node
            RootChildren[i] = CurrentNode;
            // If this is should be the first move off the stack, save it!
            if ((PossibleMoves[i].StartSquare.Name + PossibleMoves[i].TargetSquare.Name).Equals("a7a5")) {
                ExpectedFirstNode = CurrentNode;
            }
        }
        // Ugh
        // Setting wins/visits and adding as children to root
        // This foreach loop is getting pretty complex
        foreach (Node node in RootChildren) {
            // Adding to root as a child
            BranchNode.AddChild(node);
            // Target # of visits and wins
            int visits = 0;
            float wins = 0;
            // Grabbing this node's move
            Move move = node.GetMove();
            // Making move string
            string MoveName = move.StartSquare.Name + move.TargetSquare.Name;
            // Assigning visits and wins depending on move
            switch (MoveName)
            {
                case "a7a6":
                    wins = 0;
                    visits = 1;
                    break;

                case "a7a5":
                    wins = 1;
                    visits = 1;
                    break;

                case "a8b8":
                    wins = 0;
                    visits = 2;
                    break;

                case "a8b7":
                    wins = 0;
                    visits = 1;
                    break;
            }
            // Incrementing wins to match targets
            for (int i = 0; i < visits; i++) {
                // If we still need to add wins
                if (node.GetWins() < wins) {
                    // If we have a 1/2 a point, we only want to add 0.5
                    if (wins % 1 != 0) {
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
        }

        // Undoing BranchNode
        KPBoard.UndoMove(BranchNode.GetMove());
        // Doing Traverse!!
        Stack<Node> stack  = new();
        stack = Traverse(root, KPBoard, stack);
        // Testing correctness!!!
        // Checking first element off the stack is the right leaf node
        Assert.AreEqual(ExpectedFirstNode, stack.Pop());
        // The next element should be the branch node
        Assert.AreEqual(BranchNode, stack.Pop());
        // The next element off the stack should be the root
        Assert.AreEqual(root, stack.Pop());
        // Stack should be empty
        Assert.AreEqual(0, stack.Count);
    }

    // Testing Traverse for a shallower travel in tree
    [TestMethod]
    public void ShallowTraverseTest() 
    {
        // Starting move
        Move StartMove = new ("b8a8", KPBoard);
        // Making the move on the board
        KPBoard.MakeMove(StartMove);
        // Creating root
        Node root = new(StartMove, false);
        // Setting root visits and wins
        int RootVisits = 20;
        float RootWins = 5;
        for (int i = 0; i < RootVisits; i++) {
            // If we still need to add wins
            if (root.GetWins() < RootWins) {
                // If we have a 1/2 a point, we only want to add 0.5
                if (RootWins % 1 != 0) {
                    root.IncrementWins(0.5f);
                }
                // Otherwise, we want to add 1
                else {
                    root.IncrementWins(1);
                }
            }
            // Otherwise, we just add 0 to wins counter
            else {
                root.IncrementWins(0);
            }
        }
        // Getting possible moves
        Move[] PossibleMoves = KPBoard.GetLegalMoves();
        // An array to store root children in
        Node[] RootChildren = new Node[PossibleMoves.Length];
        // Target node for next tree joint
        Node BranchNode = new(Move.NullMove, true);
        // Expected node first off the stack (for testing)
        Node ExpectedFirstNode = new(Move.NullMove, true);
        // Looping through PossibleMoves, making them into nodes, and storing in RootChildren
        for (int i = 0; i < PossibleMoves.Length; i++) {
            // Current move
            Move CurrentMove = PossibleMoves[i];
            // Making node out of current move
            // All these moves are for white
            Node CurrentNode = new(CurrentMove, true);
            // Saving node
            RootChildren[i] = CurrentNode;
            // If this is the node we want to branch from...
            if ((PossibleMoves[i].StartSquare.Name + PossibleMoves[i].TargetSquare.Name).Equals("h2h3")) {
                // Hold on to it!
                BranchNode = CurrentNode;
            }
            // If this is should be the first move off the stack, save it!
            if ((PossibleMoves[i].StartSquare.Name + PossibleMoves[i].TargetSquare.Name).Equals("h1g2")) {
                ExpectedFirstNode = CurrentNode;
            }
        }
        // Setting wins/visits and adding as children to root
        // This foreach loop is getting pretty complex
        foreach (Node node in RootChildren) {
            // Adding to root as a child
            root.AddChild(node);
            // Target # of visits and wins
            int visits = 0;
            float wins = 0;
            // Grabbing this node's move
            Move move = node.GetMove();
            // Making move string
            string MoveName = move.StartSquare.Name + move.TargetSquare.Name;
            // Assigning visits and wins depending on move
            switch (MoveName)
            {
                case "h2h3":
                    wins = 8;
                    visits = 10;
                    break;

                case "h2h4":
                    wins = 5;
                    visits = 5;
                    break;

                case "h1g1":
                    wins = 1;
                    visits = 3;
                    break;

                case "h1g2":
                    wins = 1;
                    visits = 2;
                    break;
            }
            // Incrementing wins to match targets
            for (int i = 0; i < visits; i++) {
                // If we still need to add wins
                if (node.GetWins() < wins) {
                    // If we have a 1/2 a point, we only want to add 0.5
                    if (wins % 1 != 0) {
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
        }

        // Making the next move on the board
        KPBoard.MakeMove(BranchNode.GetMove());
        // Doing it all again :/
        PossibleMoves = KPBoard.GetLegalMoves();
        RootChildren = new Node[PossibleMoves.Length];
        // Looping through possible moves and making nodes
        for (int i = 0; i < PossibleMoves.Length; i++) {
            // Current move
            Move CurrentMove = PossibleMoves[i];
            // Making node out of current move
            // All these moves are for white
            Node CurrentNode = new(CurrentMove, true);
            // Saving node
            RootChildren[i] = CurrentNode;
        }
        // Ugh
        // Setting wins/visits and adding as children to root
        // This foreach loop is getting pretty complex
        foreach (Node node in RootChildren) {
            // Adding to root as a child
            BranchNode.AddChild(node);
            // Target # of visits and wins
            int visits = 0;
            float wins = 0;
            // Grabbing this node's move
            Move move = node.GetMove();
            // Making move string
            string MoveName = move.StartSquare.Name + move.TargetSquare.Name;
            // Assigning visits and wins depending on move
            switch (MoveName)
            {
                case "a7a6":
                    wins = 0;
                    visits = 2;
                    break;

                case "a7a5":
                    wins = 1;
                    visits = 2;
                    break;

                case "a8b8":
                    wins = 0;
                    visits = 2;
                    break;

                case "a8b7":
                    wins = 0;
                    visits = 4;
                    break;
            }
            // Incrementing wins to match targets
            for (int i = 0; i < visits; i++) {
                // If we still need to add wins
                if (node.GetWins() < wins) {
                    // If we have a 1/2 a point, we only want to add 0.5
                    if (wins % 1 != 0) {
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
        }

        // Undoing BranchNode
        KPBoard.UndoMove(BranchNode.GetMove());
        // Doing Traverse!!
        Stack<Node> stack  = new();
        stack = Traverse(root, KPBoard, stack);
        
        // Testing correctness!!!
        // Checking first element off the stack is the right leaf node
        Assert.AreEqual(ExpectedFirstNode, stack.Pop());
        // The next element off the stack should be the root
        Assert.AreEqual(root, stack.Pop());
        // Stack should be empty
        Assert.AreEqual(0, stack.Count);
    }

    // Testing Traverse for a node not fully populated with children
    [TestMethod]
    public void IncompleteNodeTest() 
    {
        // Starting move
        Move StartMove = new ("b8a8", KPBoard);
        // Making the move on the board
        KPBoard.MakeMove(StartMove);
        // Creating root
        Node root = new(StartMove, false);
        // Setting root visits and wins
        int RootVisits = 20;
        float RootWins = 15;
        for (int i = 0; i < RootVisits; i++) {
            // If we still need to add wins
            if (root.GetWins() < RootWins) {
                // If we have a 1/2 a point, we only want to add 0.5
                if (RootWins % 1 != 0) {
                    root.IncrementWins(0.5f);
                }
                // Otherwise, we want to add 1
                else {
                    root.IncrementWins(1);
                }
            }
            // Otherwise, we just add 0 to wins counter
            else {
                root.IncrementWins(0);
            }
        }
        // Getting possible moves
        Move[] PossibleMoves = KPBoard.GetLegalMoves();
        // An array to store root children in
        Node[] RootChildren = new Node[PossibleMoves.Length];
        // Target node for next tree joint
        Node BranchNode = new(Move.NullMove, true);
        // Expected node first off the stack (for testing)
        Node ExpectedFirstNode = new(Move.NullMove, true);
        // Looping through PossibleMoves, making them into nodes, and storing in RootChildren
        for (int i = 0; i < PossibleMoves.Length; i++) {
            // Current move
            Move CurrentMove = PossibleMoves[i];
            // Making node out of current move
            // All these moves are for white
            Node CurrentNode = new(CurrentMove, true);
            // Saving node
            RootChildren[i] = CurrentNode;
            // If this is the node we want to branch from...
            if ((PossibleMoves[i].StartSquare.Name + PossibleMoves[i].TargetSquare.Name).Equals("h2h3")) {
                // Hold on to it!
                BranchNode = CurrentNode;
            }
            // If this is should be the first move off the stack, save it!
            if ((PossibleMoves[i].StartSquare.Name + PossibleMoves[i].TargetSquare.Name).Equals("h2h3")) {
                ExpectedFirstNode = CurrentNode;
            }
        }
        // Setting wins/visits and adding as children to root
        // This foreach loop is getting pretty complex
        foreach (Node node in RootChildren) {
            // Adding to root as a child
            root.AddChild(node);
            // Target # of visits and wins
            int visits = 0;
            float wins = 0;
            // Grabbing this node's move
            Move move = node.GetMove();
            // Making move string
            string MoveName = move.StartSquare.Name + move.TargetSquare.Name;
            // Assigning visits and wins depending on move
            switch (MoveName)
            {
                case "h2h3":
                    wins = 2;
                    visits = 3;
                    break;

                case "h2h4":
                    wins = 1;
                    visits = 8;
                    break;

                case "h1g1":
                    wins = 1;
                    visits = 5;
                    break;

                case "h1g2":
                    wins = 1;
                    visits = 5;
                    break;
            }
            // Incrementing wins to match targets
            for (int i = 0; i < visits; i++) {
                // If we still need to add wins
                if (node.GetWins() < wins) {
                    // If we have a 1/2 a point, we only want to add 0.5
                    if (wins % 1 != 0) {
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
        }

        // Making the next move on the board
        KPBoard.MakeMove(BranchNode.GetMove());
        // Doing it all again :/
        Move[] AllMoves = KPBoard.GetLegalMoves();
        // Removing one of the child nodes (doesn't matter which one)
        PossibleMoves = new Move[AllMoves.Length - 1];
        for (int i = 0; i < PossibleMoves.Length; i++) {
            PossibleMoves[i] = AllMoves[i];
        }
        // Making array for nodes
        RootChildren = new Node[PossibleMoves.Length];
        // Looping through possible moves and making nodes
        for (int i = 0; i < PossibleMoves.Length; i++) {
            // Current move
            Move CurrentMove = PossibleMoves[i];
            // Making node out of current move
            // All these moves are for white
            Node CurrentNode = new(CurrentMove, true);
            // Saving node
            RootChildren[i] = CurrentNode;
        }
        // Ugh
        // Setting wins/visits and adding as children to root
        // This foreach loop is getting pretty complex
        foreach (Node node in RootChildren) {
            // Adding to root as a child
            BranchNode.AddChild(node);
            // Target # of visits and wins
            int visits = 0;
            float wins = 0;
            // Grabbing this node's move
            Move move = node.GetMove();
            // Making move string
            string MoveName = move.StartSquare.Name + move.TargetSquare.Name;
            // Assigning visits and wins depending on move
            switch (MoveName)
            {
                case "a7a6":
                    wins = 0;
                    visits = 2;
                    break;

                case "a7a5":
                    wins = 1;
                    visits = 2;
                    break;

                case "a8b8":
                    wins = 0;
                    visits = 2;
                    break;
                
                case "a8b7":
                    wins = 0;
                    visits = 2;
                    break;
            }
            // Incrementing wins to match targets
            for (int i = 0; i < visits; i++) {
                // If we still need to add wins
                if (node.GetWins() < wins) {
                    // If we have a 1/2 a point, we only want to add 0.5
                    if (wins % 1 != 0) {
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
        }

        // Undoing BranchNode
        KPBoard.UndoMove(BranchNode.GetMove());
        // Doing Traverse!!
        Stack<Node> stack  = new();
        stack = Traverse(root, KPBoard, stack);
        
        // Testing correctness!!!
        // Checking first element off the stack is the right leaf node
        Assert.AreEqual(ExpectedFirstNode, stack.Pop());
        // The next element off the stack should be the root
        Assert.AreEqual(root, stack.Pop());
        // Stack should be empty
        Assert.AreEqual(0, stack.Count);
    }

    // Testing Traverse for a completely unexplored node
    [TestMethod]
    public void UnexploredNodeTest() 
    {
        // Starting move
        Move StartMove = new ("b8a8", KPBoard);
        // Making the move on the board
        KPBoard.MakeMove(StartMove);
        // Creating root
        Node root = new(StartMove, false);
        // Setting root visits and wins
        int RootVisits = 20;
        float RootWins = 5;
        for (int i = 0; i < RootVisits; i++) {
            // If we still need to add wins
            if (root.GetWins() < RootWins) {
                // If we have a 1/2 a point, we only want to add 0.5
                if (RootWins % 1 != 0) {
                    root.IncrementWins(0.5f);
                }
                // Otherwise, we want to add 1
                else {
                    root.IncrementWins(1);
                }
            }
            // Otherwise, we just add 0 to wins counter
            else {
                root.IncrementWins(0);
            }
        }
        // Getting possible moves
        Move[] PossibleMoves = KPBoard.GetLegalMoves();
        // An array to store root children in
        Node[] RootChildren = new Node[PossibleMoves.Length];
        // Target node for next tree joint
        Node BranchNode = new(Move.NullMove, true);
        // Expected node first off the stack (for testing)
        Node ExpectedFirstNode = new(Move.NullMove, true);
        // Looping through PossibleMoves, making them into nodes, and storing in RootChildren
        for (int i = 0; i < PossibleMoves.Length; i++) {
            // Current move
            Move CurrentMove = PossibleMoves[i];
            // Making node out of current move
            // All these moves are for white
            Node CurrentNode = new(CurrentMove, true);
            // Saving node
            RootChildren[i] = CurrentNode;
            // If this is the node we want to branch from...
            if ((PossibleMoves[i].StartSquare.Name + PossibleMoves[i].TargetSquare.Name).Equals("h2h3")) {
                // Hold on to it!
                BranchNode = CurrentNode;
            }
        }
        // Setting wins/visits and adding as children to root
        // This foreach loop is getting pretty complex
        foreach (Node node in RootChildren) {
            // Adding to root as a child
            root.AddChild(node);
            // Target # of visits and wins
            int visits = 0;
            float wins = 0;
            // Grabbing this node's move
            Move move = node.GetMove();
            // Making move string
            string MoveName = move.StartSquare.Name + move.TargetSquare.Name;
            // Assigning visits and wins depending on move
            switch (MoveName)
            {
                case "h2h3":
                    wins = 10;
                    visits = 10;
                    break;

                case "h2h4":
                    wins = 0;
                    visits = 2;
                    break;

                case "h1g1":
                    wins = 2;
                    visits = 5;
                    break;

                case "h1g2":
                    wins = 3;
                    visits = 5;
                    break;
            }
            // Incrementing wins to match targets
            for (int i = 0; i < visits; i++) {
                // If we still need to add wins
                if (node.GetWins() < wins) {
                    // If we have a 1/2 a point, we only want to add 0.5
                    if (wins % 1 != 0) {
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
        }

        // Making the next move on the board
        KPBoard.MakeMove(BranchNode.GetMove());
        // Doing it all again :/
        PossibleMoves = KPBoard.GetLegalMoves();
        RootChildren = new Node[PossibleMoves.Length];
        // Looping through possible moves and making nodes
        for (int i = 0; i < PossibleMoves.Length; i++) {
            // Current move
            Move CurrentMove = PossibleMoves[i];
            // Making node out of current move
            // All these moves are for white
            Node CurrentNode = new(CurrentMove, true);
            // Saving node
            RootChildren[i] = CurrentNode;
            // If this is should be the first move off the stack, save it!
            if ((PossibleMoves[i].StartSquare.Name + PossibleMoves[i].TargetSquare.Name).Equals("a7a5")) {
                ExpectedFirstNode = CurrentNode;
            }
        }
        // Ugh
        // Setting wins/visits and adding as children to root
        // This foreach loop is getting pretty complex
        foreach (Node node in RootChildren) {
            // Adding to root as a child
            BranchNode.AddChild(node);
            // Target # of visits and wins
            int visits = 0;
            float wins = 0;
            // Grabbing this node's move
            Move move = node.GetMove();
            // Making move string
            string MoveName = move.StartSquare.Name + move.TargetSquare.Name;
            // Assigning visits and wins depending on move
            switch (MoveName)
            {
                case "a7a6":
                    wins = 0;
                    visits = 5;
                    break;

                case "a7a5":
                    wins = 0;
                    visits = 0;
                    break;

                case "a8b8":
                    wins = 0;
                    visits = 2;
                    break;

                case "a8b7":
                    wins = 0;
                    visits = 3;
                    break;
            }
            // Incrementing wins to match targets
            for (int i = 0; i < visits; i++) {
                // If we still need to add wins
                if (node.GetWins() < wins) {
                    // If we have a 1/2 a point, we only want to add 0.5
                    if (wins % 1 != 0) {
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
        }

        // Undoing BranchNode
        KPBoard.UndoMove(BranchNode.GetMove());
        // Doing Traverse!!
        Stack<Node> stack  = new();
        stack = Traverse(root, KPBoard, stack);
        // Testing correctness!!!
        // Checking first element off the stack is the right leaf node
        Assert.AreEqual(ExpectedFirstNode, stack.Pop());
        // The next element should be the branch node
        Assert.AreEqual(BranchNode, stack.Pop());
        // The next element off the stack should be the root
        Assert.AreEqual(root, stack.Pop());
        // Stack should be empty
        Assert.AreEqual(0, stack.Count);
    }
}