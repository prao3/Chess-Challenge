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

        // Returning the best move
        return bestMove;
    }
    
}