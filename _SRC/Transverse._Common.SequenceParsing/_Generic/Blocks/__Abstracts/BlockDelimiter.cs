namespace Transverse._Common.SequenceParsing.Generic;

public abstract record BlockDelimiter<TTT>
{
    public abstract ChunkMatching<TTT>? GetMatching(Chunk<TTT> chunk);
    public abstract string GetStateAsString();

    public override string ToString()
    {
        string result = $"BlockDelimiter - (please override ToString() in Children classes)";
        return result;
    }
}
