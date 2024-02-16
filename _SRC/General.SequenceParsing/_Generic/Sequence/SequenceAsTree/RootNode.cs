using System.Xml.Linq;

namespace General.SequenceParsing.Generic;

public class RootNode<TTT> : Node<TTT>
{
    //REMARQUE : la property Block, du RootNode vaudra toujours null.

    private const int NO_PARENT_ID = -1;
    private const int NO_DEPTH = -1;

    private bool isClosed;

    public Blocks<TTT> ExpectedInnerBlocks { get; }

    protected override bool IsClosed => isClosed;

    private RootNode(Blocks<TTT> expectedInnerBlocks) : base(parentId: NO_PARENT_ID, depth: NO_DEPTH)
    {
        ExpectedInnerBlocks = expectedInnerBlocks;
        //property Block, is null
    }

    internal static RootNode<TTT> Create(Blocks<TTT> expectedInnerBlocks)
    {
        RootNode<TTT> result = new RootNode<TTT>(expectedInnerBlocks);
        return result;
    }

    internal override void CheckCompleteness()
    {
        CheckIsClosed();
    }

    internal void Close()
    {
        isClosed = true;
    }

    public override string GetSimplifiedStateAsString()
    {
        List<string> state = new()
        {
            $"({Elements.Count})"
        };
        string result = string.Join("; ", state);
        return result;
    }

    public override string GetStateAsString()
    {
        List<string> state = new()
        {
            $"{GetType().GetSimpleName()}: IsClosed={IsClosed}",
            $"Elements({Elements.Count})=[{string.Join(",",Elements.Select(te=>te.GetStateAsString()))}]",
            $"ExpectedInnerBlocks({ExpectedInnerBlocks?.List.Count ?? 0})"
        };
        string result = "{" + string.Join("; ", state) + "}";
        return result;
    }
}