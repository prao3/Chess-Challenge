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
}