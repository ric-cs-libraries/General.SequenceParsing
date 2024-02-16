using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using General.Basics.Extensions;

namespace General.SequenceParsing.Generic;

[DebuggerDisplay("Sequence - ContentToString={string.Join(\"\", Content)}")]
public class Sequence<TTT>
{
    private List<TTT> Content { get; init; } = null!;

    protected Sequence(TTT[] sequence, int startIndex = 0, int? endIndex = null)
    {
        endIndex ??= sequence.Length - 1;
        Content = sequence.GetChunk_(startIndex, endIndex.Value).ToList();
    }

    public static Sequence<TTT> Create(TTT[] sequence, int startIndex = 0, int? endIndex = null)
    {
        Sequence<TTT> result = new(sequence, startIndex, endIndex);
        return result;
    }

    public TTT? GetAtomicValueAtIndex(int index)
    {
        if (index >= 0 && index < Content.Count)
        {
            return Content.ElementAt(index);
        }
        return default;
    }
    public Chunk<TTT>? GetChunk(int startIndex)
    {
        return GetChunk(startIndex, GetLastIndex());
    }
    public Chunk<TTT>? GetChunk(int startIndex, int endIndex)
    {
        if (startIndex >= 0 && endIndex < Content.Count && endIndex >= startIndex)
        {
            List<TTT> content = Content.Skip(startIndex).Take(endIndex - startIndex + 1).ToList();
            return Chunk<TTT>.Create(startIndex, content);
        }
        return null;
    }
    private int GetLastIndex()
    {
        return Content.Count - 1;
    }

    public string GetStateAsString()
    {
        List<string> state = new()
        {
            $"{GetType().GetSimpleName()}: Content({Content.Count})='{ToString()}'",
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
