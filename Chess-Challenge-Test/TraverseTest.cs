namespace Chess_Challenge_Test;
using ChessChallenge.API;
using static MyBot;

[TestClass]
public class TraverseTest {

    // Board at beginning of a chess game
    private readonly Board StartBoard;
    // King and pawn each board
    private readonly Board KPBoard;
    // Checkmating board
    private readonly Board CheckBoard;
    
    public TraverseTest() {

    }

    // Testing Traverse when root has no children
    [TestMethod]
    public void RootNoChildrenTest() 
    {
        Assert.IsTrue(false, "Not Implemented");
    }

    // Testing Traverse for travelling 'deep' in tree
    [TestMethod]
    public void DeepTraverseTest() 
    {
        Assert.IsTrue(false, "Not Implemented");
    }

    // Testing Traverse for a shallower travel in tree
    [TestMethod]
    public void ShallowTraverseTest() 
    {
        Assert.IsTrue(false, "Not Implemented");
    }

    // Testing Traverse for a node not fully populated with children
    [TestMethod]
    public void IncompleteNodeTest() 
    {
        Assert.IsTrue(false, "Not Implemented");
    }

    // Testing Traverse for a completely unexplored node
    [TestMethod]
    public void UnexploredNodeTest() 
    {
        Assert.IsTrue(false, "Not Implemented");
    }

    // Testing Traverse for decisive result!
    [TestMethod]
    public void CheckmateTest() 
    {
        Assert.IsTrue(false, "Not Implemented");
    }
}