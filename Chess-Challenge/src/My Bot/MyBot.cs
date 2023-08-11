using ChessChallenge.API;
using System;
using System.Collections.Generic;
// Can I use this??
using static System.Math;

public class MyBot : IChessBot
{
    public Move Think(Board board, Timer timer)
    {
        // Legal moves on the board
        Move[] moves = board.GetLegalMoves();

        // A variable to store the best move
        Move bestMove = moves[0];

        // Returning the best move
        return bestMove;
    }

    /*
    Propagates result of game back through the given stack of nodes
    This method also unplays all the moves in the stack on the given board
    (This means we assume all moves in the stack are on the given board)

    Parameters
    ----------
    board: Chess board moves have been played on

    stack: The stack with all moves played since current position in actual game
    */
    public static void Backpropagate(Board board, Stack<Node> stack) {
        // Checking if final position is a draw
        bool IsDraw = board.IsDraw();
        // Storing score to assign
        float Score = 0;
        // If a draw, score is 0.5 and not 0
        if (IsDraw) {
            Score = 0.5f;
        }
        // Pulling everything off the stack and assigning score
        while (stack.Count > 0) {
            // Pulling off the latest move
            Node CurrentMove = stack.Pop();
            // Undoing the move on the board
            board.UndoMove(CurrentMove.GetMove());
            // If not a draw, then we add one and mod 2 to score
            if (!IsDraw) {
                Score = (Score + 1) % 2;
            }
            // Assigning score
            CurrentMove.IncrementWins(Score);
        }
    }

    /*
    A method to do a playout according to playout policy
    Plays moves on the given board, adds move as children to root, and adds new nodes to stack
    Note: we assume the root move is already on the board

    Parameters
    ----------
    root: The last played move on the board

    board: Chess board we are using. Playout moves will be played on this board

    stack: Stack to store new nodes on (should be populated with nodes from traverse)

    Returns
    -------
    Returns passed in stack with playout moves added
    */
    public static Stack<Node> Playout(Node root, Board board, Stack<Node> stack) {
        // If board position is terminal, return
        if (IsTerminal(board)) {
            return stack;
        }
        // Getting next move to play
        Move NextMove = PlayoutPolicy(board.GetLegalMoves());
        // Creating node
        Node NextNode = new(NextMove, !root.IsWhite());
        // Assigning as child of root
        root.AddChild(NextNode);
        // Adding next node to stack
        stack.Push(NextNode);
        // Making move on the board
        board.MakeMove(NextMove);
        // Returning recursive call
        return Playout(NextNode, board, stack);
    }

    /*
    A helper method to check if position is terminal
    */
    private static bool IsTerminal(Board board) {
        return board.IsDraw() || board.IsInCheckmate();
    }

    /*
    A helper method to get next choice of move for Playout.
    Takes in a list of moves and returns one of them according to policy.
    */
    private static Move PlayoutPolicy(Move[] moves) {
        // Getting rng
        Random rng = new();
        // Random index in moves
        int i = rng.Next(moves.Length);
        // Returning random move
        return moves[i];
    }

    /*
    A method to recursively traverse the state tree from a given root down to a leaf
    Uses UCT to pick which path to take in tree for MCTS
    Note: Traverse takes a board and makes moves on that board. It does not undo those moves!
    We also assume that the move in the root has already been played on the board.
    
    Parameters
    ----------
    root: The root node to start traversal from
    
    stack: The stack to store nodes into. Note that we edit the stack in place!

    Returns
    -------
    Returns a Stack of type Node that contains all the nodes we traversed, in traversal order
    */
    public static Stack<Node> Traverse(Node root, Board board, Stack<Node> stack) {
        // Add the root to the stack
        stack.Push(root);
        // Base case (at a leaf)
        // If we have no children, we are at a leaf
        // Also at a leaf if not all possible moves are in tree
        // TODO: Include if node is terminal
        bool atLeaf = root.GetChildren().Length == 0 || root.GetChildren().Length < board.GetLegalMoves().Length;
        // If we are at a leaf...
        if (atLeaf) {
            // Return the stack
            return stack;
        }
        // Getting the children of the root
        Node[] children = root.GetChildren();
        // Variable to store UCT score
        float score = 0;
        // Variable to store most desirable child
        Node bestChild = children[0];
        // Looping through all children
        foreach (Node child in children) {
            // Doing UCT
            // Avoiding divide by 0
            float newScore = float.MaxValue;
            if(child.GetVisits() != 0) {
                newScore = child.GetWins() / child.GetVisits() + (float)Sqrt(2*Log(root.GetVisits()) / child.GetVisits());
            }
            // If this child is best, save it
            if (newScore > score) {
                score = newScore;
                bestChild = child;
            }
        }
        // Doing the selected move on the board
        board.MakeMove(bestChild.GetMove());
        // Recursively calling Traverse, with the new root being the best child
        return Traverse(bestChild, board, stack);
    }

    /*
    A class representing a node in Monte-Carlo Tree Search
    */
    public class Node {

        // The children of this node, stored in an ArrayList
        // Maybe need to optimize this idk
        private Node[] children = System.Array.Empty<Node>();

        // Counting wins and draws for trajectories including this node
        private float wins = 0;

        // Counting visits to this node in MCTS
        private int visits = 0;

        // Is this node for white's turn or black's turn?
        private readonly bool white;

        // Move this node is representing
        private readonly Move move;

        // Constructor
        // Takes in a move and a color
        public Node(Move move, bool white){
            this.move = move;
            this.white = white;
        }

        // Getter methods
        public Node[] GetChildren() {
            return children;
        }

        public float GetWins() {
            return wins;
        }

        public int GetVisits() {
            return visits;
        }

        public bool IsWhite() {
            return white;
        }

        public Move GetMove() {
            return move;
        }

        // A method to add a child to this node
        // This could probably use a better implementation
        // Maybe the storage of children needs a better data type?
        public void AddChild(Node node) {
            // Creating a new array for the old children plus our new one
            Node[] newChildren = new Node[children.Length + 1];

            // Adding old children into the array
            for (int i = 0; i < children.Length; i++) {
                newChildren[i] = children[i];
            }

            // Adding new child to end of list
            newChildren[^1] = node;

            // Setting children to new array
            children = newChildren;
        }

        // A method to increment wins by given input
        // Increase by 1 for a win, 0.5 for a draw, 0 for a loss
        public void IncrementWins(float inc) {
            // Throwing an error if the input isn't acceptable
            if (inc != 0 && inc != 0.5 && inc != 1) {
                throw new System.ArgumentException(string.Format("Node.IncrementWins: argument must be 0, 0.5, or 1, not {0}", inc));
            }
            // Incrementing wins
            wins += inc;

            // Incrementing visits
            visits++;
        }

        // Overriding equals method
        // Equal if move is equal and color is equal
        public override bool Equals(object? obj)
        {
            // If input object is null, return false
            if (obj == null) {
                return false;
            }
            // Otherwise, convert to a Node object
            Node? input = obj as Node;
            // Returning true if same move
            return this.GetMove().Equals(input.GetMove()) && this.IsWhite() == input.IsWhite();
        }

        public override int GetHashCode()
        {
            return this.GetMove().GetHashCode();
        }
    }

}