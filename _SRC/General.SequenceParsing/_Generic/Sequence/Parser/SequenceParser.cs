namespace General.SequenceParsing.Generic;

public class SequenceParser<TTT>
{
    private int sequenceCurrentIndex = -1;
    private readonly Sequence<TTT> sequence;

    protected SequenceParser(Sequence<TTT> sequence)
    {
        this.sequence = sequence;
    }

    public static SequenceParser<TTT> Create(Sequence<TTT> sequence)
    {
        SequenceParser<TTT> result = new(sequence);
        return result;
    }

    public ParseResult<TTT> Parse(Blocks<TTT> expectedInnerBlocks) //expectedInnerBlocks: Blocks(à rechercher) et de profondeur 0
    {
        TreeElement<TTT>.ResetId();
        ParseResult<TTT> scanResult = ParseResult<TTT>.Create(expectedInnerBlocks);

        Parse(scanResult.RootNode);

        return scanResult;
    }

    private void Parse(RootNode<TTT> currentNode)
    {
        Parse(currentNode, currentNode.ExpectedInnerBlocks); //la property Block n'est pas renseignée sur le RootNode
    }

    private void Parse(Node<TTT> currentNode)
    {
        Parse(currentNode, currentNode.Block.ExpectedInnerBlocks);
    }

    private void Parse(Node<TTT> currentNode, Blocks<TTT>? expectedInnerBlocks)
    {
        bool isRootNode = currentNode.Block is null; //si la property Block n'est pas renseignée alors currentNode est le RootNode.

        Chunk<TTT>? chunk = null;
        BlockMatching<TTT>? endBlockMatchingChunk = null;
        while ((chunk = GetNextChunk()) is not null)
        {
            if ((!isRootNode) //Pas de notion de fin(ou début) de Block pour le RootNode lui-même.
                 && (endBlockMatchingChunk = GetEndBlockMatching(currentNode.Block!, chunk)) is not null) //currentNode.Block existe puisque pas RootNode.
            {
                //Fin de Block trouvée pour le noeud currentNode (currentNode.Block), on marque donc currentNode en tant que clos :
                currentNode.Close(endBlockMatchingChunk.ChunkMatching);
                //if (currentNode.Elements.Count == 1  && currentNode.Elements[0] is Leaf<char> leaf) //Juste pour DEBUG
                //{
                //    File.AppendAllText("T:/NodesMonoLeaf.txt", leaf.GetStateAsString()+Environment.NewLine);
                //}
                sequenceCurrentIndex = endBlockMatchingChunk.ChunkMatching.EndIndex;
                break; //donc retour à l'appelant (noeud parent).
            }
            else
            {
                BlockMatching<TTT>? firstBlockMatchingChunk;
                if ((expectedInnerBlocks is not null) //Si des Blocks internes sont à rechercher
                     && (firstBlockMatchingChunk = GetFirstStartBlockMatchingChunk(expectedInnerBlocks, chunk)) is not null) //Recherche d'un marqueur de début de Block matchant avec chunk.
                {
                    //On a trouvé un marqueur de début de Block, on crée un noeud :
                    Node<TTT> childNode = currentNode.AddNewNode(firstBlockMatchingChunk.Block, firstBlockMatchingChunk.ChunkMatching);

                    sequenceCurrentIndex = firstBlockMatchingChunk.ChunkMatching.EndIndex;
                    Parse(childNode); //Recursion pour traiter le contenu de ce noeud enfant
                }
                else
                {
                    //On n'est pas sur un marqueur de début de Block, on est donc sur une Leaf :
                    Leaf<TTT> leaf = currentNode.GetLeafAsLastAddedElement(sequenceCurrentIndex); //Prend la Leaf en cours ou en crée une nouvelle.
                    TTT currentAtomicValue = sequence.GetAtomicValueAtIndex(sequenceCurrentIndex)!;
                    leaf.Data.Add(currentAtomicValue);
                }
            }
        }

        bool parsingTerminated = chunk is null; //true => plus rien à lire
        if (parsingTerminated && isRootNode)
        {
            ((RootNode<TTT>)currentNode).Close();
        }

        currentNode.CheckCompleteness();
    }

    private BlockMatching<TTT>? GetFirstStartBlockMatchingChunk(Blocks<TTT> searchedInnerBlocks, Chunk<TTT> chunk)
    {
        BlockMatching<TTT>? blockMatching = null;

        foreach (Block<TTT> block in searchedInnerBlocks.List)
        {
            blockMatching = GetStartBlockMatching(block, chunk);
            if (blockMatching is not null)
            {
                break;
            }
        }
        return blockMatching;
    }

    private BlockMatching<TTT>? GetStartBlockMatching(Block<TTT> block, Chunk<TTT> chunk)
    {
        BlockMatching<TTT>? blockMatching = null;
        ChunkMatching<TTT>? chunkMatching = block.StartDelimiter.GetMatching(chunk);
        if (chunkMatching is not null)
        {
            blockMatching = BlockMatching<TTT>.Create(block, chunkMatching, matchForStartDelimiter: true);
        }
        return blockMatching;
    }
    private BlockMatching<TTT>? GetEndBlockMatching(Block<TTT> block, Chunk<TTT> chunk)
    {
        BlockMatching<TTT>? blockMatching = null;
        ChunkMatching<TTT>? chunkMatching = block.EndDelimiter.GetMatching(chunk);
        if (chunkMatching is not null)
        {
            blockMatching = BlockMatching<TTT>.Create(block, chunkMatching, matchForStartDelimiter: false);
        }
        return blockMatching;
    }
    private Chunk<TTT>? GetNextChunk()
    {
        sequenceCurrentIndex++;
        Chunk<TTT>? chunk = sequence.GetChunk(sequenceCurrentIndex);
        return chunk;
    }
}
