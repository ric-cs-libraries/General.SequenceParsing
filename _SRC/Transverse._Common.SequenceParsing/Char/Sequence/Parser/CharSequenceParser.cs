using Transverse._Common.SequenceParsing.Generic;

namespace Transverse._Common.SequenceParsing.Char;

public class CharSequenceParser : SequenceParser<char>
{
    private CharSequenceParser(CharSequence sequence) : base(sequence)
    {
    }

    public static CharSequenceParser Create(CharSequence sequence)
    {
        CharSequenceParser result = new CharSequenceParser(sequence);
        return result;
    }
}
