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
            // Evaluating the position
            float eval = Evaluate(board, m);
            // If this is a better move than our previous best, save it
            if (eval > bestEval) {
                bestEval = eval;
                bestMove = m;
            }
        }

        // Waiting some amount of time so I can see the moves being made in testing
        while(timer.MillisecondsElapsedThisTurn < 1000) {

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
    private float Evaluate(Board board, Move move) {
        // If we are white, we want to count black's squares
        // If we are black, we want to count white's squares
        // So, in the for loop below, if we are white, we add 32 to the counter, otherwise we add 0
        int boardSpace = board.IsWhiteToMove ? 32 : 0;
        // Making the move on the board
        board.MakeMove(move);
        // A variable to count the squares we attack
        int weAttack = 0;
        // Checking and counting the squares we attack
        // Only counting squares in the opponent's position
        for (int i = 0; i < 32; i++) {
            if (board.SquareIsAttackedByOpponent(new Square(i + boardSpace))) {
                weAttack++;
            }
        }
        // Undoing our move
        board.UndoMove(move);
        // Score of position
        float Score = weAttack;
        // Returning score
        return Score;
    }
}