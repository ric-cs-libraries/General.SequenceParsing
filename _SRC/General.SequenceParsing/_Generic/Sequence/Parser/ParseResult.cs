namespace General.SequenceParsing.Generic;

public class ParseResult<TTT>
{
    public RootNode<TTT> RootNode { get; }

    protected ParseResult(Blocks<TTT> expectedInnerBlocks)
    {
        RootNode = RootNode<TTT>.Create(expectedInnerBlocks);
    }

    public static ParseResult<TTT> Create(Blocks<TTT> expectedInnerBlocks)
    {
        ParseResult<TTT> result = new(expectedInnerBlocks);
        return result;
    }

    public string GetStateAsString()
    {
        List<string> state = new()
        {
            $"{GetType().GetName_()}: RootNode={RootNode.GetStateAsString()}"
        };
        string result = "{" + string.Join("; ", state) + "}";
        return result;
    }
}
