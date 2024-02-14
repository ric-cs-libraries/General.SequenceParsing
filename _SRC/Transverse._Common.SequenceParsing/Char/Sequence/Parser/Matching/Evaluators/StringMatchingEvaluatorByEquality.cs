using System;

namespace Transverse._Common.SequenceParsing.Char;

public class StringMatchingEvaluatorByEquality : StringMatchingEvaluator
{
    public override string? GetMatching(string referenceString, string stringToEvaluate, bool caseSensitive)
    {
        string? stringMatching = null;

        if (stringToEvaluate.StartsWith(referenceString, stringComparison(caseSensitive)))
        {
            stringMatching = stringToEvaluate.AsSpan(0, referenceString.Length).ToString();
        }

        return stringMatching;
    }
}



