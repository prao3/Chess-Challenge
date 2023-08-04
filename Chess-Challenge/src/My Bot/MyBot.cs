using ChessChallenge.API;
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
        // TODO: Include if node is terminal
        // TODO: Also at a leaf if not all possible moves are in tree
        if (root.GetChildren().Length == 0) {
            // Returning the stack
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
            float newScore = child.GetWins() / child.GetVisits() + (float)Sqrt(2*root.GetVisits() / child.GetVisits());
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
    }

}