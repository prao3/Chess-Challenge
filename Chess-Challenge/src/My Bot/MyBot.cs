using ChessChallenge.API;
using System;
using System.Collections.Generic;

public class MyBot : IChessBot
{
    public Move Think(Board board, Timer timer)
    {
        // RNG
        Random random = new Random();
        // Legal moves on the board
        Move[] moves = board.GetLegalMoves();
        // Random index to pick from
        int randomIndex = random.Next(moves.Length);
        // Returning a random move
        return moves[randomIndex];
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