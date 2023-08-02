using ChessChallenge.API;
using System;
using System.Collections.Generic;

public class MyBot : IChessBot
{
    public Move Think(Board board, Timer timer)
    {
        // Legal moves on the board
        Move[] moves = board.GetLegalMoves();

        // A variable to store the best move
        Move bestMove = moves[0];
        // Storing that eval also so we don't have to do it twice
        // Maybe don't do this since namespaces take up memory
        float bestEval = 0;
        // Evaluating each move and keeping the one with the highest eval
        foreach (Move m in moves) {
            // Making the move on the board
            board.MakeMove(m);
            // Evaluating the position
            float eval = Evaluate(board);
            // If this is a better move than our previous best, save it
            if (eval > bestEval) {
                bestEval = eval;
                bestMove = m;
            }
            // Undoing the move so we can evaluate the rest
            board.UndoMove(m);
        }

        // Returning a random move
        return bestMove;
    }

    /*
    A method to return a numerical value of a position.
    If we think white is better, returns a positive number.
    If we think black is better, returns a negative number.
    Zero is neutral/draw.

    Parameters:
        board: the board to evaluate

    Returns:
        A float representing the numerical evaluation of a position
    */
    private float Evaluate(Board board) {
        return 0;
    }
}