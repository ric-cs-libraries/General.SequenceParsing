using System.Text.RegularExpressions;

namespace General.SequenceParsing.Char;

public class StringMatchingEvaluatorByRegExp : StringMatchingEvaluator
{
    //ATTENTION : . si la regExp reçue, sera SYSTÉMATIQUEMENT entourée par : "^()" la regExp considérée sera donc : "^(regExp)"    !!!!!
    public override string? GetMatching(string regExp, string stringToEvaluate, bool caseSensitive)
    {
        string? matchingString = null;

        if (string.IsNullOrEmpty(regExp))
        {
            throw new Exception("RegExp vide !");
        }

        Regex regEx = CreateRegExp(regExp, caseSensitive); //ATTENTION : cf. entête de mise en garde ci-dessus

        Match matching;

        if ((matching = regEx.Match(stringToEvaluate)).Success)
        {
            matchingString = matching.Groups[1].Value;
        }

        return matchingString;
    }
}



