using System.Diagnostics;

namespace Transverse._Common.SequenceParsing.Generic;

//RAPPEL : une fois créée, une instance de Block, NE PEUT ( NE DOIT en effet) être modifiée. (Value Object).
//         Donc si besoin : en faire une COPIE, etc...
//         (Ceci, car la référence mémoire d'une instance Block peut être déjà utilisée à divers endroits).


[DebuggerDisplay("Block - Type = {Type}, Nb. ExpectedInnerBlocks={ExpectedInnerBlocks?.List.Count ?? 0}, SelfNestable={SelfNestable}")]
public abstract record Block<TTT>
{
    public string? Type { get; } //Permet de mieux différencier/catégoriser les Block (et aide au debuggage (d'où le "_" pour apparition en 1er (ordre alpha.))
    public BlockDelimiter<TTT> StartDelimiter { get; }
    public BlockDelimiter<TTT> EndDelimiter { get; }

    public bool SelfNestable { get; private set; } //Modifiable qu'en phase de création de l'instance.
    public Blocks<TTT>? ExpectedInnerBlocks { get; private set; } //Sous-Block envisageables (non obligatoires à trouver donc), juste à rechercher.
                                                                  //Modifiable qu'en phase de création de l'instance.


    //Lors du matching du Delimiter de début, on est SUSCEPTIBLE DE vouloir modifier, notamment, le Delimiter de fin, en fonction de la valeur exacte ayant matché.
    //Auquel cas, comme un Block (this) est un Value Object (immutable après création), on va en créer une COPIE modifiée et la renvoyer.
    public abstract Block<TTT> OnStartDelimiterMatching_MayNeedANewAdaptedBlock(IReadOnlyList<TTT> startDelimiterMatchingContent);

    protected Block(
        string type,
        BlockDelimiter<TTT> startDelimiter, BlockDelimiter<TTT> endDelimiter, 
        Blocks<TTT>? expectedInnerBlocks = null, bool canBeNestedWithinItSelf = false)
    {
        Type = type;
        StartDelimiter = startDelimiter;
        EndDelimiter = endDelimiter;
        ExpectedInnerBlocks = expectedInnerBlocks;

        if (canBeNestedWithinItSelf)
        {
            AutoNest();
        }
    }


    protected static string GetDefaultType(BlockDelimiter<TTT> startDelimiter, BlockDelimiter<TTT> endDelimiter)
    {
        string result = $"{startDelimiter.ToString()}...{endDelimiter.ToString()}";
        return result;
    }


    private void AutoNest()
    {
        Blocks<TTT> expectedInnerBlocks = ExpectedInnerBlocks ?? Blocks<TTT>.Create(new List<Block<TTT>>());

        expectedInnerBlocks.List.Add(this);
        ExpectedInnerBlocks = expectedInnerBlocks;

        SelfNestable = true;
    }

    public string GetStateAsString()
    {
        List<string> state = new()
        {
            $"{GetType().GetSimpleName()}: type='{Type}'",
            $"StartDelimiter={StartDelimiter.GetStateAsString()}",
            $"EndDelimiter={EndDelimiter.GetStateAsString()}",
            $"SelfNestable={SelfNestable}",
            $"ExpectedInnerBlocks({ExpectedInnerBlocks?.List.Count ?? 0})" //Je ne mets pas le détail pour éviter une référence circulaire.
        };
        string result = "{"+string.Join("; ", state)+"}";
        return result;
    }
}
