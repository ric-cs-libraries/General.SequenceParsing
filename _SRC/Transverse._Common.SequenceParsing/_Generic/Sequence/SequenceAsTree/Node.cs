using System.Diagnostics;

namespace Transverse._Common.SequenceParsing.Generic;

[DebuggerDisplay("Node - For {(Block is null)? \"ROOT\" : Block.Type}, Nb. Children={Elements.Count}, Id={Id}, ParentId={ParentId}, Depth={Depth}, IsClosed={IsClosed}")]
public class Node<TTT> : TreeElement<TTT>
{

    public Block<TTT> Block { get; } = null!;

    public ChunkMatching<TTT> ChunkStartMatching { get; } = null!;
    public ChunkMatching<TTT> ChunkEndMatching { get; private set; } = null!;

    protected virtual bool IsClosed => (ChunkStartMatching is not null) && (ChunkEndMatching is not null);

    public List<TreeElement<TTT>> Elements { get; } = new();



    protected Node(int parentId, int depth) : base(parentId, depth)
    {
    }

    private Node(int parentId, int depth, Block<TTT> block, ChunkMatching<TTT> chunkStartMatching) : this(parentId, depth)
    {
        Block = block;
        ChunkStartMatching = chunkStartMatching;
    }

    private static Node<TTT> Create(int parentId, int depth, Block<TTT> block, ChunkMatching<TTT> chunkStartMatching)
    {
        Node<TTT> result = new Node<TTT>(parentId, depth, block, chunkStartMatching);
        return result;
    }

    public override void AcceptVisitor(IVisitor<TTT> visitor)
    {
        visitor.Visit(this);
        Elements.ForEach(e => e.AcceptVisitor(visitor));
    }

    internal Node<TTT> AddNewNode(Block<TTT> block, ChunkMatching<TTT> chunkStartMatching)
    {
        Node<TTT> childNode = Create(parentId: Id, Depth + 1, block, chunkStartMatching);

        Elements.Add(childNode);

        return childNode;
    }

    internal Leaf<TTT> AddNewLeaf(int startIndex)
    {
        Leaf<TTT> leaf = Leaf<TTT>.Create(parentId: Id, Depth + 1, startIndex);

        Elements.Add(leaf);

        return leaf;
    }


    internal virtual void CheckCompleteness() //<<<VIRTUAL : cf. RootNode class
    {

        if (Block is null)
        {
            throw new Exception($"Node {Id} : Block should not be null !");
        }

        if (ChunkStartMatching is null)
        {
            throw new Exception($"Node {Id} : ChunkStartMatching should not be null !");
        }

        CheckIsClosed();
    }

    protected void CheckIsClosed()
    {
        if (!IsClosed)
        {
            throw new Exception($"Node {Id} has not been closed !");
        }
    }

    internal void Close(ChunkMatching<TTT> chunkEndMatching)
    {
        SetChunkEndMatching(chunkEndMatching);
    }

    private void SetChunkEndMatching(ChunkMatching<TTT> chunkEndMatching)
    {
        if (ChunkStartMatching is null)
        {
            throw new Exception($"Node {Id} : ChunkStartMatching must have been set before tyring to set ChunkEndMatching !");
        }
        ChunkEndMatching = chunkEndMatching;
    }

    internal Leaf<TTT> GetLeafAsLastAddedElement(int startIndex)
    {
        TreeElement<TTT>? lastAddedElement = GetLastAddedElement();

        if (lastAddedElement is not Leaf<TTT> leaf)
        {
            leaf = AddNewLeaf(startIndex);
        }
        return leaf;
    }

    private TreeElement<TTT>? GetLastAddedElement()
    {
        return Elements.Any() ? Elements.ElementAt(Elements.Count - 1) : null;
    }

    public virtual string GetSimplifiedStateAsString()
    {
        List<string> state = new()
        {
            $" StartDelim{ChunkStartMatching.GetContentAsString()}",
            $"EndDelim{ChunkEndMatching.GetContentAsString()}"
        };
        string result = string.Join("; ", state);
        return result;
    }

    public override string GetStateAsString()
    {
        List<string> state = new()
        {
            $"{base.GetStateAsString()}",
            $"Block={Block?.GetStateAsString()}",
            $"ChunkStartMatching={ChunkStartMatching?.GetStateAsString()}",
            $"ChunkEndMatching={ChunkEndMatching?.GetStateAsString()}",
            $"IsClosed={IsClosed}",
            $"Elements({Elements.Count})=[{string.Join(",",Elements.Select(te=>te.GetStateAsString()))}]"
        };
        string result = "{" + string.Join("; ", state) + "}";
        return result;
    }
}