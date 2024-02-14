using Transverse._Common.SequenceParsing.Generic;
using System.Diagnostics;

namespace Transverse._Common.SequenceParsing.Char;


public delegate string DelimiterComputer(string x);


[DebuggerDisplay("CharBlockDelimiter - Delimiter={Delimiter}, IsComputedDelimiter={IsComputedDelimiter}, CaseSensitive={CaseSensitive}")]
public record CharBlockDelimiter : BlockDelimiter<char>
{

    public string? Delimiter { get; }
    public bool CaseSensitive { get; }
    public DelimiterComputer? FComputeDelimiter { get; }
    public bool IsComputedDelimiter => (FComputeDelimiter is not null);

    public StringMatchingEvaluator StringMatchingEvaluator { get; }


    private CharBlockDelimiter(string? delimiter, DelimiterComputer? fComputeDelimiter, StringMatchingEvaluator stringMatchingEvaluator, bool caseSensitive = true) : base()
    {
        CaseSensitive = caseSensitive;

        StringMatchingEvaluator = stringMatchingEvaluator;

        Delimiter = delimiter;
        FComputeDelimiter = fComputeDelimiter;


        if ((FComputeDelimiter is null) && (Delimiter is null)) //Ne devrait pas pouvoir se produire compte-tenu des possibilités (Create()) de création exposées.
        {
            throw new Exception($"Au moins un des membres FComputeDelimiter ou Delimiter, doit avoir été renseigné.");
        }
        if ((FComputeDelimiter is not null) && (Delimiter is not null)) //Ne devrait pas pouvoir se produire compte-tenu des possibilités (Create()) de création exposées.
        {
            throw new Exception($"Seul un des membres FComputeDelimiter ou Delimiter, doit avoir été renseigné.");
        }
    }

    public static CharBlockDelimiter Create(string delimiter, StringMatchingEvaluator stringMatchingEvaluator, bool caseSensitive = true)
    {
        CharBlockDelimiter result = new(delimiter, null, stringMatchingEvaluator, caseSensitive);
        return result;
    }

    public static CharBlockDelimiter Create(DelimiterComputer fComputeDelimiter, StringMatchingEvaluator stringMatchingEvaluator, bool caseSensitive = true)
    {
        CharBlockDelimiter result = new(null, fComputeDelimiter, stringMatchingEvaluator, caseSensitive);
        return result;
    }

    public CharBlockDelimiter GetClone(string delimiter)
    {
        CharBlockDelimiter result = Create(delimiter, StringMatchingEvaluator, CaseSensitive);
        return result;
    }
    public CharBlockDelimiter GetClone(DelimiterComputer fComputeDelimiter)
    {
        CharBlockDelimiter result = Create(fComputeDelimiter, StringMatchingEvaluator, CaseSensitive);
        return result;
    }

    public override ChunkMatching<char>? GetMatching(Chunk<char> chunk)
    {
        ChunkMatching<char>? chunkMatching = null;

        string chunkAsString = GetChunkAsString(chunk);

        if (Delimiter is null)
        {
            throw new Exception("A cet endroit du code, la property Delimiter ne devrait pas être null.");
        }

        string? matchingString = StringMatchingEvaluator.GetMatching(Delimiter, chunkAsString, CaseSensitive);

        if (matchingString is not null)
        {
            chunkMatching = ChunkMatching<char>.Create(chunk, matchingString.ToList());
        }

        return chunkMatching;
    }

    private static string GetChunkAsString(Chunk<char> chunk)
    {
        char[] chars = chunk.Content.ToArray();
        string str = string.Join("", chars);

        return str;
    }

    public override string ToString()
    {
        string result = $"{Delimiter}";
        return result;
    }

    public override string GetStateAsString()
    {
        List<string> state = new()
        {
            $"{GetType().GetSimpleName()}: Delimiter='{Delimiter}'",
            $"CaseSensitive={CaseSensitive}",
            $"StringMatchingEvaluator={StringMatchingEvaluator.GetType().GetSimpleName()}"
        };
        string result = "{" + string.Join("; ", state) + "}";
        return result;
    }
}
