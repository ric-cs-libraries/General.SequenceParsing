using System.Text.RegularExpressions;
using Transverse._Common.SequenceParsing.Generic;

namespace Transverse._Common.SequenceParsing.Char;

public class CharSequence : Sequence<char>
{
    private CharSequence(string sequence, int startIndex = 0, int? endIndex = null) : base(sequence.ToArray(), startIndex, endIndex)
    {
    }

    public static CharSequence Create(string sequence, int startIndex = 0, int? endIndex = null)
    {
        CharSequence result = new CharSequence(sequence, startIndex, endIndex);
        return result;
    }

    public static CharSequence Create(string sequence, Regex startAfterThisRegExpMatch, int? endIndex = null)
    {
        int startIndex = GetStartIndexFromStartAfterRegExp(sequence, startAfterThisRegExpMatch);

        CharSequence result = Create(sequence, startIndex, endIndex);

        return result;
    }

    public static CharSequence Create(string sequence, int startIndex, Regex endAtThisRegExpMatch)
    {
        int endIndex = GetEndIndexFromEndAtRegExp(sequence, endAtThisRegExpMatch);

        CharSequence result = Create(sequence, startIndex, endIndex);

        return result;
    }

    public static CharSequence Create(string sequence, Regex startAfterThisRegExpMatch, Regex endAtThisRegExpMatch)
    {
        int startIndex = GetStartIndexFromStartAfterRegExp(sequence, startAfterThisRegExpMatch);
        int endIndex = GetEndIndexFromEndAtRegExp(sequence, endAtThisRegExpMatch);

        CharSequence result = Create(sequence, startIndex, endIndex);

        return result;
    }

    private static int GetStartIndexFromStartAfterRegExp(string sequence, Regex startAfterThisRegExpMatch)
    {
        Match? match = startAfterThisRegExpMatch.Match(sequence);
        int startIndex = match.Index + match.Length; // 0 + 0 en cas de non succès
        return startIndex;
    }
    private static int GetEndIndexFromEndAtRegExp(string sequence, Regex endAtThisRegExpMatch)
    {
        Match? match = endAtThisRegExpMatch.Match(sequence);
        int endIndex = ((match.Success)? match.Index : sequence.Length) -1;
        return endIndex;
    }
}
