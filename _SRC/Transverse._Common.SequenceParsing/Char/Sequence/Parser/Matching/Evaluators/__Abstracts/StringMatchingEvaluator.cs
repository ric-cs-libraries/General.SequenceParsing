using System.Text.RegularExpressions;

namespace Transverse._Common.SequenceParsing.Char;

public abstract class StringMatchingEvaluator
{
    private const string BEGIN_PARENTHESIS = "(";
    private const string END_PARENTHESIS = ")";

    private const string BEGIN_REGEXP = "^";

    protected StringComparison stringComparison(bool caseSensitive) => (caseSensitive) ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;

    public abstract string? GetMatching(string referenceString, string stringToEvaluate, bool caseSensitive);

    protected Regex CreateRegExp(string regExp, bool caseSensitive)
    {
        regExp = $"{BEGIN_REGEXP}{BEGIN_PARENTHESIS}{regExp}{END_PARENTHESIS}";

        RegexOptions regExpOptions = (caseSensitive) ? RegexOptions.None : RegexOptions.IgnoreCase;

        return new Regex(regExp, regExpOptions);
    }

}



