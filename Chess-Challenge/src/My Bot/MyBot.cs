using ChessChallenge.API;

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