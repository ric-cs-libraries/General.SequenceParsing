using System.Diagnostics;
using System.Xml.Linq;

namespace Transverse._Common.SequenceParsing.Generic;

//Valeur (et position) exactes d'un Matching.

[DebuggerDisplay("ChunkMatching - StartIndex={StartIndex}, ContentToString={string.Join(\"\", Content)}")]
public record ChunkMatching<TTT>
{
    public List<TTT> Content { get; } //Valeur exacte du Matching.
    public int StartIndex { get; }
    public int EndIndex => StartIndex + Content.Count - 1;

    private ChunkMatching(int startIndex, List<TTT> content)
    {
        StartIndex = startIndex;
        Content = content;
    }
    public static ChunkMatching<TTT> Create(Chunk<TTT> chunk, List<TTT> content)
    {
        ChunkMatching<TTT> chunkMatching = new ChunkMatching<TTT>(chunk.StartIndex, content);
        return chunkMatching;
    }
    public string GetContentAsString()
    {
        var result = $"({Content.Count})=`{GetRawContentAsString()}`";
        return result;
    }

    public string GetRawContentAsString()
    {
        var result = $"{string.Join("", Content.Select(e => e?.ToString()))}";
        return result;
    }

    public string GetStateAsString()
    {
        List<string> state = new()
        {
            $"{GetType().GetSimpleName()}: StartIndex={StartIndex}",
            $"EndIndex={EndIndex}",
            $"Content{GetContentAsString()}"
        };
        string result = "{" + string.Join("; ", state) + "}";
        return result;
    }

    public Serializable GetStateAsSerializable()
    {
        Serializable state = new(GetContentAsString(), StartIndex, EndIndex);
        return state;
    }

    public record Serializable(string MatchOn, int StartIndex, int EndIndex);
}
