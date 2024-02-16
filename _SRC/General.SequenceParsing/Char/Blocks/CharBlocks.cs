using General.SequenceParsing.Generic;

namespace General.SequenceParsing.Char;

public record CharBlocks : Blocks<char>
{
    private CharBlocks(List<Block<char>> list) : base(list)
    {
    }

    public static CharBlocks Create(List<CharBlock> list)
    {
        List<Block<char>> adapetedList = new(list); //Car conversion List<CharBlock> en List<Block<char>> non permise.

        CharBlocks result = new(adapetedList);
        return result;
    }
}
