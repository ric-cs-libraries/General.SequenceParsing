using System.Diagnostics;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Transverse._Common.SequenceParsing.Generic;

[DebuggerDisplay("Chunk - StartIndex={StartIndex}, ContentToString={string.Join(\"\", Content)}")]
public record Chunk<TTT>
{
    public int StartIndex { get; }

    public List<TTT> Content { get; }


    private Chunk(int startIndex, List<TTT> content)
    {
        StartIndex = startIndex;
        Content = content;
    }

    public static Chunk<TTT> Create(int startIndex, List<TTT> content)
    {
        Chunk<TTT> result = new(startIndex, content);
        return result;
    }

    public string GetStateAsString()
    {
        List<string> state = new()
        {
            $"{GetType().GetSimpleName()}: StartIndex={StartIndex}",
            $"Content({Content.Count})='{ToString()}'"
        };
        string result = "{" + string.Join("; ", state) + "}";
        return result;
    }


    public override string ToString()
    {
        var result = string.Join("", Content.Select(e => e?.ToString()));
        return result;
    }

}
