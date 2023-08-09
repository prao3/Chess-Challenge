namespace Chess_Challenge_Test;
using ChessChallenge.API;
using static MyBot;

[TestClass]
public class PlayoutTest {
    [TestMethod]
    public void Test() {
        // Creating initial board
        Board board = Board.CreateBoardFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        // Making root move
        Node root = TraverseTest.MakeNodeFromString("e2e4 0 0 1", board);
        // Making move on board
        board.MakeMove(root.GetMove());
        // Making the stack
        Stack<Node> stack = new();
        // Adding the root to the stack
        stack.Push(root);
        // Doing a playout
        stack = Playout(root, board, stack);

        // Doing some checks
        // Checking that the stack is filled
        Assert.IsTrue(stack.Count > 0);

        // Checking last node is not the root
        Assert.AreNotEqual(root, stack.Pop());

        // Making sure last node is root
        Node LastNode = stack.Pop();
        // Popping all elements
        while (stack.Count > 0) {
            LastNode = stack.Pop();
        }
        // Checking if last node is root
        Assert.AreEqual(root, LastNode);

        // Checking that board is terminal
        Assert.IsTrue(board.IsDraw() || board.IsInCheckmate());
    }
}