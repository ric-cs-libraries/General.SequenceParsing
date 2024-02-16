using System.Diagnostics;

namespace General.SequenceParsing.Generic;

[DebuggerDisplay("Leaf (Length={Data.Count}), DataToString={string.Join(\"\",Data)}, Id={Id}, ParentId={ParentId}, Depth={Depth}")]
public class Leaf<TTT> : TreeElement<TTT>
{
    const string CRLF_ReplacementString = "~~";

    public string Debug_DataToString => string.Join("", Data); //JUSTE pour debuggage
    public List<TTT> Data { get; } = new();

    public int StartIndex { get; set; }
    public int EndIndex => StartIndex + Data.Count - 1;

    protected Leaf(int parentId, int depth, int startIndex) : base(parentId, depth)
    {
        StartIndex = startIndex;
    }

    internal static Leaf<TTT> Create(int parentId, int depth, int startIndex)
    {
        Leaf<TTT> result = new Leaf<TTT>(parentId, depth, startIndex);
        return result;
    }

    public override void AcceptVisitor(IVisitor<TTT> visitor)
    {
        visitor.Visit(this);
    }

    public string GetDataAsString()
    {
        var result = $"({Data.Count})='{GetRawDataAsString()}'";
        return result;
    }
    public string GetRawDataAsString()
    {
        var result = $"{string.Join("", Data.Select(d => d?.ToString()?.Replace(Environment.NewLine, CRLF_ReplacementString)))}";
        return result;
    }

    public override string GetStateAsString()
    {
        List<string> state = new()
        {
            $"{base.GetStateAsString()}",
            $"StartIndex={StartIndex}",
            $"EndIndex={EndIndex}",
            $"Data{GetDataAsString()}"
        };
        string result = "{" + string.Join("; ", state) + "}";
        return result;
    }
}
