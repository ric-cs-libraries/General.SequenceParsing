using System.Diagnostics;

namespace General.SequenceParsing.Generic;

[DebuggerDisplay("BlockMatching - Block.Type={Block.Type}, ForStartDelimiter={ForStartDelimiter}, ChunkMatching.StartIndex={ChunkMatching.StartIndex}, ChunkMatching.ContentToString={string.Join(\"\", ChunkMatching.Content)}")]
internal record class BlockMatching<TTT>
{
    public Block<TTT> Block { get; private set; } = null!;
    public ChunkMatching<TTT> ChunkMatching { get; } = null!;
    public bool ForStartDelimiter { get; } //true lorsque le matching concerne le Delimiter de début du Block.

    private BlockMatching(Block<TTT> block, ChunkMatching<TTT> chunkMatching, bool matchForStartDelimiter)
    {
        Block = block;
        ChunkMatching = chunkMatching;
        ForStartDelimiter = matchForStartDelimiter;

        OnCreate();
    }

    private void OnCreate()
    {
        //RAPPEL : une fois créée, une instance de Block, NE PEUT ( NE DOIT en effet) être modifiée. (Value Object).
        //         Donc si besoin : en faire une COPIE, etc...
        //         (Ceci, car la référence mémoire d'une instance Block peut être déjà utilisée à divers endroits).
        
        if (ForStartDelimiter)
        {
            Block = Block.OnStartDelimiterMatching_MayNeedANewAdaptedBlock(ChunkMatching.Content);
        }
    }

    public static BlockMatching<TTT> Create(Block<TTT> block, ChunkMatching<TTT> chunkMatching, bool matchForStartDelimiter)
    {
        BlockMatching<TTT> result = new(block, chunkMatching, matchForStartDelimiter);
        return result;
    }

    public string GetStateAsString()
    {
        List<string> state = new()
        {
            $"{GetType().GetSimpleName()}: ForStartDelimiter={ForStartDelimiter}",
            $"Block={Block.GetStateAsString()}",
            $"ChunkMatching={ChunkMatching.GetStateAsString()}"
        };
        string result = "{" + string.Join("; ", state) + "}";
        return result;
    }
}
