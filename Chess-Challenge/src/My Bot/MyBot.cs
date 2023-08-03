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
        private Node[] children = new Node[0];

        // Counting wins and draws for trajectories including this node
        private float wins = 0;

        // Counting visits to this node in MCTS
        private int visits = 0;

        // Is this node for white's turn or black's turn?
        private bool white = true;

        // Getter methods
        public Node[] getChildren() {
            return children;
        }

        public float getWins() {
            return wins;
        }

        public int getVisits() {
            return visits;
        }

        public bool isWhite() {
            return white;
        }

        // A method to add a child to this node
        // This could probably use a better implementation
        // Maybe the storage of children needs a better data type?
        public void addChild(Node node) {
            // Creating a new array for the old children plus our new one
            Node[] newChildren = new Node[children.Length + 1];

            // Adding old children into the array
            for (int i = 0; i < children.Length; i++) {
                newChildren[i] = children[i];
            }

            // Adding new child to end of list
            newChildren[newChildren.Length - 1] = node;

            // Setting children to new array
            children = newChildren;
        }
    }

}