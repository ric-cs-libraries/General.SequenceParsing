using General.SequenceParsing.Generic;

namespace General.SequenceParsing.Char;

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
