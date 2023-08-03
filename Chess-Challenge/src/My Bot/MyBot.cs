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
        public Node[] getChildren(){
            return children;
        }

        public float getWins(){
            return wins;
        }

        public int getVisits(){
            return visits;
        }

        public bool isWhite(){
            return white;
        }
    }

}