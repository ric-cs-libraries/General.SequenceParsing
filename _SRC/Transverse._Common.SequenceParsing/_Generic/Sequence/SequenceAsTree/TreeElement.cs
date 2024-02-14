namespace Transverse._Common.SequenceParsing.Generic;

public abstract class TreeElement<TTT>
{
    public int Id { get; }
    public int ParentId { get; }

    public int Depth { get; }

    private const int INITIAL_ID = -1;
    private static int CurrentId = INITIAL_ID;
    private static int GetNextID => ++CurrentId;

    public abstract void AcceptVisitor(IVisitor<TTT> visitor);

    protected TreeElement(int parentId, int depth)
    {
        Id = GetNextID;

        ParentId = parentId;
        Depth = depth;
    }

    public static void ResetId()
    {
        CurrentId = INITIAL_ID;
    }

    public virtual string GetStateAsString()
    {
        List<string> state = new()
        {
            $"Id={Id}", 
            $"ParentId={ParentId}", 
            $"Depth={Depth}"
        };
        string result = $"{GetType().GetSimpleName()}: "+string.Join("; ", state);
        return result;
    }
}

